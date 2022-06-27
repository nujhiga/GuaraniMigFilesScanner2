using System;
using System.Linq;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M01
{
    public static class PersonasValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case PersonasEnum.nro_documento:

                    scService.ValidateNroDocumento(fd, l, f);

                    FieldData<T> tdoc = fields.FirstOrDefault(a => a.FieldType.Equals(PersonasEnum.tipo_documento));

                    scService.ValidateNroDocumentoExistence(tdoc, fd, l, f, true, true);

                    break;
                case PersonasEnum.usuario:
                    scService.ValidateUsuario(fd, l, f);
                    break;
                case PersonasEnum.fecha_nacimiento:
                    scService.ValidateFechaNacimiento(fd, l, f);
                    break;
                case PersonasEnum.fecha_ingreso_pais:
                    scService.ValidateFechaIngresoPais(fields, l, f);
                    break;
                case PersonasEnum.email:
                    scService.ValidateEmail(fd, l, f);
                    break;
                case PersonasEnum.nacionalidad:
                    scService.ValidateNacionalidad(fd, l, f);
                    break;
            }
        }

    }
}
