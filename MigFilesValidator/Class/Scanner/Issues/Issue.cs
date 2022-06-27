using System;

namespace GuaraniMigFilesScanner.Class.Scanner.Issues
{
    public class Issue<T> where T : Enum
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string UserNumber { get; set; }
        public string DescForReport { get; set; }
        public string FieldIssue { get; set; }
        public string Line { get; set; }
        public int LineNumber { get; set; }
        public int FieldNumber { get; set; }
        public string AdditionalInfo { get; set; }        
        public string AssociatedDocument { get; set; }

        public T FieldType { get; set; }

        public IssueType IssueT { get; private set; }
        public IssueSeverity IssueSeverity { get; set; }

        public Issue(IssueType type) =>
            IssueT = type;

       /* public string GetDescription(string fieldCount = "")
        {
            string desc = "";

            switch (IssueT)
            {
                case IssueType.DateFormat:
                    desc = $"El registro ({LineNumber}) en el campo de fecha ({FieldNumber}) contiene un " +
                           $"formato literal '{FieldIssue}' inválido. (Se requiere dd/mm/aaaa)";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.InvalidFieldCount:
                    desc = $"El registro ({LineNumber}) no coincide " +
                            $"con el número esperado de campos. Contiene {FieldIssue} campos de " +
                            $"{fieldCount} requeridos.";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.DBFieldNameNotFound:
                    desc = $"La tabla relacionada ({FieldIssue}) campo ({FieldNumber}) del registro ({LineNumber}) " +
                           $" no se encuentra en los recursos de la aplicación para el proceso de conversión.";
                    IssueSeverity = IssueSeverity.Warning;
                    break;
                case IssueType.NullData:
                    desc = $"El registro {LineNumber} en el campo {FieldNumber} no " +
                           $"contiene información y este campo es obligatorio.";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.RequiredChars:
                    desc = $"El registro {LineNumber} en el campo {FieldNumber} excede la cantidad de " +
                           $"caracteres permitidos. Contiene {FieldIssue}, máximo permitido: {AdditionalInfo}";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.DuplicatedDocument:
                    desc = $"El registro {LineNumber} en el campo {FieldNumber} presenta un número de documento duplicado ({FieldIssue})";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.InvalidCuitCuil:
                    desc = $"El registro {LineNumber} en el campo {FieldNumber} es un cuit-cuil que contiene caracteres inválidos. ({FieldIssue})";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.InvalidRelatedTableData:
                    desc = $"El registro {LineNumber} en el campo relacionado {FieldNumber} contiene información inválida. ({FieldIssue})";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.InvalidDataTypeString:
                    desc = $"El registro {LineNumber} en el campo {FieldNumber} presenta un tipo de dato erroneo, debe ser una cadena de texto. ({FieldIssue})";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.InvalidDataTypeInteger:
                    desc = $"El registro {LineNumber} en el campo {FieldNumber} presenta un tipo de dato errorneo, debe ser un número entero. ({FieldIssue})";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                case IssueType.YoungPerson:
                    desc = $"El registro {LineNumber} en el campo {FieldNumber} tiene una edad menor a 15 años. ({FieldIssue})";
                    IssueSeverity = IssueSeverity.Error;
                    break;
                //case IssueType.DuplicatedDocument:
                //    desc = $"El registro {LineNumber} ({LastName}, {UserName}, {UserNumber}) es una persona duplicada. Documento del Registro: {FieldIssue}. " +
                //           $"Número de registro en la tabla mdp_personas_documento: {AdditionalInfo}";
                //    IssueSeverity = IssueSeverity.Error;
                //    break;
            }

            return desc;
        }*/
    }
}
