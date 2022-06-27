using GuaraniMigFilesScanner.Class.MigFiles;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

using System;
using System.Threading.Tasks;
using PasifaeG3Migrations.Class.Extras;

namespace GuaraniMigFilesScanner.Class.MigEditorServices
{
    public class MigEditorService<T> where T : Enum
    {
        private ScannerService<T> _scanner;
        public MigEditorService(ScannerService<T> scanner)
        {
            _scanner = scanner;
        }


        public async Task FormatFileDates()
        {
            string[] lines = _scanner.MigFile.Lines;

            await Task.Run(() =>
            {
                Parallel.For(0, lines.Length, (l) =>
                {
                    if (string.IsNullOrWhiteSpace(lines[l])) return;

                    lock (lines)
                    {
                        string[] lData = lines[l].Split('|');

                        FieldData<T>[] fields = BackgroupScanner.GetFields(_scanner.MigFile, lData, l);

                        for (int i = 0; i < fields.Length; i++)
                        {
                            if (!fields[i].FieldType.ToString().ToLower().Contains("fecha")) continue;

                            if (!DateTime.TryParse(fields[i].Data, out DateTime newDate)) continue;

                            fields[i].Data = newDate.ToString("dd/MM/yyyy");
                        }
                    }
                });
            });

        }


        public async Task AddEsColegioExtranjeroField()
        {
            if (_scanner.MigFile.Type != MigFileType.mig_personas) return;

            string[] lines = _scanner.MigFile.Lines;

            await Task.Run(() =>
            {
                Parallel.For(0, lines.Length, (ind) =>
                {
                    if (string.IsNullOrWhiteSpace(lines[ind])) return;

                    lock (lines)
                    {
                        string[] lineData = lines[ind].Split('|');
                        { }
                        if (lineData.Length >= 69) return;

                        string[] auxNewData = new string[69];

                        for (int i = 0; i < 21; i++)
                            auxNewData[i] = lineData[i];

                        auxNewData[22] = "N";

                        for (int i = 23; i < auxNewData.Length; i++)
                            auxNewData[i] = lineData[i - 1];

                        lines[ind] = auxNewData.ToCsvString('|', false);
                    }
                });
            });
        }


        //public async Task MigCursadasPromocionHotFix()
        //{
        //    string[] lines = _scanner.MigFile.Lines;
        //    await Task.Run(() =>
        //    {
        //        Parallel.For(0, lines.Length, (i) =>
        //        {
        //            lock (lines)
        //            {
        //                string[] lineData = lines[i].Split('|');

        //                string dateStr = lineData[2];
        //                DateTime date;

        //                try
        //                {
        //                    string[] dateData = dateStr.Split('-');
        //                    string day = dateData[0];
        //                    string mont = "X";
        //                    switch (dateData[1])
        //                    {
        //                        case "Mar": mont = "3"; break;
        //                        case "Abr": mont = "4"; break;
        //                        case "May": mont = "5"; break;
        //                        case "Jun": mont = "6"; break;
        //                        case "Jul": mont = "7"; break;
        //                        case "Ago": mont = "8"; break;
        //                        case "Sep": mont = "9"; break;
        //                        case "Oct": mont = "10"; break;
        //                        case "Nov": mont = "11"; break;
        //                        case "Dic": mont = "12"; break;
        //                    }

        //                    date = DateTime.Parse($"{day}/{mont}/20{dateData[2]}");
        //                }
        //                catch
        //                {
        //                    date = DateTime.Parse(dateStr);
        //                }

        //                lineData[2] = date.ToString("dd/MM/yyyy");

        //                lineData[4] = "Cursada";

        //                lineData[8] = lineData[8].Replace("cuatrimestre", "Cuatrimestre");

        //                if (lineData[8].StartsWith("Primer"))
        //                    lineData[8] = lineData[8].Replace("Primer", "1er");

        //                if (lineData[8].StartsWith("Segundo"))
        //                    lineData[8] = lineData[8].Replace("Segundo", "2do");

        //                lineData[8] = $"{lineData[8]} {lineData[7]}";

        //                lineData[22] = lineData[22] == "D" ? "R" : lineData[22];

        //                lines[i] = lineData.ToCsvString('|', false);
        //            }
        //        });
        //    });

        //    System.IO.File.WriteAllLines(FileHandler.MigFilesPath + "\\mig_actas_cursada_promocion_2.csv", lines, Encoding.UTF8);
        //}
        //public async Task MigPersonasRomerBrestHotFix()
        //{
        //    string file = $"{FileHandler.MigFilesPath}\\soloestas.txt";

        //    List<string> soloLines = new List<string>();

        //    List<string> linesl = new List<string>();

        //    foreach (var l in File.ReadLines(file))
        //        soloLines.Add(l);

        //    string[] lines = _scanner.MigFile.Lines;

        //    for (int i = 0; i < lines.Length; i++)

        //    {
        //        if (soloLines.Contains(lines[i].Split('|')[15]))
        //            linesl.Add(lines[i]);

        //    }

        //    File.WriteAllLines(Directory.GetCurrentDirectory() + "\\mig_personas.csv", linesl, Encoding.UTF8);

        //    { }

        //}

    }
}
