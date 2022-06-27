using GuaraniMigFilesScanner.Class.FilesManagement;
using GuaraniMigFilesScanner.Class.MigFiles;

using System;

namespace GuaraniMigFilesScanner.Class.Scanner.FieldsData
{
    public static class FieldDataFactory
    {
        public static FieldData<T> CreateFieldData<T>(MigFile<T> migFile, string data, string line, int l, int f, int maxChars = 0) where T : Enum
        {
            FieldData<T> fd = new FieldData<T>(data, line, l, f)
            {
                AdmitsNulls = migFile.Rules[f].AdmitNulls,
                MaxCharValue = migFile.Rules[f].MaxCharacters,
                FieldType = (T)Enum.Parse(typeof(T), GetEnumName<T>(f)),
                Type = migFile.Rules[f].DataType
            };

            try
            {
                if (maxChars != -1 && maxChars > 0)
                    fd.MaxCharValue = maxChars;
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, true);
            }

            return fd;
        }

        public static string GetEnumName<T>(int f) where T : Enum
        {
            string[] names = Enum.GetNames(typeof(T));

            return names[f];
        }


    }
}
