using System;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M04
{
    public static class LlamadosMesaValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
           where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case LlamadosMesaEnum.mesa_examen_nombre:
                    scService.ValidateMesaExistence(fd, l, f);
                    break;

                case LlamadosMesaEnum.turno_examen_nombre:
                    scService.ValidatePeriodoNombreExistence(fd, l, f);
                    break;
                case LlamadosMesaEnum.llamado_nombre:
                    scService.ValidatePeriodoNombreExistence(fd, l, f);
                    break;

                case LlamadosMesaEnum.actividad_codigo:
                    scService.ValidateActivityCodeExistence(fd, l, f);
                    break;

                default:
                    break;
            }
        }
    }
}
