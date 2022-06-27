using System;
using System.Linq;

namespace GuaraniMigFilesScanner.Class.RulesSources
{
    public class Rule<T> where T : Enum
    {
        public const int NUMBER_INDEX = 0;

        public const int NAME_INDEX = 1;

        public const int TYPE_INDEX = 2;

        public const int ADMIT_NULLS = 3;

        public int Number { get; set; }
        public int Index { get; set; }
        public int MaxCharacters { get; set; }
        public int[] NumericMaxChars { get; set; }
        public bool AdmitNulls { get; set; }
        public Type DataType { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public T FieldType { get; set; }

        //public PersonasEnum FieldType { get; set; }

        public static Type GetDataType(string dataTypeString)
        {
            Type rValue = null;
            if (dataTypeString.Contains("integer"))
                rValue = typeof(int);
            else if (dataTypeString.Contains("numeric"))
                rValue = typeof(decimal);
            else if (dataTypeString.Contains("varchar"))
                rValue = typeof(string);
            else if (dataTypeString.Contains("char"))
                rValue = typeof(char);
            else if (dataTypeString.Contains("date"))
                rValue = typeof(DateTime);
            else if (dataTypeString.Contains("string"))
                rValue = typeof(string);
            else if (dataTypeString.Contains("text"))
                rValue = typeof(string);
            else if (dataTypeString.Contains("smallint"))
                rValue = typeof(short);
            return rValue;
        }

        public static int[] GetNumericMaxChars(string dataTypeString)
        {
            if (!dataTypeString.Contains("(") && !dataTypeString.Contains(")") && !dataTypeString.Contains(",")) return null;

            int integer = int.Parse(dataTypeString.Split(',').First().
                Split('(').Last().Split(')').First());

            int decim = int.Parse(dataTypeString.Split(',').Last().
                Split('(').Last().Split(')').First());

            return new int[] { integer, decim };
        }

        public static int GetMaxCharacters(string dataTypeString)
        {
            if (!dataTypeString.Contains("(") && !dataTypeString.Contains(")")) return 0;

            return int.Parse(dataTypeString.Split('(').Last().Split(')').First());
        }

        public static string GetComments(string commentsString)
        {
            string comments = "";
            if (string.IsNullOrWhiteSpace(commentsString)) return comments;

            var commentData = commentsString.Split(new char[] { '\t' },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var c in commentData)
                comments += " " + c;

            return comments;
        }
    }

}
