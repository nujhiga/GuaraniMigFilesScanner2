using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuaraniMigFilesScanner.Class.Scanner.Issues;

namespace PasifaeG3Migrations.Class.Extras
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> array) => array == null || !array.Any();

        public static bool IsEmpty(this ICollection<string> array)
        {
            foreach (var i in array)
            {
                if (!string.IsNullOrWhiteSpace(i))
                    return false;
            }

            return true;
        }
        public static string ToStatament(this string[] data, char comma = ';', string key = null)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length - 1; i++)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    sb.Append($"{data[i]}{comma} ");
                }
                else
                {
                    sb.Append($"{key}.{data[i]}{comma} ");
                }
            }

            if (string.IsNullOrWhiteSpace(key))
                sb.Append(data[^1]);
            else
                sb.Append($"{key}.{data[^1]}");

            return sb.ToString();
        }
        public static string ToCsvString(this string[] data, char comma = ';', bool whiteSpace = false)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length - 1; i++)
            {
                if (whiteSpace)
                    sb.Append($"{data[i]}{comma} ");
                else
                    sb.Append($"{data[i]}{comma}");
            }

            sb.Append(data[^1]);

            return sb.ToString();
        }

        public static string ToCsvString(this char[] data, char comma = ';')
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length - 1; i++)
                sb.Append($"{data[i]}{comma} ");

            sb.Append(data[^1]);

            return sb.ToString();
        }

        public static string GetFileName(this string filePath) => filePath.Split('\\').Last();
        public static string GetFileNameNoExt(this string filePath) => GetFileName(filePath).Split('.').First();

        public static string FormatIssueTypeString(this IssueType i)
        {
            string value = i.ToString().Replace("_", " ");

            return value;
        }

        public static T GetEnumValue<T>(string enumName) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));

            foreach (var v in values)
                if (v.ToString().ToLower() == enumName)
                    return (T)v;

            return default;
        }
        /*
        public static string ToCsvString<T>(this Field<T>[] fields) where T : Enum
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fields.Length - 1; i++)
                sb.Append($"{fields[i].SrcValue}|");

            sb.Append(fields[^1]);

            return sb.ToString();
        }
        */
        public static T Get<T>(this NpgsqlDataReader dr, string str)
        {
            int org = dr.GetOrdinal(str);

            T value = default;

            var obj = dr.GetValue(org);

            try
            {
                if (obj != DBNull.Value)
                    value = (T)obj;
            }
            finally { }

            return value;
        }

        public static T Get<T>(this NpgsqlDataReader dr, Enum enm)
        {
            var aux = enm.ToString();
            { }
            int org = dr.GetOrdinal(aux);
            { }
            T value = default;
            var obj = dr.GetValue(org);
            { }

            try
            {
                if (obj != DBNull.Value)
                    value = (T)obj;
            }
            finally { }

            return value;
        }

        public enum ErrCode
        {
            None,
            NoSuchField,
            NotNullable,
            NullNullable,
            InvalidFieldsLength,
            InvalidDataLength,
            InvalidDataType,
        }
        /*
        private static bool IsValidStruct<TField>(this Field<TField> field, int fieldsLength, int requiredFields, Rule<TField> rule, out ErrCode errCode) where TField : Enum
        {

            errCode = ErrCode.NoSuchField;
            if (field == null) return false;

            errCode = ErrCode.InvalidFieldsLength;
            if (fieldsLength != requiredFields) return false;

            errCode = ErrCode.NullNullable;
            if (rule.IsNullable && string.IsNullOrWhiteSpace(field.SrcValue)) return true;

            errCode = ErrCode.NotNullable;
            if (!field.NullException && !rule.IsNullable && string.IsNullOrWhiteSpace(field.SrcValue)) return false;

            errCode = ErrCode.InvalidDataLength;
            if (rule.MaxCharacters > 0 && field.SrcValue.Length > rule.MaxCharacters) return false;

            return true;
        }
        */
        //public static bool IsInstantiable(this PropertyInfo prop)
        //{
        //    var attrs = prop.GetCustomAttributes<FileModelAttribute>();
        //    attrs.ElementAt(0).Instantiable
        //}



        /*
        public static object ReadField<TField>(this IEnumerable<Field<TField>> fields, object fieldType, int requiredFields) where TField : Enum
        {
            object value = null;

            Field<TField> field = fields.FirstOrDefault(f => f.FieldType.Equals(fieldType));

            Rule<TField> rule = field.Rule;

            if (!field.IsValidStruct(fields.Count(), requiredFields, rule, out ErrCode errCode))
                return errCode;
            else
            {
                if (errCode == ErrCode.NullNullable)
                    return errCode;

                try
                {
                    value = Convert.ChangeType(field.SrcValue, rule.Type);
                }
                catch
                {
                    return ErrCode.InvalidDataType;
                }
            }

            return value;
        }

        public static int GetFindex<TField>(this IEnumerable<Field<TField>> fields, object fieldType) where TField : Enum
        {
            Field<TField> field = fields.FirstOrDefault(f => f.FieldType.Equals(fieldType));
            return field.Rule.Index;
        }

        */
        //public static TValue Read<TValue, TField>(this Field<TField>[] fields, Enum fieldType)
        //    where TField : Enum
        //{
        //    TValue value = default;

        //    Field<TField> field = fields.FirstOrDefault(f => f.FieldType.Equals(fieldType));

        //    if (field == null) return default;

        //    try
        //    {
        //        value = (TValue)Convert.ChangeType(field.SrcValue, field.Type);
        //    }
        //    catch
        //    {
        //    }

        //    return value;
        //}
    }
}


