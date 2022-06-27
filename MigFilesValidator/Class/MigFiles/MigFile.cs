using GuaraniMigFilesScanner.Class.RulesSources;

using System;
using System.Collections.Generic;

namespace GuaraniMigFilesScanner.Class.MigFiles
{
    public class MigFile<T> where T : Enum
    {
        public readonly string Path;

        public readonly string FileName;

        public readonly string FileNameNoExt;

        public readonly MigFileType Type;

        public string[] Lines { get; set; }

        public readonly List<Rule<T>> Rules;

        public readonly int RequiredFields;

        public MigFile(string path, string fileName, MigFileType type, string[] lines, List<Rule<T>> rules)
        {
            Path = path;
            FileName = fileName;
            FileNameNoExt = fileName.Split('.')[1];
            Type = type;
            Lines = lines;
            Rules = rules;

            if (rules != null) RequiredFields = rules.Count;
        }


    }
}
