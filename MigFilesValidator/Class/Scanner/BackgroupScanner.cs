using GuaraniMigFilesScanner.Class.FilesManagement;
using GuaraniMigFilesScanner.Class.MigFiles;
using GuaraniMigFilesScanner.Class.RulesSources;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;
using GuaraniMigFilesScanner.Class.Scanner.Issues;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PasifaeG3Migrations.Modules.M01;

namespace GuaraniMigFilesScanner.Class.Scanner
{
    public static class BackgroupScanner
    {
        public static ConcurrentDictionary<int, string> GetAuxLinesDictionary(string[] lines)
        {
            ConcurrentDictionary<int, string> auxDictionary = new ConcurrentDictionary<int, string>();

            for (int i = 0; i < lines.Length; i++)
                auxDictionary.TryAdd(i, lines[i]);

            return auxDictionary;
        }
        public static FieldData<T>[] GetFields<T>(MigFile<T> migFile, string[] lData, int l) where T : Enum
        {
            List<FieldData<T>> fields = new List<FieldData<T>>();

            string line = migFile.Lines[l];
            { }
            for (int f = 0; f < lData.Length; f++)
            {
                fields.Add(FieldDataFactory.CreateFieldData(migFile, lData[f], line, l, f));
            }

            return fields.ToArray();
        }
        public static bool ValidateInvalidDataType<T>(FieldData<T> field) where T : Enum
        {
            bool InvalidDataType = false;

            switch (field.Data.GetType().ToString().ToLower())
            {
                case "system.string":
                    InvalidDataType = !(field.Data is string);
                    break;
                case "system.int32":
                    InvalidDataType = !int.TryParse(field.Data, out int _);
                    break;
                case "system.datetime":
                    InvalidDataType = !DateTime.TryParse(field.Data, out DateTime _);
                    break;
                case "system.decimal":
                    InvalidDataType = !decimal.TryParse(field.Data, out decimal _);
                    break;
                case "system.char":
                    InvalidDataType = !char.TryParse(field.Data, out char _);
                    break;
                case "system.int16":
                    InvalidDataType = !short.TryParse(field.Data, out short _);
                    break;
            }

            return InvalidDataType;
        }

