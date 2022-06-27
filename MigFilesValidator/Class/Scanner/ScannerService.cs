using GuaraniMigFilesScanner.Class.Connections;
using GuaraniMigFilesScanner.Class.FilesManagement;
using GuaraniMigFilesScanner.Class.MigFiles;
using GuaraniMigFilesScanner.Class.RulesSources;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;
using GuaraniMigFilesScanner.Class.Scanner.Issues;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PasifaeG3Migrations.Class.Extras;
using PasifaeG3Migrations.Class.DbSetsServices;
using PasifaeG3Migrations.Modules.M01;
using PasifaeG3Migrations.Modules.M02;
using PasifaeG3Migrations.Modules.M03;
using PasifaeG3Migrations.Modules.M04;
using PasifaeG3Migrations.Modules.M05;
using PasifaeG3Migrations.Modules.M08;
using PasifaeG3Migrations.Modules.M09;
using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;

namespace GuaraniMigFilesScanner.Class.Scanner
{
    public class ScannerService<T> where T : Enum
    {
        public MigFile<T> MigFile { get; set; }
        public bool Working { get; set; }
        public Dictionary<int, List<Issue<T>>> Issues { get; private set; }
        public GuaraniContext GCtx { get; }

        private ConcurrentDictionary<int, List<Issue<T>>> _auxIssues;

        public ConcurrentDictionary<string, string> DICDocuments;
        public ConcurrentDictionary<string, string> ComisionesDuplicadas;

        public ConcurrentDictionary<string, int> DocumentsFromDB;

        public ConcurrentDictionary<string, int> Personas;
        public ConcurrentDictionary<string, string> NroLibrosDuplicados;
        public ConcurrentDictionary<string, int> Alumnos;

        public ConcurrentDictionary<string, int> PeriodosNamesFromDB;

        public ConcurrentDictionary<string, int> ActivitiesFromDB;

        public ConcurrentDictionary<string, int> ElementsFromDB;

        public ConcurrentDictionary<string, int> EscalasNotasFromDB;

        public ConcurrentDictionary<string, int> AniosAcademicosFromDB;

        public ConcurrentDictionary<string, string> DICUsers;

        public ConcurrentDictionary<string, string> DICCuitCuils;

        public ConcurrentDictionary<string, string> DICDocenteLegajo;


        public ConcurrentDictionary<string, string> ActasDuplicadas;

        public ConcurrentDictionary<string, string> AlumnosDuplicados;

        //  public DbSetService DbSetService { get; set; }

        public ScannerService(MigFile<T> migFile, Connection connection)
        {
            MigFile = migFile;

            Issues = new Dictionary<int, List<Issue<T>>>();

            _auxIssues = new ConcurrentDictionary<int, List<Issue<T>>>();

            DICDocuments = new ConcurrentDictionary<string, string>();

            DICUsers = new ConcurrentDictionary<string, string>();

            DICCuitCuils = new ConcurrentDictionary<string, string>();
            NroLibrosDuplicados = new();
            DICDocenteLegajo = new ConcurrentDictionary<string, string>();

            ActasDuplicadas = new ConcurrentDictionary<string, string>();

            AlumnosDuplicados = new ConcurrentDictionary<string, string>();

            ComisionesDuplicadas = new();

            GCtx = new GuaraniContext(connection);

            Working = false;
        }

        private void GetDBDataDictionary(GuaraniTable guaTable)
        {
            switch (guaTable)
            {
                case GuaraniTable.sga_anios_academicos:
                    break;
                case GuaraniTable.sga_elementos:
                    break;
                case GuaraniTable.sga_escalas_notas:
                    break;
                case GuaraniTable.sga_periodos:
                    break;
                case GuaraniTable.vw_personas:
                    break;
                case GuaraniTable.vw_planes:
                    break;
            }
        }
        /*
        public async Task InitializeElementsFromDB()
        {
            ElementsFromDB = await DbSetService.GetElementsCodes();
        }

        public async Task InitializePlanesPropuestasFromDB()
        {
            PlanesPropuestasFromDB = await DbSetService.GetVwPlanes();
        }


        public async Task InitPersAlus()
        {
            Personas = await DbSetService.GetPersonasDb();
            Alumnos = await DbSetService.GetAlumnosDb();
        }

        public async Task InitializeDocumentsFromDB()
        {
            DocumentsFromDB = await DbSetService.GetMdpPersonasDocumentos();
        }

        public async Task InitializePeriodosNamesFromDB()
        {
            PeriodosNamesFromDB = await DbSetService.GetSgaPeriodosNames();
        }

        public async Task InitializeElementosCodesFromDB()
        {
            ActivitiesFromDB = await DbSetService.GetSgaElementosCodes();
        }

        public async Task InitializeEscalasNotasFromDB()
        {
            EscalasNotasFromDB = await DbSetService.GetSgaEscalasNotas();
        }

        public async Task InitializeAniosAcademicosFromDB()
        {
            AniosAcademicosFromDB = await DbSetService.GetSgaAnioAcademicos();
            { }
        }
        */

