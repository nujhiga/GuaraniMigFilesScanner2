using System;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M04
{
    public static class PeriodosLectivosValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case PeriodosLectivosEnum.nombre:
                    break;
                case PeriodosLectivosEnum.descripcion:
                    break;
                case PeriodosLectivosEnum.anio_academico:
                    break;
                case PeriodosLectivosEnum.periodo_generico:
                    break;
                case PeriodosLectivosEnum.fecha_inicio:
                case PeriodosLectivosEnum.fecha_fin:
                case PeriodosLectivosEnum.fecha_inicio_dictado:
                case PeriodosLectivosEnum.fecha_fin_dictado:
                case PeriodosLectivosEnum.fecha_tope_movimientos:
                case PeriodosLectivosEnum.fecha_inactivacion:
                case PeriodosLectivosEnum.fecha_publicacion_comision:
                    scService.ValidateDateFormat(fd, l, f);
                    break;
            }
        }
    }
}