//      where T : Enum => $"select {table.GetTableFields<T>("t1")} from {schema}.{table} join {joinTable.GetTableFields<>}";

//using GuaraniMigFilesScanner.Class.Connections;
//using GuaraniMigFilesScanner.Class.Scanner.FieldsData;
//using GuaraniMigFilesScanner.Class.Scanner.Issues;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace GuaraniMigFilesScanner.Class.Extras
//{
//    public static class Extensions
//    {
//        public static bool IsNullOrEmpty<T>(this ICollection<T> array) => array == null || array.Count == 0;

//        public static bool IsEmpty(this ICollection<string> array)
//        {
//            foreach (var i in array)
//            {
//                if (!string.IsNullOrWhiteSpace(i))
//                    return false;
//            }

//            return true;
//        }

//        public static string ToCsvString(this string[] data, char comma = ';', bool whiteSpace = true)
//        {
//            StringBuilder sb = new StringBuilder();

//            for (int i = 0; i < data.Length - 1; i++)
//            {
//                if (whiteSpace)
//                    sb.Append($"{data[i]}{comma} ");
//                else
//                    sb.Append($"{data[i]}{comma}");
//            }

//            sb.Append(data[^1]);

//            return sb.ToString();
//        }

//        public static string ToCsvString(this char[] data, char comma = ';')
//        {
//            StringBuilder sb = new StringBuilder();

//            for (int i = 0; i < data.Length - 1; i++)
//                sb.Append($"{data[i]}{comma} ");

//            sb.Append(data[^1]);

//            return sb.ToString();
//        }

//        public static string GetFileName(this string filePath) => filePath.Split('\\').Last();
//        public static string GetFileNameNoExt(this string filePath) => GetFileName(filePath).Split('.').First();

//        public static string FormatIssueTypeString(this IssueType i)
//        {
//            string value = i.ToString().Replace("_", " ");

//            return value;
//        }

//        public static T GetEnumValue<T>(string enumName) where T : Enum
//        {
//            var values = Enum.GetValues(typeof(T));

//            foreach (var v in values)
//                if (v.ToString().ToLower() == enumName)
//                    return (T)v;

//            return default;
//        }

//        public static string ToCsvString<T>(this FieldData<T>[] fields) where T : Enum
//        {
//            StringBuilder sb = new StringBuilder();

//            for (int i = 0; i < fields.Length - 1; i++)
//                sb.Append($"{fields[i].Data}|");

//            sb.Append(fields[^1]);

//            return sb.ToString();
//        }
//        public static string GetQuery<T>(GuaraniTable gTable) where T : Enum => $"select {Enum.GetNames(typeof(T)).ToCsvString(',')} from {gTable}";

//    }
//}