        public async ValueTask<bool> CompleteScanAsync()
        {
            bool succes =
            await Task.Run(() =>
            {
                int proLineCount = 1;

                int migFileLinesCount = MigFile.Lines.Length;

                var obj = new object();

                var auxDictionary = BackgroupScanner.GetAuxLinesDictionary(MigFile.Lines);
                {}
                try
                {
                    Parallel.For(0, migFileLinesCount, (l) =>
                    {
                        if (string.IsNullOrWhiteSpace(MigFile.Lines[l])) return;

                        _auxIssues.TryAdd(l, new List<Issue<T>>());

                        Console.Write($"\r > Inspeccionando Registro {proLineCount} de {migFileLinesCount} Registros.");

                        string[] lData;

                        auxDictionary.TryGetValue(l, out string line);

                        lock (obj) lData = line.Split('|');
                        {}
                        if (lData.Length != MigFile.RequiredFields)
                        {
                            {}
                            InvalidFieldCount(lData, line, l, MigFile.RequiredFields);
                        }
                        else
                        {
                            FieldData<T>[] fields = BackgroupScanner.GetFields(MigFile, lData, l);

                            //fields = edService.FormatFileDates(fields);

                            //line = fields.ToCsvString();

                            for (int f = 0; f < fields.Length; f++)
                            {
                                GenericMigTypesScan(fields, l, f, MigFile.Type);

                                ScanByMigTypes(fields, l, f, MigFile.Type);
                            }
                        }

                        Interlocked.Increment(ref proLineCount);

                    });
                    
                    Console.Write($"\r > Inspeción Finalizada.");

                    Issues = _auxIssues.Where(k => k.Value.Count > 0).ToDictionary(k => k.Key, v => v.Value);

                    return true;
                }
                catch (Exception ex)
                {
                    ExHandler.Handle(ex, true);

                    return false;
                }

                System.IO.File.WriteAllLines(Directory.GetCurrentDirectory() + "\\" + MigFile.FileNameNoExt + "_2.csv", auxDictionary.Select(k => k.Value), Encoding.UTF8);


            });

            return succes;
        }

        //public void ScanByMigTypes(FieldData<T>[] fields, int l, int f, MigFileType mtype)
        //{
        //    switch (mtype)
        //    {
        //        case MigFileType.mig_personas:
        //            PersonasValidations.Validate(this, fields, l, f);
        //            break;

        //        case MigFileType.mig_docentes:
        //            DocentesValidations.Validate(this, fields, l, f);
        //            break;

        //        case MigFileType.mig_periodos_lectivos:
        //            PeriodosLectivosValidations.Validate(this, fields, l, f);
        //            break;

        //        case MigFileType.mig_actividades:
        //            ActividadesValidations.Validate(this, fields, l, f);
        //            break;

        //        case MigFileType.mig_alumnos:
        //            AlumnosValidations.Validate(this, fields, l, f);

        //            break;

        //        case MigFileType.mig_acta_cursada_promocion:
        //            //await InitiliazeDataFromDB();
        //            ActasCursadaValidations.Validate(this, fields, l, f);
        //            break;

        //        case MigFileType.mig_actas_examen:
        //            //await InitiliazeDataFromDB();
        //            ActasExamenValidations.Validate(this, fields, l, f);
        //            break;
        //        case MigFileType.mig_actas_examen_detalle:
        //            //await InitiliazeDataFromDB();
        //            ActasExamenDetalleValidations.Validate(this, fields, l, f);
        //            break;

        //        case MigFileType.mig_equivalencia:
        //            EquivalenciasValidations.Validate(this, fields, l, f);
        //            break;

        //            //case MigFileType.mig_insc_cursada:
        //            //    InscCursadasValidations.Validate(this, fields, l, f);
        //            //    break;
        //    }
        //}


        public void ScanByMigTypes(FieldData<T>[] fields, int l, int f, MigFileType mtype)
        {
            switch (mtype)
            {
                case MigFileType.mig_personas:
                    PersonasValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_docentes:
                    DocentesValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_periodos_lectivos:
                    PeriodosLectivosValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_actividades:
                    ActividadesValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_alumnos:
                    AlumnosValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_acta_cursada_promocion:
                    //await InitiliazeDataFromDB();
                    ActasCursadaValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_mesas:
                    MesasValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_llamados_mesa:
                    LlamadosMesaValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_actas_examen:
                    //await InitiliazeDataFromDB();
                    ActasExamenValidations.Validate(this, fields, l, f);
                    break;
                case MigFileType.mig_actas_examen_detalle:
                    //await InitiliazeDataFromDB();
                    ActasExamenDetalleValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_equivalencia:
                    EquivalenciasValidations.Validate(this, fields, l, f);
                    break;

                case MigFileType.mig_insc_cursada:
              //      InscCursadasValidations.Validate(this, fields, l, f);
                    break;
            }
        }

        public async Task InitiliazeDataFromDB()
        {

            await GCtx.InitPersonas();
         
            await GCtx.InitAlumnos();
           
           // await GCtx.InitElementos();
           
           // await GCtx.InitPropuestas();
            
            {}

            //await GCtx.InitActasExamenes();

           // await GCtx.InitLibrosActas();

            await GCtx.InitPeriodos();
            {}
          //  await GCtx.InitMesasExamen();
            {}
         //   await GCtx.InitEscalasNotas();
            {}
            //await InitPersAlus();



            //await InitializeDocumentsFromDB();

            //await InitializeElementsFromDB();

            //// await InitializePlanesPropuestasFromDB();

            //await InitializePeriodosNamesFromDB();

            //await InitializeElementosCodesFromDB();

            //await InitializeEscalasNotasFromDB();

            //await InitializeAniosAcademicosFromDB();

            { }
        }

