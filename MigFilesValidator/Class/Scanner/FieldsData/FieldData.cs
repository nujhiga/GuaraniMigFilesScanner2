using System;
using Microsoft.Office.Interop.Excel;

namespace GuaraniMigFilesScanner.Class.Scanner.FieldsData
{
    public class FieldData<T> where T : Enum
    {
        public bool AdmitsNulls { get; set; }
        public string Data { get; set; }
        public int FieldNum { get; set; }
        public int LineNum { get; set; }
        public int MaxCharValue { get; set; }
        public int[] NumericMaxChars { get; set; }
        public string Line { get; set; }
        public T FieldType { get; set; }
        public Type Type { get; set; }
        public string AssociatedDocument { get; set; }
        public bool Duplicated { get; set; }

        public FieldData(string data, string line, int l, int f, bool required = false, int maxCharValue = 0)
        {
            FieldNum = f;
            LineNum = l;
            Line = line;
            Data = data;
            AdmitsNulls = required;
            MaxCharValue = maxCharValue;
        }

        public bool EmptyNullableFieldData => AdmitsNulls && string.IsNullOrWhiteSpace(Data);

        public override string ToString()
        {
            return Data;
        }
    }
}
