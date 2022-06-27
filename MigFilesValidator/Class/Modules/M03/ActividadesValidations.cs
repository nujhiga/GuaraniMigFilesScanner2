using System;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M03
{
    public static class ActividadesValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            switch (fd.FieldType)
            {
                case ActividadesEnum.codigo:
                    scService.ValidateActivityCodeExistence(fd, l, f);
                    break;
                case ActividadesEnum.estado:
                    scService.ValidateActividadesEstado(fd, l, f);
                    break;
                case ActividadesEnum.disponible_para:
                    scService.ValidateActividadesDisponiblePara(fd, l, f);
                    break;
            }
        }

    }
}
