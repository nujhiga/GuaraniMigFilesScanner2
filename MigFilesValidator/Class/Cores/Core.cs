using GuaraniMigFilesScanner.Class.Connections;
using GuaraniMigFilesScanner.Class.Cores.Menues;
using GuaraniMigFilesScanner.Class.FilesManagement;
using GuaraniMigFilesScanner.Class.MigFiles;
using GuaraniMigFilesScanner.Class.Scanner;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;
using Npgsql.Schema;
using PasifaeG3Migrations.Class.Extras;
using PasifaeG3Migrations.Modules.M01;
using PasifaeG3Migrations.Modules.M02;
using PasifaeG3Migrations.Modules.M03;
using PasifaeG3Migrations.Modules.M04;
using PasifaeG3Migrations.Modules.M05;
using PasifaeG3Migrations.Modules.M08;
using PasifaeG3Migrations.Modules.M09;
using PasifaeG3Migrations.Modules.M06;

namespace GuaraniMigFilesScanner.Class
{
    public class Core
    {
        public bool IsWorking { get; private set; }
        public bool ConnectionsReady { get; private set; }
        public Dictionary<int, string[]> PaginatedFilePaths { get; private set; }
        public string[] CurrentPageFiles { get; set; }
        public string CurrentMigFilePath { get; private set; }
        public string CurrentMigFileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CurrentMigFilePath)) return string.Empty;
                return CurrentMigFilePath.GetFileName();
            }
        }
        public string CurrentMigFileNameNoExt
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CurrentMigFilePath)) return string.Empty;
                return CurrentMigFilePath.GetFileNameNoExt();
            }
        }

        private Connection[] _connections;
        public Connection CurrentConnection { get; private set; }
        public int CurrentPagesCount { get; set; }

        public Core()
        {
            FileHandler.VerifyPaths();

            PaginatedFilePaths = new Dictionary<int, string[]>();

            TryDeserializeConnections();
        }
        public void TryDeserializeConnections()
        {
            try
            {
                _connections = JsonSerializer.Deserialize<Connection[]>(System.IO.File.ReadAllText(FileHandler.ResourcesPath + "\\Connections.Json"));
            }
            finally
            {
                if (_connections.IsNullOrEmpty())
                    Console.WriteLine("No se pudo deserealizar 'Connections.Json'");
            }
        }

        public async Task InitCore()
        {
            InitializeMigFilesPaths();
            await MenusManager.RunMainMenu(this);
        }
        public async void ResetCore()
        {
            FileHandler.VerifyPaths();

            PaginatedFilePaths = new Dictionary<int, string[]>();

            CurrentPageFiles = null;

            CurrentMigFilePath = string.Empty;

            await InitCore();
        }

        public void SemiResetCore()
        {
            Console.Title = "SIU-Guaraní Controles Pre Migración.";

            CurrentMigFilePath = string.Empty;
        }


        private Dictionary<MigFileType, string> GetAllMigFiles(MigFileType[] types = null)
        {
            Dictionary<MigFileType, string> files = new Dictionary<MigFileType, string>();

            string[] auxFiles = FileHandler.GetFiles(FileHandler.MigFilesPath).Where(f => f.Contains("mig_") && f.EndsWith(".csv")).ToArray();

            if (auxFiles.IsNullOrEmpty()) return null;

            foreach (var f in auxFiles)
            {
                MigFileType type = MigFileFactory.GetMigFileType(f);

                if (!types.IsNullOrEmpty() && !types.Contains(type))
                    continue;

                if (!files.TryGetValue(type, out string _))
                    files.Add(type, f);
            }

            return files;
        }

        public enum LocalFilesResponse
        {
            None,
            NoPersonas,
            NoOtherFiles,
            Failed,
            Success
        }

        private async ValueTask<ConcurrentDictionary<string, int>> GetFileDocumentsAsync(MigFileType type, string file)
        {
            string[] fileLines = await FileHandler.GetFileLinesParallelAsync(file);

            ConcurrentDictionary<string, int> personsDocs = new ConcurrentDictionary<string, int>();

            Console.WriteLine("");

            int auxIndex = 0;

            await Task.Run(() =>
            {
                Parallel.ForEach(fileLines, (p) =>
                {
                    lock (fileLines)
                    {
                        Console.Write($"\r > [{type}] Cargando documentos {auxIndex + 1} de {fileLines.Length} lineas.");

                        int[] index = GetDocTypeDocIndex(type);
                        { }
                        string docT = p.Split('|')[index[0]];
                        string doc = p.Split('|')[index[1]];
                        { }
                        personsDocs.TryAdd($"{docT}-{doc}", auxIndex);

                        Interlocked.Increment(ref auxIndex);
                    }
                });
            });
   
            Console.WriteLine($"{type} Documentos listos.");
            Console.WriteLine("");

            return personsDocs;
        }

        private int[] GetDocTypeDocIndex(MigFileType type)
        {
            switch (type)
            {
                case MigFileType.mig_personas:
                    return new[] { 14, 15 };
                case MigFileType.mig_alumnos:
                    return new[] { 1, 2 };
                case MigFileType.mig_actas_examen_detalle:
                    return new[] { 2, 3 };
                default:
                    return null;
            }
        }

        public async ValueTask<LocalFilesResponse> LocalFilesPersonsValidation()
        {
            Console.WriteLine("Preparando archivos... ");
            await Task.Delay(250);

            MigFileType[] validPersonsFile = new MigFileType[] { MigFileType.mig_actas_examen_detalle, MigFileType.mig_alumnos };

            Dictionary<MigFileType, string> files = GetAllMigFiles();

            if (!files.Keys.Contains(MigFileType.mig_personas)) return LocalFilesResponse.NoPersonas;

            if (files.Count == 1) return LocalFilesResponse.NoOtherFiles;

            string personsFile = files.SingleOrDefault(f => f.Key == MigFileType.mig_personas).Value;

            files.Remove(MigFileType.mig_personas);

            ConcurrentDictionary<string, int> personsDocs = await GetFileDocumentsAsync(MigFileType.mig_personas, personsFile);
        
            ConcurrentDictionary<int, string> personsNotFound = new ConcurrentDictionary<int, string>();

            Console.Write($"\r ok!");
            Console.WriteLine("");

            foreach (var f in files)
            {
                if (!validPersonsFile.Contains(f.Key)) continue;

                Console.WriteLine($"Preparando {f.Key}...");
                await Task.Delay(250);

                ConcurrentDictionary<string, int> currentFileDocs = await GetFileDocumentsAsync(f.Key, f.Value);
                { }
                int auxIndex = 0;

                Console.WriteLine($"Comprobando documentos de {f.Key}...");
                await Task.Delay(250);

                Parallel.ForEach(currentFileDocs, (p) =>
                {
                    Console.Write($"\r > {auxIndex + 1} de {currentFileDocs.Count} registros.");

                    if (personsDocs.TryAdd(p.Key, 0))
                        personsNotFound.TryAdd(auxIndex, $"{p.Key}|{f.Key}");

                    Interlocked.Increment(ref auxIndex);
                });

                Console.WriteLine("");
            }

            Console.WriteLine("Analisis finalizado, creando resultados...");
  
            ///string outPutResponseFile = $"{FileHandler.OutputFilePath}personas_no_encontradas.csv";

           /// System.IO.File.WriteAllLines(outPutResponseFile, personsNotFound.Select(p => p.Value));

            Console.Write("\r Listo");

            return LocalFilesResponse.Success;
        }



        public async Task RunScannerService()
        {
            var file = CurrentMigFileNameNoExt.ToLower();

            IsWorking = true;

            if (file.Contains(nameof(MigFileType.mig_personas)))
            {
                await InitializeScanner<PersonasEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_docentes)))
            {


                await InitializeScanner<DocentesEnum>();




            }
            else if (file.Contains(nameof(MigFileType.mig_periodos_lectivos)))
            {
                await InitializeScanner<PeriodosLectivosEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_actividades)))
            {
                await InitializeScanner<ActividadesEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_alumnos)))
            {
                await InitializeScanner<AlumnosEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_acta_cursada_promocion)))
            {
                await InitializeScanner<ActasCursadaEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_actas_examen_detalle)))
            {
                await InitializeScanner<ActasExamenDetalleEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_actas_examen)))
            {
                await InitializeScanner<ActasExamenEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_equivalencia)))
            {
                await InitializeScanner<EquivalenciasEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_insc_cursada)))
            {
                await InitializeScanner<InscCursadasEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_llamados_mesa)))
            {
                await InitializeScanner<LlamadosMesaEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_mesas)))
            {
                await InitializeScanner<MesasEnum>();
            }
            else if (file.Contains(nameof(MigFileType.mig_comisiones)))
            {
                await InitializeScanner<ComisionesEnum>();
            }

            IsWorking = false;
        }

        //public async Task RunScannerService()
        //{
        //    var file = CurrentMigFileNameNoExt.ToLower();

        //    IsWorking = true;

        //    if (file.Contains(nameof(MigFileType.mig_personas)))
        //    {
        //        await InitializeScanner<PersonasEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_docentes)))
        //    {
        //        await InitializeScanner<DocentesEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_periodos_lectivos)))
        //    {
        //        await InitializeScanner<PeriodosLectivosEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_actividades)))
        //    {
        //        await InitializeScanner<ActividadesEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_alumnos)))
        //    {
        //        await InitializeScanner<AlumnosEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_acta_cursada_promocion)))
        //    {
        //        await InitializeScanner<ActasCursadaEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_actas_examen_detalle)))
        //    {
        //        await InitializeScanner<ActasExamenDetalleEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_actas_examen)))
        //    {
        //        await InitializeScanner<ActasExamenEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_equivalencia)))
        //    {
        //        await InitializeScanner<EquivalenciasEnum>();
        //    }
        //    else if (file.Contains(nameof(MigFileType.mig_insc_cursada)))
        //    {
        //       // await InitializeScanner<InscCursadasEnums>();
        //    }

        //    IsWorking = false;
        //}
        public async Task SelectMigFile()
        {
            CurrentMigFilePath = MenusManager.RunMigFilesMenu(this);
            {}
            if (!string.IsNullOrWhiteSpace(CurrentMigFilePath))
            {
                Console.Title = "SIU-Guaraní Controles Pre Migración." +
                    $" Archivo Cargado: {CurrentMigFileName.ToUpper()}";

                await MenusManager.RunMigFileActionsMenu(this);
            }
            else
            {
                ResetCore();
            }        
        }
        private static async Task InitializeDataFromDB<T>(ScannerService<T> scanner) where T : Enum
        {
            await scanner.InitiliazeDataFromDB();
        }

        public async ValueTask<bool> InitializeScanner<T>() where T : Enum
        {
            {}
            if (LoadMigFileOnMemory(out MigFile<T> mfile))
            {
                Console.WriteLine(Environment.NewLine);
     
                {}
                ScannerService<T> scService = new ScannerService<T>(mfile,
                    _connections.Single(c => c.ConnectionType == ConnectionType.DGENSyA_TEST))
                {
                   // DbSetService = new DbSetsServices.DbSetService(_connections.Single(c => c.ConnectionType == ConnectionType.UCSFD_DGENSyA_TEST))
                };
                {}
               // MigEditorService<T> edService = new MigEditorService<T>(scService);

                //if (mfile.Type == MigFileType.mig_personas)
                //    await edService.AddEsColegioExtranjeroField();

            //    await edService.FormatFileDates();

                await InitializeDataFromDB(scService);
                
                if (!await SetAndRunScanner(scService)) return false;

                Console.WriteLine(Environment.NewLine);

                await Task.Delay(250);

                ConcurrentDictionary<string, string> pers = new ConcurrentDictionary<string, string>();
                ConcurrentDictionary<string, string> alus = new ConcurrentDictionary<string, string>();

                //foreach (var line in File.ReadLines(Directory.GetCurrentDirectory()+ "\\Mig Files\\mig_personas.csv"))
                //{
                //    if (string.IsNullOrWhiteSpace(line)) continue;

                //    string[] data = line.Split('|');

                //    string tdoc = data[14];
                //    string ndoc = data[15];

                //    string key = $"{tdoc}-{ndoc}";

                //    pers.TryAdd(key, line);
                //}

                //foreach (var line in File.ReadLines(Directory.GetCurrentDirectory() + "\\Mig Files\\mig_alumnos.csv"))
                //{
                //    if (string.IsNullOrWhiteSpace(line)) continue;

                //    string[] data = line.Split('|');

                //    string tdoc = data[1];
                //    string ndoc = data[2];

                //    string key = $"{tdoc}-{ndoc}";

                //    alus.TryAdd(key, line);
                //}

                //List<string> alusNoPers = new List<string>();

                //foreach (var alu in alus)
                //{
                //    if (!pers.ContainsKey(alu.Key))
                //        alusNoPers.Add($"{alu.Key}=>{alu.Value}");

                    
                //}

                //if (alusNoPers.Count > 0)
                //    File.WriteAllLines(Directory.GetCurrentDirectory() + "\\AlusNotPer.csv", alusNoPers);
                
                if (!await CreateScannerReports(scService, mfile.Type)) return false;

               // System.IO.File.WriteAllLines(System.IO.Directory.GetCurrentDirectory() + "\\" + mfile.FileNameNoExt + "_2.csv", mfile.Lines, Encoding.UTF8);

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("Volviendo al menu principal...");

                await Task.Delay(1250);

                ResetCore();
            }
            else
            {
                return false;
            }

            return true;
        }
        private async ValueTask<bool> CreateScannerReports<T>(ScannerService<T> scService, MigFileType mtype) where T : Enum
        {
            bool? succes = false;

            try
            {
                Console.WriteLine($"<< Preparando y creando los resultados del archivo {CurrentMigFileName}.");

                await Task.Delay(750);

                ExportService<T> eService = new ExportService<T>(scService);

                succes = await eService.CreateReports(mtype);

                string successMessage;

                if (succes == null)
                {
                    successMessage = ">> No se detectaron errores.";
                }
                else
                {
                    successMessage = (bool)succes ? ">> Reportes creados exitosamente" : ">> Ocurrió un error y los reportes no pudieron ser generados.";
                }

                //string successMessage = succes ? ">> Reportes creados exitosamente" :
                //    ">> Ocurrió un error y los reportes no pudieron ser generados.";

                Console.WriteLine(successMessage);

                succes = true;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, true);
            }

            return (bool)succes;
        }

        private bool LoadMigFileOnMemory<T>(out MigFile<T> mfile) where T : Enum
        {
            try
            {
                {}
                Console.WriteLine($"<< Cargando archivo {CurrentMigFileName} ...");

                MigFileType mtype = MigFileFactory.GetMigFileType(CurrentMigFilePath);

                mfile = MigFileFactory.CreateMigFile<T>(CurrentMigFilePath, mtype);

                if (mfile.Lines.IsNullOrEmpty())
                {
                    Console.WriteLine($">> {CurrentMigFileName} está vacío.");
                    return false;
                }
                else
                {
                    Console.WriteLine($">> {CurrentMigFileName} Listo.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, true);
                mfile = null;

                return false;
            }
        }

        private async ValueTask<bool> SetAndRunScanner<T>(ScannerService<T> scService) where T : Enum
        {
            bool succes = false;

            try
            {
                Console.WriteLine($"<< Preparando y comenzando analísis del archivo {CurrentMigFileName} ...");

                await Task.Delay(750);

                succes = await scService.CompleteScanAsync();

                string successMessage = succes ? $">> {CurrentMigFileName} se ha analizado correctamente." :
                                                 $">> ¡Cancelado! Ocurrió un error durante el analísis de {CurrentMigFileName}";

                Console.WriteLine(successMessage);
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, true);
            }

            return succes;
        }
        private bool InitializeMigFilesPaths()
        {
            try
            {
                string[] files = FileHandler.GetFiles(FileHandler.MigFilesPath).Where(f => f.Contains("mig_")).ToArray();

                if (files.IsNullOrEmpty()) return false;

                List<string> auxFiles = new List<string>();

                int index = 0;

                int auxIndex = 0;
                int filesCount = files.Length;
                int remaining = filesCount;

                foreach (var f in files)
                {
                    auxFiles.Add(f);

                    auxIndex++;

                    if (auxIndex == 9 || (auxIndex < 9 && auxIndex == remaining))
                    {
                        PaginatedFilePaths.Add(index, auxFiles.ToArray());

                        index++;

                        auxFiles = new List<string>();

                        remaining -= auxIndex;
                        auxIndex = 0;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, true);

                return false;
            }
        }
    }
}
