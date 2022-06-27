using GuaraniMigFilesScanner.Class.FilesManagement;
using GuaraniMigFilesScanner.Class.RulesSources;

using System;
using System.Collections.Generic;
using System.Linq;

namespace GuaraniMigFilesScanner.Class.MigFiles
{
    public static class MigFileFactory
    {
        public static MigFile<T> CreateMigFile<T>(string path, MigFileType mtype) where T : Enum
        {
            string fileName = path.Split('\\').Last();

            string[] lines = FileHandler.GetFileLines(path);
        
            List<Rule<T>> rules = RuleFactory.CreateRules<T>(mtype);

            MigFile<T> mFile = new MigFile<T>(path, fileName, mtype, lines, rules);

            return mFile;
        }

      
        public static MigFileType GetMigFileType(string fileName)
        {
            string auxFileName = fileName.Replace(".csv", string.Empty).Split('\\').Last();

            string[] enumNames = Enum.GetNames(typeof(MigFileType));

            int i = 0;

            MigFileType type = MigFileType.None;
         
            while (i < enumNames.Length && type == MigFileType.None)
            {
                if (auxFileName.Equals(enumNames[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    type = (MigFileType)i;
                }
                else
                {
                    i++;
                }
            }

            return type;
        }
    }
}
