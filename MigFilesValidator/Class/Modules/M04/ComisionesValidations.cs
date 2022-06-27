using System;
using System.Linq;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M04
{
    public static class ComisionesValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
           where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case ComisionesEnum.nombre:

                    FieldData<T> anio = fields.FirstOrDefault(a => a.Type.Equals(ComisionesEnum.anio_academico));
                    FieldData<T> period = fields.FirstOrDefault(a => a.Type.Equals(ComisionesEnum.periodo_lectivo_nombre));
                    FieldData<T> actividad = fields.FirstOrDefault(a => a.Type.Equals(ComisionesEnum.actividad_codigo));
                    // FieldData<T> anio = fields.FirstOrDefault(a => a.FieldType.Equals(ComisionesEnum.anio_academico));


                    scService.ValidateDuplicatedComision(fd, anio, period, actividad, l, f);

                    break;

                case ComisionesEnum.periodo_lectivo_nombre:
                    scService.ValidatePeriodoNombreExistence(fd, l, f);
                    break;

                case ComisionesEnum.actividad_codigo:
                    scService.ValidateActivityCodeExistence(fd, l, f);

                    break;

                default:
                    break;
            }
        }
    }
}
