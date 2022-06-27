using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

using System;

namespace GuaraniMigFilesScanner.Class.Scanner.Issues
{
    public static class IssuesFactory
    {
        public static Issue<T> CreateIssue<T>(FieldData<T> fd, IssueType itype,
            int l, int f , IssueSeverity severity = IssueSeverity.Error, string addInfo = null)
            where T : Enum
        {
            Issue<T> iss = new Issue<T>(itype)
            {
                FieldIssue = fd.Data,
                Line = fd.Line,
                LineNumber = l + 1,
                FieldNumber = f + 1,
                AssociatedDocument = fd.AssociatedDocument,
                AdditionalInfo = addInfo,
                IssueSeverity = severity,
                FieldType = fd.FieldType
            };

            return iss;
        }
        public static Issue<T> CreateIssue<T>(string data, string line, 
            IssueType itype, int l, IssueSeverity severity = IssueSeverity.Error, string addInfo = null)
            where T : Enum
        {
            Issue<T> iss = new Issue<T>(itype)
            {
                FieldIssue = data,
                Line = line,
                LineNumber = l + 1,
                AdditionalInfo = addInfo,
                IssueSeverity = severity
            };

            return iss;
        }

    }
}