        public static bool ValidateDateFormat<T>(FieldData<T> fd, out DateTime date) where T : Enum
        {
            if (DateTime.TryParse(fd.Data, out DateTime newDate))
                fd.Data = newDate.ToString("dd/mm/yyyy");

            return DateTime.TryParseExact(fd.Data, "dd/mm/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        public static bool ValidateDateFormat(string date)
        {
            return DateTime.TryParseExact(date, "dd/mm/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        public static bool CuitCuilCoreValidation(string cuit)
        {
            bool rv = false;

            int verificador;
            int resultado = 0;
            string codes = "6789456789";
            long cuit_long = 0;

            if (long.TryParse(cuit, out cuit_long))
            {
                verificador = int.Parse(cuit[^1].ToString());
                int x = 0;
                try
                {
                    while (x < 10)
                    {
                        int digitoValidador = int.Parse(codes.Substring((x), 1));
                        int digito = int.Parse(cuit.Substring((x), 1));
                        int digitoValidacion = digitoValidador * digito;
                        resultado += digitoValidacion;
                        x++;
                    }
                    resultado = resultado % 11;
                    rv = (resultado == verificador);
                }
                catch
                {

                }
            }

            return rv;
        }
        public static bool ValidateEmail(string email)
        {
            bool valid = false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                valid = addr.Address == email;
            }
            catch
            {
            }
            return valid;
        }
        /*
        public static void SetDocumentIfExists<T>(FieldData<T>[] fields, FieldData<T> currfd) where T : Enum
        {
            var docEnumValue = Extensions.GetEnumValue<T>("nro_documento");

            if (docEnumValue == null) return; 

            FieldData<T> fdDocument = fields.SingleOrDefault(f => f.FieldType.Equals(docEnumValue));

            if (fdDocument == null) return;

            if (fdDocument != null && !string.IsNullOrWhiteSpace(fdDocument.Data))
                currfd.AssociatedDocument = fdDocument.Data;
        }
        */
        public static bool ValidateDataInCatalogTableRange<T>(FieldData<T> fd, SIUGTables siugTable) where T : Enum
        {
            string file = $"{FileHandler.RulesSourcePath}{siugTable}.csv";

            if (!File.Exists(file)) return false;

            bool valid = false;

            Parallel.ForEach(File.ReadLines(file), (l) =>
            {

                if (string.IsNullOrWhiteSpace(l)) return;

                if (valid) return;

                valid = l.Equals(fd.Data);

            });

            return valid;
        }


        public static bool IsAndSetDuplicated<T>(FieldData<T> fd, 
            ConcurrentDictionary<string, string> dupDictionary, 
            ConcurrentDictionary<int, List<Issue<T>>> auxissues, int l, int f) where T : Enum
        {
            bool duplicated = dupDictionary.TryGetValue(fd.Data, out string doc);

            if (!dupDictionary.TryGetValue(fd.Data, out doc))
                dupDictionary.TryAdd(fd.Data, fd.Data);

            if (duplicated)
            {
                if (auxissues.TryGetValue(l, out List<Issue<T>> issues))
                {
                    var issueData = GetDuplicatedIssueData(fd);

                    string addInfo = (string)issueData[0];
                    IssueType itype = (IssueType)issueData[1];

                    issues.Add(IssuesFactory.CreateIssue(fd, itype, l, f, IssueSeverity.Error, addInfo));
                }
            }

            return duplicated;
        }

        private static object[] GetDuplicatedIssueData<T>(FieldData<T> fd) where T : Enum
        {
            string addInfo = null;
            IssueType itype = default;

            switch (fd.FieldType)
            {
                case PersonasEnum.nro_documento:
                    addInfo = "Persona/Docente duplicada por número de DOCUMENTO.";
                    itype = IssueType.Documentos_Duplicados;
                    break;
                case PersonasEnum.usuario:
                    addInfo = "Persona/Docente duplicada por número de USUARIO.";
                    itype = IssueType.Usuarios_Duplicados;
                    break;
                case PersonasEnum.cuit_cuil:
                    addInfo = "Persona/Docente duplicada por número de CUIT_CUIL.";
                    itype = IssueType.CuitCuils_Duplicados;
                    break;
            }

            return new object[] { addInfo, itype };
        }

        public static bool IsPersonYoungerThan(DateTime birthDate, int youngThanYear = 15)
        {
            DateTime today = DateTime.UtcNow;

            int age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age)) age--;

            return age < 15;
        }

        public static bool IsAnyFieldDocument(MigFileType mtype)
        {
            bool hit = false;

            switch (mtype)
            {
                case MigFileType.mig_personas:
                case MigFileType.mig_docentes:
                case MigFileType.mig_alumnos:
                case MigFileType.mig_requisitos:
                case MigFileType.mig_insc_cursada:
                case MigFileType.mig_eval_detalle:
                case MigFileType.mig_insc_examen:
                case MigFileType.mig_acta_cursada_promocion:
                case MigFileType.mig_actas_examen_detalle:
                case MigFileType.mig_equivalencia:
                case MigFileType.mig_actas_examen_detalle_fc:
                case MigFileType.mig_aprob_x_resolucion:
                    hit = true;
                    break;
                default:
                    break;
            }

            return hit;
        }

        public static bool StrInvalidChars(string strData)
        {
            char[] chars = strData.ToCharArray();

            foreach (var c in chars)
                if (!char.IsLetterOrDigit(c))
                    return true;

            return false;
        }

        public static bool IsInRange(this char chr, char[] range) => range.Any(c => c.Equals(chr));

       

     

    }
}
