using System;
using System.Linq;

using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M08
{




    public static class ActasExamenValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
           where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case ActasExamenEnum.nro_acta:

                    //turno_examen_nombre + anio_academico + mesa_examen_nombre + actividad_codigo + fecha.

                    FieldData<T> ten = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.turno_examen_nombre));
                    FieldData<T> aa = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.anio_academico));
                    FieldData<T> men = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.mesa_examen_nombre));
                    FieldData<T> ac = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.actividad_codigo));
                    FieldData<T> date = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.fecha));

                    scService.ValidateDuplicatedActaExamen(ten, aa, men, ac, date, fd, l, f);

                    break;

                case ActasExamenEnum.estado:
                    scService.ValidateIsCharInRange(fd, l, f, new[] { 'A', 'C' });
                    break;
                case ActasExamenEnum.fecha:
                    scService.ValidateDateFormat(fd, l, f);
                    break;
                case ActasExamenEnum.turno_examen_nombre:

                    ten = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.turno_examen_nombre));
                    aa = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.anio_academico));
                    men = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.mesa_examen_nombre));
                    ac = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.actividad_codigo));
                    date = fields.SingleOrDefault(a => a.FieldType.Equals(ActasExamenEnum.fecha));

                 //   scService.ValidateActasExistence(ten, aa, men, ac, date, l, f, true, false);

                    scService.ValidatePeriodoNombreExistence(fd, l, f);

                    break;
                case ActasExamenEnum.actividad_codigo:
                    scService.ValidateActivityCodeExistence(fd, l, f);
                    break;
                //case ActasExamenEnum.escala_nota:
                //    scService.ValidateEscalaNotaExistence(fd, l, f);
                //    break;
                case ActasCursadaEnum.anio_academico:
                    scService.ValidateAnioAcademicoExistence(fd, l, f);
                    break;
            }


        }
    }
}
