using System;

using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;

namespace PasifaeG3Migrations.Modules.M08
{
    public static class ActasCursadaValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            //switch (fd.Type)
            //{
            //    case ActasCursadaEnum.origen:
            //        scService.ValidateIsCharInRange(fd, l, f, new[] { 'R', 'P' });
            //        break;
            //    case ActasCursadaEnum.nro_acta:

            //        break;
            //    case ActasCursadaEnum.fecha:
            //    case ActasCursadaEnum.fecha_vigencia:
            //        scService.ValidateDateFormat(fd, l, f);
            //        break;
            //    case ActasCursadaEnum.resultado:
            //        scService.ValidateIsCharInRange(fd, l, f, new[] { 'A', 'R', 'U' });
            //        break;
            //    case ActasCursadaEnum.tipo_documento:
            //        scService.ValidateTipoDocumento(fd, SIUGTables.mdp_tipos_documentos, l, f);
            //        break;
            //    case ActasCursadaEnum.nro_documento:

            //        FieldData<T> ftype = fields.FirstOrDefault(a => a.Type.Equals(ActasCursadaEnum.tipo_documento));
            //        // scService.ValidateNroDocumentoExistence(ftype, fd, l, f);

            //        break;
            //    case ActasCursadaEnum.actividad_codigo:
            //        scService.ValidateActivityCodeExistence(fd, l, f);

            //        FieldData<T> auxfd = fields.Where(a => a.Type.Equals(ActasCursadaEnum.nro_documento)).ElementAt(0);
            //        FieldData<T> auxfd2 = fields.Where(a => a.Type.Equals(ActasCursadaEnum.periodo_lectivo_nombre)).ElementAt(0);

            //        scService.ValidateDuplicatedActa(fd, auxfd, auxfd2, l, f);

            //        break;
            //    case ActasCursadaEnum.escala_nota:
            //        scService.ValidateEscalaNotaExistence(fd, l, f);
            //        break;
            //    case ActasCursadaEnum.anio_academico:
            //        scService.ValidateAnioAcademicoExistence(fd, l, f);
            //        break;
            //    case ActasCursadaEnum.periodo_lectivo_nombre:
            //        scService.ValidatePeriodoNombreExistence(fd, l, f);
            //        //scService.ValidatePeriodoNombreAniooAcademicos();
            //        break;
            //}


        }


    }
}