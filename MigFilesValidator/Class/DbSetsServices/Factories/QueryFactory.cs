
using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
using PasifaeG3Migrations.Class.DbSetsServices.DbModels;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.AuxiliarStructs;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.MDP;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.SGA;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.Views;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures;

using System;
using System.IO;
using System.Text;
using GuaraniMigFilesScanner.Class.FilesManagement;
using PasifaeG3Migrations.Class.Extras;

namespace PasifaeG3.DbSetsServices.Factories
{
    public static class QueryFactory
    {
        public static string GetQuery<T>() where T : DbModel
        {
            string query = string.Empty;

            switch (typeof(T).Name)
            {
                case nameof(Persona):
                    query = GetQuery<MdpPersonasDocumentos>(G3Table.mdp_personas_documentos);
                    break;

                case nameof(Alumno):
                    query = GetQuery<VwAlumnos, MdpPersonasDocumentos>(G3Table.vw_alumnos,
                        G3Table.mdp_personas_documentos, VwAlumnos.persona, MdpPersonasDocumentos.persona);
                    break;

                case nameof(Plan):
                    query = GetQuery<VwPlanes>(G3Table.vw_planes); // Necesita revisón ¿porque son lo mismo?
                    break;

                case nameof(Propuesta):
                    query = GetQuery<VwPlanes>(G3Table.vw_planes); // Necesita revisón ¿porque son lo mismo?
                    break;

                case nameof(Elemento):
                    query = GetQuery<VwElementos>(G3Table.vw_elementos);
                    break;

                case nameof(PropuestaElementos):
                    query = GetQuery<VwElementosPlan>(G3Table.vw_elementos_plan);
                    break;

                case nameof(EscalaNota):
                    query = GetQuery<VwEscalasNotas>(G3Table.vw_escalas_notas);
                    break;

                case nameof(PeriodoLectivo):
                    query = GetQuery<VwPeriodosLectivos>(G3Table.vw_periodos_lectivos);
                    break;

                case nameof(Periodo):
                    query = GetQuery<SgaPeriodos>(G3Table.sga_periodos);
                    break;

                case nameof(ActaRegularesDetalle):
                    query = GetActasQuery(ResourceQueryName.GetAR);
                    break;

                case nameof(ActaRegularPromocionDetalle):
                    query = GetActasQuery(ResourceQueryName.GetAP);
                    break;

                case nameof(ActaExamen):
                    query = GetActasQuery(ResourceQueryName.GetAEdet);
                    break;

                case nameof(TurnosExamen):
                    query = GetQuery<AuxTurnoExamen, AuxLlamadoExamen, SgaPeriodos>(G3Table.vw_turnos_examen, G3Table.sga_periodos,
                        VwTurnosExamen.turno_examen_periodo, SgaPeriodos.periodo, VwTurnosExamen.llamado_periodo, SgaPeriodos.periodo);
                    break;

                case nameof(LibroActas):
                    query = GetQuery<SgaLibrosActas>(G3Table.sga_libros_actas);
                    break;

                case nameof(MesaExamen):
                    query = GetQuery<VwMesasExamen>(G3Table.vw_mesas_examen);
                    break;

                case nameof(Llamado): // Es Necesario ?
                    //
                    break;

                case nameof(Comision): // vw_comisiones no anda para SSALV Test
                    query = GetQuery<SgaComisiones, VwElementos, VwPeriodosLectivos>(G3Table.sga_comisiones, G3Table.vw_elementos, G3Table.vw_periodos_lectivos,
                        SgaComisiones.elemento, VwElementos.elemento, SgaComisiones.periodo_lectivo, VwPeriodosLectivos.periodo_lectivo);
                    break;

                case nameof(AnioAcademico):
                    query = GetQuery<SgaAniosAcademicos>(G3Table.sga_anios_academicos);
                    break;
            }
            return query;
        }
        private static string GetActasQuery(ResourceQueryName type)
        {
            string queryFile = $"{FileHandler.ResourcesPath}{type}.sql";
            if (!File.Exists(queryFile)) return null;

            string query = File.ReadAllText(queryFile);

            return query;
        }