        private void GenericMigTypesScan(FieldData<T>[] fields, int l, int f, MigFileType mtype)
        {
            //if (BackgroupScanner.IsAnyFieldDocument(mtype))
            //    BackgroupScanner.SetDocumentIfExists(fields, fields[f]);

            FieldData<T> fd = fields[f];

            if (!fd.AdmitsNulls)
                ScanNullField(fd, l, f);

            //if (fd.Type != null && !fd.Type.Equals(typeof(decimal)))
            //{
            //    ScanInvalidRequiredCharCount(fd, l, f);
            //}
            //else
            //{
            //    ScanNumericInvalidRequiredCharCount(fd, l, f);
            //}
        }

        public void ValidateLocalidad(FieldData<T> fd, SIUGTables siugTable, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!BackgroupScanner.ValidateDataInCatalogTableRange(fd, siugTable))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'localidad' no está incluido en la tabla catálogo mug_localidades.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Localidad_Invalida, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidateColegio(FieldData<T> fd, SIUGTables siugTable, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!BackgroupScanner.ValidateDataInCatalogTableRange(fd, siugTable))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El código colegio {fd.Data} no está incluido en la tabla catalogo sga_colegios_secundarios.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Colegio_Invalido, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidateSexo(FieldData<T> fd, int l, int f)
        {
            if (fd.Data != "F" && fd.Data != "M")
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'Sexo' debe ser 'F' o 'M'.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Sexo_Invalido, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidateTipoDocumento(FieldData<T> fd, SIUGTables siugTable, int l, int f)
        {
            if (!BackgroupScanner.ValidateDataInCatalogTableRange(fd, siugTable))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'tipo_documento' no está incluido en la tabla catálogo mdp_tipos_documentos.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Tipo_Documento_Invalido, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidatePaisDocumento(FieldData<T> fd, SIUGTables siugTable, int l, int f)
        {
            if (!BackgroupScanner.ValidateDataInCatalogTableRange(fd, siugTable))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'pais_documento' no esta incluido en la tabla catálogo paises.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Pais_Documento_Invalido, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidatePaisOrigen(FieldData<T>[] fields, SIUGTables siugTable, int l, int f)
        {
            FieldData<T> fdNacionalidad = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.nacionalidad));
            FieldData<T> fdPaisOrigen = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.pais_origen));

            if (fdNacionalidad == null || fdPaisOrigen == null) return;

            IssueType iType = IssueType.None;

            bool error = false;

            string addinfo = string.Empty;

            switch (fdNacionalidad.Data)
            {
                case "1" when fdPaisOrigen.Data.Length > 0:
                    iType = IssueType.Pais_Origen_Invalido;
                    error = true;
                    addinfo = "El campo 'pais_origen' no debe tener datos si el campo 'nacionalidad' tiene valor 1.";
                    break;
                case "2" when fdPaisOrigen.Data.Length <= 0 || !BackgroupScanner.ValidateDataInCatalogTableRange(fdPaisOrigen, siugTable):
                    iType = IssueType.Pais_Origen_Invalido;
                    error = true;
                    addinfo = "El campo 'pais_origen' está vacío o no está incluido en la tabla catálogo paises.";
                    break;
                default:

                    break;
            }

