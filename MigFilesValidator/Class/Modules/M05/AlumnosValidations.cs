using System;
using System.Linq;

using GuaraniMigFilesScanner.Class.RulesSources;
using GuaraniMigFilesScanner.Class.Scanner;
using GuaraniMigFilesScanner.Class.Scanner.FieldsData;
using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;

namespace PasifaeG3Migrations.Modules.M05
{



    public static class AlumnosValidations
    {
        public static void Validate<T>(ScannerService<T> scService, FieldData<T>[] fields, int l, int f)
            where T : Enum
        {
            FieldData<T> fd = fields[f];

            Propuesta auxprop = null;

            switch (fd.FieldType)
            {
                case AlumnosEnum.legajo:
                    break;
                case AlumnosEnum.tipo_documento:
                  //  scService.ValidateTipoDocumento(fd, SIUGTables.mdp_tipos_documentos, l, f);
                    break;
                case AlumnosEnum.nro_documento:

                    FieldData<T> tdoc = fields.FirstOrDefault(a => a.FieldType.Equals(AlumnosEnum.tipo_documento));
                    FieldData<T> prop = fields.FirstOrDefault(a => a.FieldType.Equals(AlumnosEnum.propuesta));

                    //scService.ValidateNroDocumento(fd, l, f);

                    scService.ValidateNroDocumentoExistence(tdoc, fd, l, f, false, true);

                    scService.ValidateDuplicatedAlumnos(fd, tdoc, prop, l, f);

                    scService.ValidateAlumnoExistence(tdoc, fd, prop, l, f, true, true);

                    break;
                case AlumnosEnum.propuesta:

                 //   auxprop = scService.ValidatePropuesta_PlanEstado(fd, l, f);

                 //   FieldData<T> currPlan = fields.FirstOrDefault(a => a.FieldType.Equals(AlumnosEnum.plan_version_actual));

                    //scService.ValidatePropuesta(fd, currPlan, l, f);

                    // Validate In Client Data Table 
                    break;
                case AlumnosEnum.plan_version_ingreso when auxprop != null:

                    //FieldData<T> plana = fields.FirstOrDefault(a => a.Type.Equals(AlumnosEnum.plan_version_actual));
                    //scService.ValidatePlanVerIngreso(plana, fd, auxprop, l, f);

                    // Validate In Client Data Table 
                    break;
                case AlumnosEnum.plan_version_actual when auxprop != null:

                  //  FieldData<T> plani = fields.FirstOrDefault(a => a.Type.Equals(AlumnosEnum.plan_version_ingreso));
                {}
                 //   scService.ValidatePlanVerActual(fd, plani, auxprop, l, f);


                    // Validate In Client Data Table 
                    break;
                case AlumnosEnum.ubicacion:
                    break;
                case AlumnosEnum.modalidad:
                    scService.ValidateIsCharInRange(fd, l, f, new[] { 'P', 'A' }); // = ValidateInSIUGTable
                    break;
                case AlumnosEnum.regular:
                    scService.ValidateIsCharInRange(fd, l, f, new[] { 'S', 'N' });
                    break;
                case AlumnosEnum.calidad:
                    scService.ValidateIsCharInRange(fd, l, f, new[] { 'A', 'P' }); // = ValidateInSIUGTable
                    break;
                case AlumnosEnum.anio_academico:
                  //  scService.ValidateIntegerDataType(fd, l, f, 4);
                    break;

                case AlumnosEnum.estado_inscripcion:
                    scService.ValidateIsCharInRange(fd, l, f, new[] { 'P', 'A', 'R' });
                    break;
                case AlumnosEnum.noreg_anio_academico:
                    break;
                case AlumnosEnum.noreg_fecha:
                case AlumnosEnum.pasivo_fecha:
                case AlumnosEnum.fecha_inscripcion:
                case AlumnosEnum.egre_fecha_egreso:
                case AlumnosEnum.plan_version_actual_fecha:
                    scService.ValidateDateFormat(fd, l, f);
                    break;
                case AlumnosEnum.noreg_causa:
                    break;
                case AlumnosEnum.pasivo_motivo:
                    break;
                case AlumnosEnum.egre_titulo:
                    break;
                case AlumnosEnum.egre_nro_expediente:
                    break;
                case AlumnosEnum.egre_promedio:
                    break;
                case AlumnosEnum.egre_promedio_sin_aplazos:
                    break;
                case AlumnosEnum.egre_observaciones:
                    break;
            }
        }
    }
}
