using System;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Class.Modules.M04
{



    public static class TurnosValidations 
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
           where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case TurnosEnum.turno_examen_nombre:
                    scService.ValidatePeriodoNombreExistence(fd, l, f);
                    break;
                case TurnosEnum.anio_academico:
                 //   scService.ValidateAnioAcademicoExistence2(fd, l, f);
                    break;
                case TurnosEnum.turno_tipo:
                    break;
                case TurnosEnum.fecha_inicio:
                    break;
                case TurnosEnum.fecha_fin:
                    break;
            }
        }
    }
}