        private static string GetQuery<T>(G3Table table, G3Schema schema = G3Schema.negocio)
               where T : Enum => $"select {GetTableFields<T>()} from {schema}.{table}";
        private static string GetQuery<T, T2>(G3Table table, G3Table joinTable, Enum t1Key, Enum t2Key,
            G3Schema schema = G3Schema.negocio) where T : Enum where T2 : Enum
        {
            string schTable = $"{schema}.{table}";

            StringBuilder sb = new();

            sb.Append($"select {GetTableFields<T>("t1")}, {GetTableFields<T2>("t2")} from {schTable} t1 ");

            schTable = $"{schema}.{joinTable}";

            sb.Append($"join {schTable} t2 on t1.{t1Key} = t2.{t2Key}");

            return sb.ToString();
        }
        private static string GetQuery<T, T2, T3>(G3Table table, G3Table joinTable, G3Table joinTable2,
            Enum tKey, Enum jKey, Enum tKey2, Enum jKey2, G3Schema schema = G3Schema.negocio)
            where T : Enum where T2 : Enum where T3 : Enum
        {
            string schTable = $"{schema}.{table}";

            StringBuilder sb = new();

            sb.Append($"select {GetTableFields<T>("t1")}, {GetTableFields<T2>("t2")}, {GetTableFields<T3>("t3")} from {schTable} t1 ");

            schTable = $"{schema}.{joinTable}";

            sb.Append($"join {schTable} t2 on t1.{tKey} = t2.{jKey}");

            schTable = $"{schema}.{joinTable2}";

            sb.Append($"join {schTable} t3 on t1.{tKey2} = t3.{jKey2}");

            return sb.ToString();
        }
        private static string GetQuery<T, T2, T3>(G3Table table, G3Table joinTable,
           Enum tKey, Enum jKey, Enum tKey2, Enum jKey2, G3Schema schema = G3Schema.negocio)
           where T : Enum where T2 : Enum where T3 : Enum
        {
            string schTable = $"{schema}.{table}";

            StringBuilder sb = new();

            sb.Append($"select {GetTableFields<T>("t1")}, {GetTableFields<T2>("t2")}, {GetTableFields<T>("t1")}, {GetTableFields<T2>("t3")} from {schTable} t1 ");

            schTable = $"{schema}.{joinTable}";

            sb.Append($"join {schTable} t2 on t1.{tKey} = t2.{jKey}");

            schTable = $"{schema}.{joinTable}";

            sb.Append($"join {schTable} t3 on t1.{tKey2} = t3.{jKey2}");

            return sb.ToString();
        }

        private static string GetTableFields<T>(string tkey = null)
        {
            string statament;

            if (string.IsNullOrWhiteSpace(tkey))
            {
                statament = Enum.GetNames(typeof(T)).ToStatament(',');
            }
            else
            {
                statament = Enum.GetNames(typeof(T)).ToStatament(',', tkey);
            }

            return statament;
        }

        private enum ResourceQueryName
        {
            None,
            GetAR,
            GetAP,
            GetAEdet
        }
    }
}


//using GuaraniMigFilesScanner.Class.FilesManagement;

//using PasifaeG3Migrations.Class.DbSetsServices.DbModels;
//using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.AuxiliarStructs;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.MDP;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.SGA;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.Views;
//using PasifaeG3Migrations.Class.Extras;

//using System;
//using System.IO;
//using System.Text;

//namespace PasifaeG3Migrations.Class.DbSetsServices.Factories
//{
//    public static class QueryFactory
//    {
//        public static string GetQuery<T>() where T : DbModel
//        {
//            string query = string.Empty;

//            switch (typeof(T).Name)
//            {
//                case nameof(Persona):
//                    query = GetQuery<MdpPersonasDocumentos>(G3Table.mdp_personas_documentos);
//                    break;

//                case nameof(Alumno):
//                    query = GetQuery<VwAlumnos, MdpPersonasDocumentos>(G3Table.vw_alumnos,
//                        G3Table.mdp_personas_documentos, VwAlumnos.persona, MdpPersonasDocumentos.persona);
//                    break;

//                case nameof(Plan):
//                    query = GetQuery<VwPlanes>(G3Table.vw_planes); // Necesita revisón ¿porque son lo mismo?
//                    break;

//                case nameof(Propuesta):
//                    query = GetQuery<VwPlanes>(G3Table.vw_planes); // Necesita revisón ¿porque son lo mismo?
//                    break;

//                case nameof(Elemento):
//                    query = GetQuery<VwElementos>(G3Table.vw_elementos);
//                    break;

//                case nameof(PropuestaElementos):
//                    query = GetQuery<VwElementosPlan>(G3Table.vw_elementos_plan);
//                    break;

//                case nameof(EscalaNota):
//                    query = GetQuery<VwEscalasNotas>(G3Table.vw_escalas_notas);
//                    break;

//                case nameof(PeriodoLectivo):
//                    query = GetQuery<VwPeriodosLectivos>(G3Table.vw_periodos_lectivos);
//                    break;

//                case nameof(Periodo):
//                    query = GetQuery<SgaPeriodos>(G3Table.sga_periodos);
//                    break;

//                case nameof(ActaRegularesDetalle):
//                    query = GetActasQuery(ResourceQueryName.GetAR);
//                    break;

//                case nameof(ActaRegularPromocionDetalle):
//                    query = GetActasQuery(ResourceQueryName.GetAP);
//                    break;

//                case nameof(ActaExamen):
//                    query = GetActasQuery(ResourceQueryName.GetAEdet);
//                    break;

