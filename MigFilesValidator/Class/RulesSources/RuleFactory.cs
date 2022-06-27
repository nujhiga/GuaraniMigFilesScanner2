using GuaraniMigFilesScanner.Class.FilesManagement;
using GuaraniMigFilesScanner.Class.MigFiles;

using System;
using System.Collections.Generic;
using System.IO;
using PasifaeG3Migrations.Class.Extras;

namespace GuaraniMigFilesScanner.Class.RulesSources
{
    public static class RuleFactory
    {
        public static List<Rule<T>> CreateRules<T>(MigFileType type) where T : Enum
        {
            List<Rule<T>> rules = new List<Rule<T>>();

            string rulesSource = FileHandler.GetRuleSourcesFile(type);

            if (string.IsNullOrWhiteSpace(rulesSource)) return null;

            foreach (var l in File.ReadLines(rulesSource))
            {
                if (string.IsNullOrWhiteSpace(l)) continue;

                string[] lData;

                lData = l.Split('\t');

                if (lData.IsNullOrEmpty()) continue;

                rules.Add(CreateRule<T>(lData));
            }

            return rules;
        }

        private static Rule<T> CreateRule<T>(string[] data) where T : Enum
        {
            Rule<T> r = new Rule<T>();

            Type dataType = Rule<T>.GetDataType(data[Rule<T>.TYPE_INDEX]);

            int rIndex = 0;
            
            if (dataType != null && dataType.Equals(typeof(decimal)))
            {
                r.NumericMaxChars = Rule<T>.GetNumericMaxChars(data[Rule<T>.TYPE_INDEX]);
            }
            else
            {
                r.MaxCharacters = Rule<T>.GetMaxCharacters(data[Rule<T>.TYPE_INDEX]);
            }

            string name = data[Rule<T>.NAME_INDEX];

            bool admitNulls = data[Rule<T>.ADMIT_NULLS] == "S" || data[Rule<T>.ADMIT_NULLS] == "SI";

            if (int.TryParse(data[Rule<T>.NUMBER_INDEX], out int rNumber))
                if (rNumber >= 1) rIndex = rNumber - 1;

            SetComments(data, r);

            r.Number = rNumber;
            r.Index = rIndex;
            r.Name = name;
            r.DataType = dataType; 
            r.AdmitNulls = admitNulls;

            r.FieldType = GetRuleType<T>(data[Rule<T>.NAME_INDEX], default);

            return r;
        }

        private static T GetRuleType<T>(string name, T defaultValue) where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T));

            name = name.ToLower();

            foreach (T item in enumValues)
            {
                if (name.Contains(item.ToString().ToLower()))
                    return item;
            }

            return defaultValue;
        }

        private static void SetComments<T>(string[] data, Rule<T> r) where T : Enum
        {
            string comments = string.Empty;

            if (data.Length > 3)
            {
                string commentString = "";
                for (int i = 4; i < data.Length; i++)
                    commentString += data[i];

                comments = Rule<T>.GetComments(commentString);
            }

            r.Comments = comments;
        }

    }
}

