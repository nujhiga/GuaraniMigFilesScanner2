using System;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M04
{
    public class LlamadosValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
           where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case LlamadosEnum.turno_examen_nombre:
                    scService.ValidatePeriodoNombreExistence(fd, l, f);
                    break;
                case LlamadosEnum.anio_academico:
                    break;
                case LlamadosEnum.llamado_nombre:
                    scService.ValidatePeriodoNombreExistence(fd, l, f);
                    break;
                case LlamadosEnum.fecha_inicio:
                    break;
                case LlamadosEnum.fecha_fin:
                    break;
            }
        }

    }
}
