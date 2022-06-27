using System;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M04
{
    public static class MesasValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                //nombre+turno+actvidad

                case MesasEnum.nombre:
                    scService.ValidateMesaExistence(fd, l, f);
                    break;

                case MesasEnum.turno_examen_nombre:
                    scService.ValidatePeriodoNombreExistence(fd, l, f);
                    break;

                case MesasEnum.actividad_codigo:
                    scService.ValidateActivityCodeExistence(fd, l, f);
                    break;

                case MesasEnum.escala_nota:
                    scService.ValidateEscalaNotaExistence(fd, l, f);
                    break;

                default:
                    break;
            }
        }
    }
}
