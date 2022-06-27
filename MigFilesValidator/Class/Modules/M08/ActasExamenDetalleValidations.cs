using System;
using System.Linq;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M08
{
    

    public static class ActasExamenDetalleValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case ActasExamenDetalleEnum.resultado:
                    scService.ValidateIsCharInRange(fd, l, f, new[] { 'A', 'R', 'U' });
                    break;
                case ActasExamenDetalleEnum.fecha:
                  //  scService.ValidateDateFormat(fd, l, f);
                    break;
                case ActasExamenDetalleEnum.tipo_inscripcion:
                    scService.ValidateIsCharInRange(fd, l, f, new[] { '3', '4' });
                    break;
                case ActasExamenDetalleEnum.tipo_documento:
                    // scService.ValidateTipoDocumento(fd, SIUGTables.mdp_tipos_documentos, l, f);
                    break;
                case ActasExamenDetalleEnum.nro_documento:

                    FieldData<T> tdoc = fields.FirstOrDefault(a => a.FieldType.Equals(ActasExamenDetalleEnum.tipo_documento));

                    FieldData<T> prop = fields.FirstOrDefault(a => a.FieldType.Equals(ActasExamenDetalleEnum.propuesta));

                    scService.ValidateNroDocumentoExistence(tdoc, fd, l, f, false, true);

                    scService.ValidateAlumnoExistence(tdoc, fd, prop, l, f, false, true);

                    break;

                case ActasExamenDetalleEnum.nro_acta:

 

                    //1   nro_acta varchar N   30
                    //2   nro_libro varchar N
                    //3   tipo_documento Integer N
                    //4   nro_documento varchar N
                    //5   propuesta varchar N
                    //6   plan_version integer S
                    //7   tipo_inscripcion Integer N
                    //8   fecha Date    S dd/ mm / aaaa

                    ///FieldData<T> auxfd = fields.Where(a => a.FieldType.Equals(ActasExamenDetalleEnum.nro_documento)).ElementAt(0);
                    //FieldData<T> auxfd2 = fields.Where(a => a.FieldType.Equals(ActasExamenDetalleEnum)).ElementAt(0);
                    //scService.ValidateDuplicatedActa(fd, auxfd, auxfd2, l, f);




                    break;
            }


        }
    }
}
