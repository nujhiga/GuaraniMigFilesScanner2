using System;
using System.Linq;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M09
{
    public static class EquivalenciasValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case EquivalenciasEnum.nro_documento:
                    scService.ValidateNroDocumento(fd, l, f);

                    FieldData<T> ftype = fields.FirstOrDefault(a => a.Type.Equals(EquivalenciasEnum.tipo_documento));
                    // scService.ValidateNroDocumentoExistence(ftype, fd, l, f);

                    break;
                case EquivalenciasEnum.fecha_ext:
                case EquivalenciasEnum.fecha_int:
                case EquivalenciasEnum.fecha_otorgada:
                case EquivalenciasEnum.fecha_tramite:
                case EquivalenciasEnum.fecha_vigencia:
                case EquivalenciasEnum.documento_fecha:
                    scService.ValidateDateFormat(fd, l, f);
                    break;
                case EquivalenciasEnum.alcance:
                    scService.ValidateIsCharInRange(fd, l, f, new char[] { 'T', 'R', 'P' });
                    break;
                case EquivalenciasEnum.elemento:
                    scService.ValidateElementCodeExistence(fd, l, f);
                    break;
            }
        }
    }
}