            if (error)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    issues.Add(IssuesFactory.CreateIssue(fdPaisOrigen, iType, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidateNacionalidad(FieldData<T> fd, int l, int f)
        {
            if (string.IsNullOrWhiteSpace(fd.Data) || (fd.Data != "1" && fd.Data != "2"))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El campo 'nacionalidad' es inválido. Valores aceptados 1 o 2";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Nacionalidad_Invalida, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidateFechaNacimiento(FieldData<T> fd, int l, int f)
        {
            if (!BackgroupScanner.ValidateDateFormat(fd, out DateTime birthDate))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = $"El valor del campo 'fecha_nacimiento' tiene un formato inválido. Formato Esperado dd/mm/aaaa";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Formato_Fecha_Invalido, l, f, IssueSeverity.Error));
                }
            }
            else if (BackgroupScanner.IsPersonYoungerThan(birthDate))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valor del campo 'fecha_nacimiento' da como resultado una persona menor a 15 años.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Persona_Menor_de_15, l, f, IssueSeverity.Warning, addInfo));
                }
            }
        }
        public void ValidateDateFormat(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!BackgroupScanner.ValidateDateFormat(fd, out DateTime birthDate))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = $"El valor del campo '{fd.FieldType}' tiene un formato inválido. Formato Esperado dd/mm/aaaa";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Formato_Fecha_Invalido, l, f, IssueSeverity.Error));
                }
            }
        }
        public void ValidateInstitucionOtra(FieldData<T>[] fields, int l, int f)
        {
            FieldData<T> fdInstitucion = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.institucion));
            FieldData<T> fdInstitucionOtra = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.institucion_otra));

            if (fdInstitucion == null || fdInstitucionOtra == null) return;

            if (!string.IsNullOrWhiteSpace(fdInstitucionOtra.Data) && fdInstitucion.Data.Length != 0)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    issues.Add(IssuesFactory.CreateIssue(fdInstitucionOtra,
                        IssueType.Institucion_Otra_Invalida, l, f, IssueSeverity.Warning));
                }
            }
        }
        public void ValidateFechaIngresoPais(FieldData<T>[] fields, int l, int f)
        {
            FieldData<T> fdNacionalidad = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.nacionalidad));
            FieldData<T> fdFechaIngresoPais = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.fecha_ingreso_pais));

            if (fdNacionalidad == null || fdFechaIngresoPais == null) return;

            bool error = true;

            IssueType iType = IssueType.Fecha_Ingreso_Pais_Invalida;

            string addInfo = string.Empty;

            if (fdNacionalidad.Data == "2")
            {
                if (string.IsNullOrWhiteSpace(fdFechaIngresoPais.Data))
                {
                    iType = IssueType.Fecha_Ingreso_Pais_Invalida;
                    addInfo = "El campo 'fecha_ingreso_pais' no debe estar vacío cuando la nacionalidad es 2 (Extranjero).";
                }
                else if (!BackgroupScanner.ValidateDateFormat(fdFechaIngresoPais.Data))
                {
                    iType = IssueType.Formato_Fecha_Invalido;
                    addInfo = "El valor del campo 'fecha_ingreso_pais' tiene un formato inválido. Formato esperado dd/mm/aaaa";
                }
                else
                {
                    error = false;
                }
            }
            else if (fdNacionalidad.Data == "1")
            {
                if (!string.IsNullOrWhiteSpace(fdFechaIngresoPais.Data))
                {
                    iType = IssueType.Fecha_Ingreso_Pais_Invalida;
                    addInfo = "El valor del campo 'fecha_ingreso_pais' debe estar vacío si el campo 'nacionalidad' tiene valor 1 (Argentino).";
                }
                else
                {
                    error = false;
                }
            }

            if (error)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    issues.Add(IssuesFactory.CreateIssue(fdFechaIngresoPais,
                        iType, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }
        public void ValidateInstitucion(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (fd.Data != "1")
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El código del campo 'institucion' {fd.Data} debe ser '1'.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Institucion_Invalida, l, f));
                }
            }
        }
        public void ValidateColegioOtro(FieldData<T>[] fields, int l, int f)
        {
            FieldData<T> fdColegio = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.colegio));
            FieldData<T> fdColegioOtro = fields.SingleOrDefault(f => f.FieldType.Equals(PersonasEnum.colegio_otro));

            if (fdColegio == null || fdColegioOtro == null) return;

            if (!string.IsNullOrWhiteSpace(fdColegioOtro.Data) && fdColegio.Data.Length != 0)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "El campo 'colegio_otro' no debería contener información si el campo 'colegio' tiene datos.";
                    issues.Add(IssuesFactory.CreateIssue(fdColegioOtro, IssueType.Colegio_Otro_Invalido, l, f, IssueSeverity.Warning, addInfo));
                }
            }
        }
        public void GenericValidations(FieldData<T> fd, int l, int f)
        {

        }
        public void ValidateEmail(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!BackgroupScanner.ValidateEmail(fd.Data))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El campo 'email' tiene un valor inválido.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Email_Invalido, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }
        public void InvalidFieldCount(string[] lData, string line, int l, int requiredFields)
        {
            if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
            {
                string addInfo = $"Cantidad de campos incorrecta. Esperados: {requiredFields} Actuales: {lData.Length}. Cuando un registro no tiene el número de campos correctos no pueden efectuarse el resto de las validaciones.";
                issues.Add(IssuesFactory.CreateIssue<T>(lData.Length.ToString(), line, IssueType.Cantidad_Campos_Invalida, l, IssueSeverity.Error, addInfo));
            }
        }
        public void ScanNullField(FieldData<T> fd, int l, int f)
        {
            if (string.IsNullOrWhiteSpace(fd.Data))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "Este campo no permite valores nulos/vacíos y está vacío.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Campo_Vacio, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }
        public void ScanInvalidDataType(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!BackgroupScanner.ValidateInvalidDataType(fd))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Tipo_Dato_Invalido,
                        l, f, IssueSeverity.Error, fd.Type.ToString()));
                }
            }
        }
        public void ScanInvalidRequiredCharCount(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (fd.MaxCharValue > 0 && fd.Data.Length > fd.MaxCharValue)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"Cantidad de caracteres excedida. Valor Máximo: {fd.MaxCharValue} Actual: {fd.Data.Length}";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Cantidad_Caracteres_Invalida, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }
        public void ScanNumericInvalidRequiredCharCount(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!fd.Data.Contains(",") || fd.NumericMaxChars == null)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El formato del valor del campo con tipo de dato decimal {fd.FieldType} es inválido.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Campo_Decimal_Invalido, l, f, IssueSeverity.Error, addInfo));
                }
            }
            else
            {
                string integerD = fd.Data.Split(',').First();
                string decimalD = fd.Data.Split(',').Last();
                if (integerD.Length > fd.NumericMaxChars[0] || decimalD.Length > fd.NumericMaxChars[1])
                {
                    if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                    {
                        string addInfo = $"Cantidad de caracteres excedida para el campo decimal '{fd.FieldType}'. Valor Máximo Parte Entera: {fd.NumericMaxChars[0]} Actual: {integerD.Length}. Valor Máximo Parte Decimal: {fd.NumericMaxChars[1]} Actual: {decimalD.Length}.";
                        issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Cantidad_Caracteres_Decimal_Entero_Invalida, l, f, IssueSeverity.Error, addInfo));
                    }
                }
            }
        }
        public void ValidateDocenteLegajo(FieldData<T> fd, int l, int f)
        {
            fd.Duplicated = !DICDocenteLegajo.TryAdd(fd.Data, fd.Line);

            if (fd.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'docente_legajo' está duplicado en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Usuarios_Duplicados, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }
        public void ValidateUsuario(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            fd.Duplicated = !DICUsers.TryAdd(fd.Data, fd.Line);

            if (fd.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'usuario' está duplicado en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Usuarios_Duplicados, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }
        public void ValidateNroDocumentoExistence(FieldData<T> tdoc, FieldData<T> doc, int l, int f, bool evaluateExists, bool evaluateNotExists)
        {

            string key = $"{tdoc}-{doc}-";

            bool dbExistence = GCtx.Personas.ContainsKey(key); //GCtx.Personas.TryAdd(key, null);

            if (!dbExistence)
            {
                if (evaluateNotExists && _auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "La combinación tipo_doc + nro_doc no existe en la tabla mdp_personas_documentos";

                    issues.Add(IssuesFactory.CreateIssue(doc, IssueType.Nro_Documento_Inexistente, l, f, IssueSeverity.Error, addInfo));

                    //UnsafeCurrentFileLines.Add($"{doc.FieldType}:NOEXIST|{doc.Line}");

                    //  UnsafeCurrentFileLines.Add(doc.Line);   
                }
            }
            else
            {
                if (evaluateExists && _auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "La combinación tipo_doc + nro_doc ya existe en la tabla mdp_personas_documentos";

                    issues.Add(IssuesFactory.CreateIssue(doc, IssueType.Nro_Documento_Existente, l, f, IssueSeverity.Error, addInfo));

                    //SafeCurrentFileLines.Add($"{doc.FieldType}:EXIST|{doc.Line}");
                }
            }
        }
        public void ValidateDuplicatedComision(FieldData<T> nombre, FieldData<T> anio, FieldData<T> periodo, FieldData<T> actividad, int l, int f)
        {

            string key = $"{nombre.Data}-{anio.Data}-{periodo.Data}-{actividad.Data}-";

            nombre.Duplicated = ComisionesDuplicadas.ContainsKey(key);

            if (nombre.Duplicated)
            {
                { }
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "Comision en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(nombre, IssueType.Comision_Duplicada, l, f, IssueSeverity.Error, addinfo));
                }
            }
            else
            {
                ComisionesDuplicadas.TryAdd(key, nombre.Line);
                //    NoDupsAux.Add(nombre.Line);
            }
        }
        public void ValidatePersonaExists(FieldData<T> tdoc, FieldData<T> ndoc, int l, int f)
        {

            var reg = $"{tdoc.Data}-{ndoc.Data}";

            if (!Personas.ContainsKey(reg))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "El valor del campo 'nro_documento' no existe en la tabla mdp_personas_documentos.nro_documento";
                    issues.Add(IssuesFactory.CreateIssue(ndoc, IssueType.Persona_No_Existe, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }
        /*
        public void ValidateAnioAcademicoExistence2(FieldData<T> anio, int l, int f)
        {
            if (GCtx.AniosAcademicos.IsNullOrEmpty()) return;

            string key = $"{anio.Data}-";

            string addInfo = $"El valor del campo '{anio.Type}'";

            var aux = GCtx.AniosAcademicos.Select(a => a.Anio);

            if (!aux.Exists(new AnioAcademico())
            {
                addInfo = $"{addInfo} ya existe en sga_anios_academicos";

                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                    issues.Add(IssuesFactory.CreateIssue(anio,
                        IssueType.Anios_Academico_Inexistente, l, f,
                        IssueSeverity.Error, addInfo));
            }
        }
        */
        public void ValidateMesaExistence(FieldData<T> mesa, int l, int f)
        {
            bool existence = GCtx.MesasExamen.TryAdd(mesa.Data, null);

            if (!existence)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valor del campo {mesa.Type} ya existe en sga_mesa_examen.nombre";
                    issues.Add(IssuesFactory.CreateIssue(mesa, IssueType.Mesa_Examen_Existente, l, f, IssueSeverity.Error, addInfo));
                }
            }
            else
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valor del campo {mesa.Type} no existe en sga_mesa_examen.nombre";
                    issues.Add(IssuesFactory.CreateIssue(mesa, IssueType.Mesa_Examen_No_Existe, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidatePlanVerActual(FieldData<T> plana, FieldData<T> plani, Propuesta prop, int l, int f)
        {
            bool sameVer = plana.Data.Equals(plani.Data);

            bool isValid = prop.Plan != null && prop.Plan.VersionActual == int.Parse(plana.Data);

            if (!isValid && !sameVer)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string auxplan = $"{plana.Data}/{prop.Plan?.ID}/{prop.Plan?.VersionActual}";
                    string auxprop = $"{prop.ID}/{prop.Codigo}";

                    string addInfo = $"El plan {auxplan} no corresponde su versión actual tiene un valor inválido {plani.Type}. Carrera {auxprop}";

                    issues.Add(IssuesFactory.CreateIssue(plani, IssueType.Plan_Propuesta_Invalido, l, f, IssueSeverity.Error, addInfo));
                }
            }

        }
        public void ValidateNroLibroExistence(FieldData<T> nro, int l, int f)
        {
            if (GCtx.LibrosActas.IsNullOrEmpty()) return;

            string key = $"{nro.Data}-";

            string addInfo = $"El valor del campo '{nro.Type}'";

            IssueType itype;

            if (GCtx.LibrosActas.ContainsKey(key))
            {
                addInfo = $"{addInfo} ya existe en sga_libros_actas";
                itype = IssueType.Nro_Libro_Existe;
            }
            else
            {
                addInfo = $"{addInfo} no existe en sga_libros_actas";
                itype = IssueType.Nro_Libro_Inexistente;
            }

            if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                issues.Add(IssuesFactory.CreateIssue(nro, itype, l, f,
                    IssueSeverity.Error, addInfo));
        }
        public void ValidateActivityCodeExistence(FieldData<T> fd, int l, int f)
        {
            if (GCtx.Elementos.Count > 0 && !GCtx.Elementos.Any(e => e.Codigo != null && e.Codigo.Equals(fd.Data)))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valor del campo '{fd.Type}' no existe en la tabla sga_elementos.codigo";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Actividad_Codigo_Inexistente, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        //public void ValidateElementExistence(FieldData<T> fd, int l, int f)
        //{
        //    if (GCtx.Elementos2.IsNullOrEmpty()) return;

        //    if (GCtx.Elementos2.Find(e => e.ID == int.Parse(fd.Data)) == null)
        //    {
        //        if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
        //        {
        //            string addInfo = $"El valor del campo '{fd.Type}' no existe en la tabla sga_elementos.elemento";
        //            issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Elemento_Inexistente, l, f, IssueSeverity.Error, addInfo));
        //        }
        //    }
        //}

        public void ValidatePropuestaExistence(FieldData<T> prop, int l, int f)
        {
            if (GCtx.Propuestas.IsNullOrEmpty()) return;

            //tring key = $"{prop.Value}-";

            if (GCtx.Propuestas.Find(p => p.ID == int.Parse(prop.Data)) == null)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = $"La propuesta {prop.Data} no existe en la table sga_propuestas.";
                    issues.Add(IssuesFactory.CreateIssue(prop, IssueType.Propuesta_Inexistente, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidateDuplicateNroLibro(FieldData<T> nro, int l, int f)
        {
            string key = $"{nro.Data}-";

            if (!NroLibrosDuplicados.ContainsKey(key))
                NroLibrosDuplicados.TryAdd(key, nro.Line);
            else
            {
                nro.Duplicated = true;

                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "Nro libro duplicado en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(nro, IssueType.Nro_Libro_Duplicado, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }
        public void ValidateActasExistence(FieldData<T> ten, FieldData<T> aa, FieldData<T> men, FieldData<T> ac, FieldData<T> date, int l, int f, bool exists, bool notExists)
        {
            string key = $"{ten}-{aa}-{men}-{ac}-{date}-";

            bool dbExistence = GCtx.ActasExamenes.ContainsKey(key);

            //GCtx.ActasExamenes.TryAdd(key, null);

            if (!dbExistence)
            {
                if (notExists && _auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "La combinación turno_examen_nombre + anio_academico + mesa_examen_nombre + actividad_codigo + fecha no existe como acta de examen.";

                    issues.Add(IssuesFactory.CreateIssue(ten, IssueType.Acta_No_Existente, l, f, IssueSeverity.Error, addInfo));
                }
            }
            else
            {
                if (exists && _auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "La combinación turno_examen_nombre + anio_academico + mesa_examen_nombre + actividad_codigo + fecha ya existe como acta de examen.";

                    issues.Add(IssuesFactory.CreateIssue(ten, IssueType.Acta_Existente, l, f, IssueSeverity.Error, addInfo));
                }
            }

        }
        public void ValidateDuplicatedActaExamen(FieldData<T> turno, FieldData<T> anio, FieldData<T> mesa, FieldData<T> elem, FieldData<T> date, FieldData<T> acta, int l, int f)
        {

            string key = $"{turno.Data}-{anio.Data}-{mesa.Data}-{elem.Data}-{date.Data}-";

            bool duplicated = ActasDuplicadas.TryAdd(key, acta.Line);

            acta.Duplicated = duplicated;

            if (!acta.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "Acta examen duplicada en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(acta, IssueType.Acta_Examen_Duplicada, l, f, IssueSeverity.Error, addinfo));
                }
            }

        }

        public void ValidateDuplicatedActaExamenDetalle(FieldData<T> acta, FieldData<T> nlib, FieldData<T> tdoc, FieldData<T> ndoc, int l, int f)
        {
            //string key = $"{fdActa.Data}*{fdDocument.Data}*{fdPeriodo.Data}";
            //turno_examen_nombre + anio_academico + mesa_examen_nombre + actividad_codigo + fecha.

            string key = $"{acta.Data}-{nlib.Data}-{tdoc.Data}-{ndoc.Data}-";

            //

            acta.Duplicated = ActasDuplicadas.TryAdd(key, acta.Line);

            if (!acta.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "Acta examen detalle duplicada en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(acta, IssueType.Acta_Examen_Detalle_Duplicada, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }
        public Propuesta ValidatePropuesta_PlanEstado(FieldData<T> prop, int l, int f)
        {
            if (prop.Data is null)
            {
                {}
            }
            var gtxProp = GCtx.Propuestas.Find(p => p != null && p.ID == int.Parse(prop.Data));

            if (gtxProp == null)
            {
                { }
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"La propuesta {prop.Data} no existe en el sistema.";
                    issues.Add(IssuesFactory.CreateIssue(prop, IssueType.Propuesta_Inexistente, l, f, IssueSeverity.Error, addInfo));
                }
            }
            else if (gtxProp.Plan == null || (gtxProp.Plan.Estado != "V" && gtxProp.Plan.Estado != "A"))
            {
                { }
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El plan de la propuesta {gtxProp.ID}/{gtxProp.Codigo} no existe o bien su estado no es ACTIVO/VIGENTE. Valor actual: {gtxProp.Plan.Estado}";
                    issues.Add(IssuesFactory.CreateIssue(prop, IssueType.Plan_Propuesta_Invalido, l, f, IssueSeverity.Error, addInfo));
                }
            }

            return gtxProp;
        }

        public void ValidateAlumnoExistence(FieldData<T> tdoc, FieldData<T> doc, FieldData<T> prop, int l, int f, bool evaluateExists, bool evaluateNotExists)
        {
            string key = $"{tdoc}-{doc}-{prop}-";

            //bool dbExistence = GCtx.Alumnos.TryAdd(key, null);


            bool exists = GCtx.Alumnos.ContainsKey(key);

            { }

            if (!exists)
            {
                if (evaluateNotExists && _auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = $"La combinación tipo_doc + nro_doc + prop_id no existe en sga_alumnos. (key:{key})";

                    issues.Add(IssuesFactory.CreateIssue(doc, IssueType.Alumno_No_Existe, l, f, IssueSeverity.Error, addinfo));
                }
            }
            else
            {
                if (evaluateExists && _auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = $"La combinación tipo_doc + nro_doc + prop_id ya existe en sga_alumnos.";
                    issues.Add(IssuesFactory.CreateIssue(doc, IssueType.Alumno_Existe, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }
        public void ValidateDuplicatedAlumnos(FieldData<T> doc, FieldData<T> tdoc, FieldData<T> propc,  int l, int f)
        {
            string key = $"{tdoc}-{doc}-{propc}-";
         
            bool aux = AlumnosDuplicados.TryAdd(key, doc.Line);

            if (!aux)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = $"Los valores DNI + Propuesta están duplicados en otro registro.";
                    issues.Add(IssuesFactory.CreateIssue(doc, IssueType.Alumno_Duplicado, l, f, IssueSeverity.Error, addinfo));
                }
            }

        }
        public void ValidateAlumnoExists(FieldData<T> tdoc, FieldData<T> ndoc, FieldData<T> prop, int l, int f)
        {

            var reg = $"{tdoc.Data}-{ndoc.Data}-{prop.Data}";

            if (!Alumnos.ContainsKey(reg))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "El valor del campo 'nro_documento' no existe en la tabla mdp_personas_documentos.nro_documento";
                    issues.Add(IssuesFactory.CreateIssue(ndoc, IssueType.Alumno_No_Existe, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidateNroDocumentoExistence(FieldData<T> fd, int l, int f)
        {
            if (DocumentsFromDB.Count > 0 && !DocumentsFromDB.TryGetValue(fd.Data, out int _))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "El valor del campo 'nro_documento' no existe en la tabla mdp_personas_documentos.nro_documento";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Nro_Documento_Inexistente, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidateElementCodeExistence(FieldData<T> fd, int l, int f)
        {
            if (ElementsFromDB.Count > 0 && !ElementsFromDB.TryGetValue(fd.Data, out int _))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valor del campo '{fd.FieldType}' no existe en la tabla sga_elementos.elemento";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Elemento_Inexistente, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }
        //public void ValidateActivityCodeExistence(FieldData<T> fd, int l, int f)
        //{
        //    if (ActivitiesFromDB.Count > 0 && !ActivitiesFromDB.TryGetValue(fd.Data, out int _))
        //    {
        //        if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
        //        {
        //            string addInfo = $"El valor del campo '{fd.FieldType}' no existe en la tabla sga_elementos.codigo";
        //            issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Actividad_Codigo_Inexistente, l, f, IssueSeverity.Error, addInfo));
        //        }
        //    }
        //}

        public void ValidatePeriodoNombreExistence(FieldData<T> fd, int l, int f)
        {
           
            /*if (PeriodosNamesFromDB.Count > 0 && !PeriodosNamesFromDB.TryGetValue(fd.Data, out int _))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valor del campo '{fd.FieldType}' no existe en la tabla sga_periodos.nombre";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Periodo_Nombre_Inexistente, l, f, IssueSeverity.Error, addInfo));
                }
            }*/

        }

        public void ValidateEscalaNotaExistence(FieldData<T> fd, int l, int f)
        {
            if (EscalasNotasFromDB != null && EscalasNotasFromDB.Count > 0 && !EscalasNotasFromDB.TryGetValue(fd.Data, out int _))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valore del campo '{fd.FieldType}' no existe en la tabla sga_escalas_notas.escala_nota";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Escala_Nota_Inexistente, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidateAnioAcademicoExistence(FieldData<T> fd, int l, int f)
        {
            if (AniosAcademicosFromDB != null && AniosAcademicosFromDB.Count > 0 && !AniosAcademicosFromDB.TryGetValue(fd.Data, out int _))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valore del campo '{fd.FieldType}' no existe en la tabla sga_anios_academicos.anio_academico";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Anios_Academico_Inexistente, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }


        public void ValidateDuplicatedAlumnos(FieldData<T> fdDni, FieldData<T> fdProp, int l, int f)
        {
            string key = $"{fdDni.Data}*{fdProp.Data}";

            fdDni.Duplicated = !AlumnosDuplicados.TryAdd(key, fdDni.Line);

            if (fdDni.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = $"Los valores DNI + Propuesta están duplicados en otro registro.";
                    issues.Add(IssuesFactory.CreateIssue(fdDni, IssueType.Alumno_Duplicado, l, f, IssueSeverity.Error, addinfo));

                }
            }
        }

        public void ValidateDuplicatedActa(FieldData<T> fdActa, FieldData<T> fdDocument, FieldData<T> fdPeriodo, int l, int f)
        {
            string key = $"{fdActa.Data}*{fdDocument.Data}*{fdPeriodo.Data}";

            fdActa.Duplicated = !ActasDuplicadas.TryAdd(key, fdActa.Line);

            if (fdActa.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'nro_documento' está duplicado en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(fdActa, IssueType.Acta_Duplicada, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidateNroDocumento(FieldData<T> fd, int l, int f)
        {
            fd.Duplicated = !DICDocuments.TryAdd(fd.Data, fd.Line);

            if (fd.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'nro_documento' está duplicado en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Documentos_Duplicados, l, f, IssueSeverity.Error, addinfo));
                }
            }

            if (BackgroupScanner.StrInvalidChars(fd.Data))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'nro_documento' es inválido. Solo puede contener letras o digitos.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Nro_Documento_Invalido, l, f, IssueSeverity.Error, addinfo));
                }
            }
        }

        public void ValidateCuitCuil(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            fd.Duplicated = !DICCuitCuils.TryAdd(fd.Data, fd.Line);

            if (fd.Duplicated)
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'cuit_cuil' está duplicado en otro registro/linea.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.CuitCuils_Duplicados, l, f, IssueSeverity.Error, addinfo));
                }
            }

            if (!BackgroupScanner.CuitCuilCoreValidation(fd.Data))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addinfo = "El valor del campo 'cuit_cuil' es inválido o contiene caracteres incorrectos.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.CuitCuil_Invalido, l, f, IssueSeverity.Warning, addinfo));
                }
            }
        }

        public void ValidateActividadesEstado(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!fd.Data.ToLower().Equals("a") && !fd.Data.ToLower().Equals("b"))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "El valor del campo 'estado' es inválido. Solo admite 'A' (Activo) o 'B' (Baja)";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Act_Estado, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidateActividadesDisponiblePara(FieldData<T> fd, int l, int f)
        {
            if (fd.EmptyNullableFieldData) return;

            if (!fd.Data.ToLower().Equals("t") && !fd.Data.ToLower().Equals("c") && !fd.Data.ToLower().Equals("m"))
            {

                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = "El valor del campo 'disponible_para' es inválido. Solo admite 'T' (Mesas y Comisiones), 'C' (Solo para Comisiones) o 'M' (Solo para Mesas)";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Act_Disponible_Para, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidateIsCharInRange(FieldData<T> fd, int l, int f, char[] range)
        {
            if (fd.EmptyNullableFieldData) return;

            if (fd.Data.Length != 1) return;

            char chr = fd.Data.ToCharArray()[0];

            if (!chr.IsInRange(range))
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El valor del campo {fd.FieldType} es inválido. Solo admite {range.ToCsvString()}. Valor actual: {chr}";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Valor_Fuera_De_Rango, l, f, IssueSeverity.Error, addInfo));
                }
            }
        }

        public void ValidateIntegerDataType(FieldData<T> fd, int l, int f, int logicValueCharCount)
        {
            if (fd.EmptyNullableFieldData) return;

            if (fd.Type == typeof(int))
            {
                if (fd.Data.Length != logicValueCharCount)
                {
                    if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                    {
                        string addInfo = $"El valor del campo {fd.FieldType} es inválido. Para este tipo de dato se espera un valor compuesto de {logicValueCharCount} carácteres.";
                        issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Valor_Fuera_De_Rango, l, f, IssueSeverity.Error));
                    }
                }
            }
            else
            {
                if (_auxIssues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    string addInfo = $"El tipo de dato del campo {fd.FieldType} es inválido. Se requiere un tipo de dato entero.";
                    issues.Add(IssuesFactory.CreateIssue(fd, IssueType.Tipo_Dato_Invalido, l, f, IssueSeverity.Error));
                }
            }
        }


    }
}


//T - Mesas y Comisiones
//C - Solo para Comisiones
//M - Solo para mesas