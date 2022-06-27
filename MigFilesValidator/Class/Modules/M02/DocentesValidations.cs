using System;
using GuaraniMigFilesScanner.Class.RulesSources;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M02
{
    public static class DocentesValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case DocentesEnum.cuit_cuil:
                    scService.ValidateCuitCuil(fd, l, f);
                    break;
                case DocentesEnum.nro_documento:
                    scService.ValidateNroDocumento(fd, l, f);
                    break;
                case DocentesEnum.fecha_nacimiento:
                    scService.ValidateFechaNacimiento(fd, l, f);
                    break;
                case DocentesEnum.fecha_ingreso_pais:
                    scService.ValidateFechaIngresoPais(fields, l, f);
                    break;
                case DocentesEnum.localidad_nacimiento:
                    scService.ValidateLocalidad(fd, SIUGTables.mug_localidades, l, f);
                    break;
                case DocentesEnum.localidad:
                    scService.ValidateLocalidad(fd, SIUGTables.mug_localidades, l, f);
                    break;
                case DocentesEnum.email:
                    scService.ValidateEmail(fd, l, f);
                    break;
                case DocentesEnum.nacionalidad:
                    scService.ValidateNacionalidad(fd, l, f);
                    break;
                case DocentesEnum.pais_documento:
                    scService.ValidatePaisDocumento(fd, SIUGTables.mug_paises, l, f);
                    break;
                case DocentesEnum.pais_origen:
                    scService.ValidatePaisOrigen(fields, SIUGTables.mug_paises, l, f);
                    break;
                case DocentesEnum.tipo_documento:
                    scService.ValidateTipoDocumento(fd, SIUGTables.mdp_tipos_documentos, l, f);
                    break;
                case DocentesEnum.sexo:
                    scService.ValidateSexo(fd, l, f);
                    break;
                case DocentesEnum.docente_legajo:
                    scService.ValidateDocenteLegajo(fd, l, f);
                    break;
            }
        }

    }
}
