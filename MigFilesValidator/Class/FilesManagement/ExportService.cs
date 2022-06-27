using GuaraniMigFilesScanner.Class.MigFiles;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.Issues;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasifaeG3Migrations.Class.Extras;

namespace GuaraniMigFilesScanner.Class.FilesManagement
{
    public class ExportService<T> where T : Enum
    {
        public enum OutPutHeader
        {
            Nro_Linea_Registro,
            Nro_Campo,
            Dato_Inconsistente,
            Linea_Completa,
            Persona_Nro_Documento,
            Datos_Extra,
            Nombre_Campo
        }

        private ScannerService<T> _scanner;

        public ExportService(ScannerService<T> scanner)
        {
            _scanner = scanner;
        }

        public async Task<ConcurrentDictionary<IssueType, List<Issue<T>>>> GetIssuesTypesDictionary()
        {
            var issues = _scanner.Issues;

            ConcurrentDictionary<IssueType, List<Issue<T>>> DictionaryIssuesByType = new ConcurrentDictionary<IssueType, List<Issue<T>>>();

            await Task.Run(async () =>
            {

                foreach (var t in Enum.GetValues(typeof(IssueType)))
                {
                    List<Issue<T>> issuesList = await GetIssuesByType(issues, (IssueType)t);
                    if (issuesList == null) continue;

                    DictionaryIssuesByType.GetOrAdd((IssueType)t, issuesList);
                }

            });

            return DictionaryIssuesByType;
        }

        public async ValueTask<bool?> CreateReports(MigFileType mtype)
        {
            var issues = await GetIssuesTypesDictionary();

            List<string> fileLines = new List<string>();

            bool? succes;

            try
            {
                if (issues.IsNullOrEmpty()) return null;

                await CreateGeneralReport(mtype, issues);

                await Task.Run(() =>
                {
                    foreach (var i in issues)
                    {
                        if (i.Value.IsNullOrEmpty()) continue;

                        string fileName = GetFileName(i.Key);

                        if (string.IsNullOrWhiteSpace(fileName)) continue;

                        string[] headers = GetHeaderByIssueType(i.Key, mtype);

                        fileLines.Add(headers.ToCsvString());

                        Issue<T>[] issuesArray = i.Value.ToArray();
                        int len = issuesArray.Length;

                        if (issuesArray.Length > 5000)
                        {
                            Parallel.For(0, len, (i) => { lock (issuesArray) fileLines.Add(GetRowArray(issuesArray[i], mtype).ToCsvString()); });
                        }
                        else
                        {
                            foreach (var iss in i.Value) fileLines.Add(GetRowArray(iss, mtype).ToCsvString());
                        }

                        string dir = FileHandler.GetScannerOutPutPath(mtype);

                        if (fileLines.Count > 0) File.WriteAllLines($"{dir}{fileName}.csv", fileLines, Encoding.UTF8);

                        fileLines = new List<string>();
                    }
                });

                succes = true;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, true);
                succes = false;
            }

            return succes;
        }

        private async Task CreateGeneralReport(MigFileType mtype, ConcurrentDictionary<IssueType, List<Issue<T>>> issues)
        {
            if (issues.Count == 0) return;

            string fileName = $"Reporte General de {mtype}.txt";

            List<string> fileLines = new List<string>();

            fileLines.Add(GetGeneralReportHeaders().ToCsvString());

            await Task.Run(() =>
            {
                foreach (var iss in issues)
                {
                    if (iss.Key == IssueType.None) continue;

                    if (iss.Value.Count > 0)
                        fileLines.Add(GetGeneralReportRowArray(iss.Key, iss.Value.Count).ToCsvString());
                }

                if (fileLines.Count > 0)
                {
                    string newDir = FileHandler.GetScannerOutPutPath(mtype);

                    File.WriteAllLines($"{newDir}{fileName}", fileLines, Encoding.UTF8);
                }

            });
        }

        private string[] GetGeneralReportRowArray(IssueType it, int iCount)
        {
            return new string[] { it.FormatIssueTypeString(), iCount.ToString() };
        }

        private string[] GetGeneralReportHeaders()
        {
            return new string[] { "Tipo de Inconsistencia", "Cantidad de Registros Afectados" };
        }

        private async Task<List<Issue<T>>> GetIssuesByType(Dictionary<int, List<Issue<T>>> issues, IssueType issueType)
        {
            if (issues == null) return null;
            List<Issue<T>> rValue = new List<Issue<T>>();

            await Task.Run(() =>
            {
                var worthIssues = issues.Where(o => o.Value.Count > 0);

                foreach (var i in worthIssues)
                {
                    Parallel.For(0, i.Value.Count, (j) =>
                    {
                        if (i.Value[j].IssueT == issueType)
                        {
                            lock (rValue)
                            {
                                rValue.Add(i.Value[j]);
                            }
                        }
                    });
                }
            });

            return rValue;
        }

        private string GetFileName(IssueType issueType) => issueType.ToString().Replace("_", " ");

        private string[] GetHeaderByIssueType(IssueType issueType, MigFileType mtype)
        {
            string[] headerArray = new string[] { };

            switch (issueType)
            {
                default:

                    if (mtype == MigFileType.mig_personas || mtype == MigFileType.mig_docentes || mtype == MigFileType.mig_alumnos)
                    {
                        headerArray = GetPersonasDocentesHeaderArray();
                    }
                    else
                    {
                        headerArray = GetGenericHeaderArray();
                    }

                    break;

            }

            return headerArray;
        }

        private string[] GetPersonasDocentesHeaderArray()
        {
            return new string[] { nameof(OutPutHeader.Nombre_Campo), nameof(OutPutHeader.Nro_Linea_Registro),
                                  nameof(OutPutHeader.Nro_Campo), nameof(OutPutHeader.Dato_Inconsistente),
                                  nameof(OutPutHeader.Persona_Nro_Documento), nameof(OutPutHeader.Datos_Extra),
                                  nameof(OutPutHeader.Linea_Completa)
            };

        }
        private string[] GetGenericHeaderArray()
        {
            return new string[] { nameof(OutPutHeader.Nombre_Campo), nameof(OutPutHeader.Nro_Linea_Registro),
                                  nameof(OutPutHeader.Nro_Campo), nameof(OutPutHeader.Dato_Inconsistente),
                                  nameof(OutPutHeader.Datos_Extra),
                                  nameof(OutPutHeader.Linea_Completa)
            };

        }



        private string[] GetRowArray(Issue<T> issue, MigFileType mtype)
        {
            string l = issue.LineNumber.ToString();
            string f = issue.FieldNumber.ToString();
            string data = issue.FieldIssue.ToString();
            string type = issue.FieldType.ToString();
            string addInfo = issue.AdditionalInfo;
            string assoDoc = issue.AssociatedDocument;
            string line = issue.Line;

            string[] rowArray = new string[] { };

            switch (issue.IssueT)
            {
                default:

                    if (mtype == MigFileType.mig_personas || mtype == MigFileType.mig_docentes || mtype == MigFileType.mig_alumnos)
                    {
                        rowArray = new[] { type, l, f, data, assoDoc, addInfo, line }; //GetPersonasDocentesRowArray(type, l, f, data, assoDoc, addInfo, line);
                    }
                    else
                    {
                        rowArray = new[] { type, l, f, data, addInfo, line };
                    }

                    break;
            }

            return rowArray;
        }






        private string[] GetPersonasDocentesRowArray(string type, string data, string l, string f, string assoDoc, string addInfo, string line)
        {
            return new[] { type, data, l, f, assoDoc, addInfo, line };
        }

    }
}
