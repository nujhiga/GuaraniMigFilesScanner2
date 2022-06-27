using System;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M08
{
    public class LibrosDeActasValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
           where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case LibrosDeActasEnum.nro_libro:
                    scService.ValidateDuplicateNroLibro(fd, l, f);
                    scService.ValidateNroLibroExistence(fd, l, f);
                    break;
                case LibrosDeActasEnum.anio_academico:
                  //  scService.ValidateAnioAcademicoExistence2(fd, l, f);
                    break;
                case LibrosDeActasEnum.alcance_acta_regular:
                case LibrosDeActasEnum.alcance_acta_promocion:
                case LibrosDeActasEnum.es_libro_activo:
                case LibrosDeActasEnum.alcance_acta_examen:
                    scService.ValidateIsCharInRange(fd, l, f, new char[] { 'S', 'N' });
                    break;
                case LibrosDeActasEnum.propuesta:
                    scService.ValidatePropuestaExistence(fd, l, f);
                    break;
                case LibrosDeActasEnum.elemento:
                  //  scService.ValidateElementExistence(fd, l, f);
                    break;
                case LibrosDeActasEnum.fecha_activacion:
                case LibrosDeActasEnum.fecha_fin_vigencia:
                    scService.ValidateDateFormat(fd, l, f);
                    break;

            }
        }

    }
}