//                case nameof(TurnosExamen):
//                    query = GetQuery<AuxTurnoExamen, AuxLlamadoExamen, SgaPeriodos>(G3Table.vw_turnos_examen, G3Table.sga_periodos,
//                        VwTurnosExamen.turno_examen_periodo, SgaPeriodos.periodo, VwTurnosExamen.llamado_periodo, SgaPeriodos.periodo);
//                    break;

//                case nameof(LibroActas):
//                    query = GetQuery<SgaLibrosActas>(G3Table.sga_libros_actas);
//                    break;

//                case nameof(Llamado): // Es Necesario ?
//                    //
//                    break;

//                case nameof(Comision): // vw_comisiones no anda para SSALV Test
//                    query = GetQuery<SgaComisiones, VwElementos, VwPeriodosLectivos>(G3Table.sga_comisiones, G3Table.vw_elementos, G3Table.vw_periodos_lectivos,
//                        SgaComisiones.elemento, VwElementos.elemento, SgaComisiones.periodo_lectivo, VwPeriodosLectivos.periodo_lectivo);
//                    break;

//                case nameof(AnioAcademico):
//                    query = GetQuery<SgaAniosAcademicos>(G3Table.sga_anios_academicos);
//                    break;
//            }
//            return query;
//        }
//        private static string GetActasQuery(ResourceQueryName type)
//        {
//            return GetResourceQuery(type);
//        }

//        private static string GetQuery<T>(G3Table table, G3Schema schema = G3Schema.negocio)
//               where T : Enum => $"select {GetTableFields<T>()} from {schema}.{table}";
//        private static string GetQuery<T, T2>(G3Table table, G3Table joinTable, Enum t1Key, Enum t2Key,
//            G3Schema schema = G3Schema.negocio) where T : Enum where T2 : Enum
//        {
//            string schTable = $"{schema}.{table}";

//            StringBuilder sb = new();

//            sb.Append($"select {GetTableFields<T>("t1")}, {GetTableFields<T2>("t2")} from {schTable} t1 ");

//            schTable = $"{schema}.{joinTable}";

//            sb.Append($"join {schTable} t2 on t1.{t1Key} = t2.{t2Key}");

//            return sb.ToString();
//        }
//        private static string GetQuery<T, T2, T3>(G3Table table, G3Table joinTable, G3Table joinTable2,
//            Enum tKey, Enum jKey, Enum tKey2, Enum jKey2, G3Schema schema = G3Schema.negocio)
//            where T : Enum where T2 : Enum where T3 : Enum
//        {
//            string schTable = $"{schema}.{table}";

//            StringBuilder sb = new();

//            sb.Append($"select {GetTableFields<T>("t1")}, {GetTableFields<T2>("t2")}, {GetTableFields<T3>("t3")} from {schTable} t1 ");

//            schTable = $"{schema}.{joinTable}";

//            sb.Append($"join {schTable} t2 on t1.{tKey} = t2.{jKey}");

//            schTable = $"{schema}.{joinTable2}";

//            sb.Append($"join {schTable} t3 on t1.{tKey2} = t3.{jKey2}");

//            return sb.ToString();
//        }
//        private static string GetQuery<T, T2, T3>(G3Table table, G3Table joinTable,
//           Enum tKey, Enum jKey, Enum tKey2, Enum jKey2, G3Schema schema = G3Schema.negocio)
//           where T : Enum where T2 : Enum where T3 : Enum
//        {
//            string schTable = $"{schema}.{table}";

//            StringBuilder sb = new();

//            sb.Append($"select {GetTableFields<T>("t1")}, {GetTableFields<T2>("t2")}, {GetTableFields<T>("t1")}, {GetTableFields<T2>("t3")} from {schTable} t1 ");

//            schTable = $"{schema}.{joinTable}";

//            sb.Append($"join {schTable} t2 on t1.{tKey} = t2.{jKey}");

//            schTable = $"{schema}.{joinTable}";

//            sb.Append($"join {schTable} t3 on t1.{tKey2} = t3.{jKey2}");

//            return sb.ToString();
//        }
//        private static string GetResourceQuery(ResourceQueryName rqtype)
//        {
//            string queryFile = $"{FileHandler.ResourcesPath}{rqtype}.sql";
//            if (!File.Exists(queryFile)) return null;

//            string query = File.ReadAllText(queryFile);

//            return query;
//        }
//        private static string GetTableFields<T>(string tkey = null)
//        {
//            string statament;

//            if (string.IsNullOrWhiteSpace(tkey))
//            {
//                statament = Enum.GetNames(typeof(T)).ToStatament(',');
//            }
//            else
//            {
//                statament = Enum.GetNames(typeof(T)).ToStatament(',', tkey);
//            }

//            return statament;
//        }

//        private enum ResourceQueryName
//        {
//            None,
//            GetAR,
//            GetAP,
//            GetAEdet
//        }
//    }
//}
