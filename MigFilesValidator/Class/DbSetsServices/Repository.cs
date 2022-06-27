using Npgsql;

using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
using PasifaeG3Migrations.Class.DbSetsServices.Factories;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.AuxiliarStructs;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.SGA;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.Views;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuaraniMigFilesScanner.Class.Connections;
using PasifaeG3.DbSetsServices.Factories;

namespace GuaraniMigFilesScanner
{
    public class Repository
    {
        public readonly Connection Conn;
        public Repository(Connection conn)
        {
            Conn = conn;
        }
        public async ValueTask<ConcurrentDictionary<object, Alumno>> GetAlumnosAsync()
        {
            string query = QueryFactory.GetQuery<Alumno>();

            ConcurrentDictionary<object, Alumno> alumnos =
                await DbModelFactory.GetModelsDictAsync<Alumno, GenericKeyFields>(
                    query, Conn, GenericKeyFields.tipo_documento, GenericKeyFields.nro_documento, GenericKeyFields.propuesta);
            { }
            return alumnos;
        }
        public async ValueTask<ConcurrentDictionary<object, Persona>> GetPersonasAsync()
        {
            string query = QueryFactory.GetQuery<Persona>();

            ConcurrentDictionary<object, Persona> personas =
                await DbModelFactory.GetModelsDictAsync<Persona, GenericKeyFields>
                 (query, Conn, GenericKeyFields.tipo_documento, GenericKeyFields.nro_documento);

            return personas;
        }
        public async ValueTask<ConcurrentDictionary<object, PropuestaElementos>> GetPropuestasElementosAsync()
        {
            string query = QueryFactory.GetQuery<PropuestaElementos>();

            ConcurrentDictionary<object, PropuestaElementos> propuestas =
                await DbModelFactory.GetModelsTreeAsync<PropuestaElementos>(query, Conn);

            return propuestas;
        }
        public async ValueTask<ConcurrentDictionary<object, ActaExamen>> GetActasExamenesAsync()
        {
            string query = QueryFactory.GetQuery<ActaExamen>();

            ConcurrentDictionary<object, ActaExamen> actasExamenes =
                await DbModelFactory.GetModelsTreeAsync<ActaExamen>(query, Conn);

            return actasExamenes;
        }
        public async ValueTask<ConcurrentDictionary<object, ActaCursada<ActaRegularesDetalle>>> GetActasRegularesAsync()
        {
            string query = QueryFactory.GetQuery<ActaRegularesDetalle>();

            ConcurrentDictionary<object, ActaCursada<ActaRegularesDetalle>> actasExamenes =
                await DbModelFactory.GetModelsTreeAsync<ActaCursada<ActaRegularesDetalle>>(query, Conn);

            return actasExamenes;
        }

        public async ValueTask<ConcurrentDictionary<object, ActaCursada<ActaRegularPromocionDetalle>>> GetActasPromocionAsync()
        {
            string query = QueryFactory.GetQuery<ActaRegularPromocionDetalle>();

            ConcurrentDictionary<object, ActaCursada<ActaRegularPromocionDetalle>> actasExamenes =
                await DbModelFactory.GetModelsTreeAsync<ActaCursada<ActaRegularPromocionDetalle>>(query, Conn);

            return actasExamenes;
        }

        public async ValueTask<List<Elemento>> GetElementosAsync()
        {
            string query = QueryFactory.GetQuery<Elemento>();
            {}
            IEnumerable<Elemento> elementos = await DbModelFactory.GetEnumerableModelsAsync<Elemento>(query, Conn);

            return elementos.ToList();
        }

        public async ValueTask<List<Propuesta>> GetPropuestasAsync()
        {
            string query = QueryFactory.GetQuery<Propuesta>();

            IEnumerable<Propuesta> propuestas =
                await DbModelFactory.GetEnumerableModelsAsync<Propuesta>(query, Conn);

            return propuestas.ToList();
        }

        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaMesasExamen()
        {

            using var cmd = new NpgsqlCommand("select nombre from negocio.vw_mesas_examen", await Conn.OpenAsync());

            using var rdr = await cmd.ExecuteReaderAsync();

            ConcurrentDictionary<string, int> periodos = new ConcurrentDictionary<string, int>();

            int index = 0;

            while (await rdr.ReadAsync())
            {
                periodos.TryAdd(rdr.GetValue(0).ToString(), index);

                index++;
            }

            return periodos;
        }



        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaPeriodosNames()
        {

            using var cmd = new NpgsqlCommand("select nombre from negocio.sga_periodos", await Conn.OpenAsync());

            using var rdr = await cmd.ExecuteReaderAsync();

            ConcurrentDictionary<string, int> periodos = new ConcurrentDictionary<string, int>();

            int index = 0;

            while (await rdr.ReadAsync())
            {
                periodos.TryAdd(rdr.GetValue(0).ToString(), index);

                index++;
            }

            return periodos;
        }

        public async ValueTask<ConcurrentDictionary<string, int>> GetElementsCodes()
        {
            using var cmd = new NpgsqlCommand("select elemento from negocio.sga_elementos", await Conn.OpenAsync());

            using var rdr = await cmd.ExecuteReaderAsync();

            ConcurrentDictionary<string, int> codes = new ConcurrentDictionary<string, int>();

            int index = 0;

            while (await rdr.ReadAsync())
            {
                codes.TryAdd(rdr.GetValue(0).ToString(), index);
                index++;
            }


            Conn.Dispose();

            return codes;
        }

        //public async ValueTask<ConcurrentDictionary<string, string>> GetSgaElementosCodes()
        //{
        //    using var cmd = new NpgsqlCommand("select codigo from negocio.sga_elementos", await Conn.OpenAsync());

        //    using var rdr = await cmd.ExecuteReaderAsync();

        //    ConcurrentDictionary<string, string> codes = new ConcurrentDictionary<string, string>();

        //    int index = 0;

        //    while (await rdr.ReadAsync())
        //    {
        //        codes.TryAdd(rdr.GetValue(0).ToString(), index.ToString());
        //        index++;
        //    }

        //    Conn.Dispose();

        //    return codes;
        //}

        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaEscalasNotas()
        {
            using var cmd = new NpgsqlCommand("select escala_nota from negocio.sga_escalas_notas", await Conn.OpenAsync());

            using var rdr = await cmd.ExecuteReaderAsync();

            ConcurrentDictionary<string, int> escalas = new ConcurrentDictionary<string, int>();

            int index = 0;

            while (await rdr.ReadAsync())
            {
                escalas.TryAdd(rdr.GetValue(0).ToString(), index);

                index++;
            }

            Conn.Dispose();


            return escalas;
        }

        public async ValueTask<IEnumerable<AnioAcademico>> GetAniosAcademicosAsync()
        {
            string query = QueryFactory.GetQuery<AnioAcademico>();

            IEnumerable<AnioAcademico> anios =
                await DbModelFactory.GetEnumerableModelsAsync<AnioAcademico>(query, Conn);

            return anios;
        }

        public async ValueTask<ConcurrentDictionary<object, LibroActas>> GetLibrosActasAsync()
        {
            /*string query = QueryFactory.GetQuery<LibroActas>();

            ConcurrentDictionary<object, LibroActas> libros =
                await DbModelFactory.GetModelsDictAsync<LibroActas, LibrosDeActasEnum>(
                    query, Conn, LibrosDeActasEnum.nro_libro);*/

            return null;
        }


        public async ValueTask<ConcurrentDictionary<object, MesaExamen>> GetMesasExamenAsync()
        {
            string query = QueryFactory.GetQuery<MesaExamen>();
            {}
            ConcurrentDictionary<object, MesaExamen> mesas =
                await DbModelFactory.GetModelsDictAsync<MesaExamen, VwMesasExamen>(query, Conn, VwMesasExamen.mesa_examen_nombre,
                VwMesasExamen.turno_examen_nombre, VwMesasExamen.anio_academico, VwMesasExamen.mesa_examen_elemento);

            return mesas;
        }

        public async ValueTask<ConcurrentDictionary<object, EscalaNota>> GetEscalasNotasAsync()
        {
            string query = QueryFactory.GetQuery<EscalaNota>();

            ConcurrentDictionary<object, EscalaNota> periodos =
                await DbModelFactory.GetModelsDictAsync<EscalaNota, VwEscalasNotas>(query, Conn, VwEscalasNotas.escala_nota);

            return periodos;
        }
        public async ValueTask<ConcurrentDictionary<object, Periodo>> GetPeriodosAsync()
        {
            string query = QueryFactory.GetQuery<Periodo>();

            ConcurrentDictionary<object, Periodo> periodos =
                await DbModelFactory.GetModelsDictAsync<Periodo, SgaPeriodos>(query, Conn, SgaPeriodos.nombre);

            return periodos;
        }

        //public async ValueTask<ConcurrentDictionary<string, int>> GetSgaAnioAcademicos()
        //{

        //    using var cmd = new NpgsqlCommand("select anio_academico from negocio.sga_anios_academicos", await Conn.OpenAsync());

        //    using var rdr = await cmd.ExecuteReaderAsync();

        //    ConcurrentDictionary<string, int> anios = new ConcurrentDictionary<string, int>();

        //    int index = 0;

        //    while (await rdr.ReadAsync())
        //    {
        //        anios.TryAdd(rdr.GetValue(0).ToString(), index);

        //        index++;
        //    }

        //    Conn.Dispose();


        //    return anios;
        //}

    }
}


//using Npgsql;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using GuaraniMigFilesScanner.Class.Connections;
//using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
//using PasifaeG3Migrations.Class.DbSetsServices.Factories;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.AuxiliarStructs;

//namespace PasifaeG3Migrations.Class.DbSetsServices
//{
//    public class Repository
//    {
//        public readonly Connection Conn;
//        public Repository(Connection conn) => Conn = conn;
//        public async ValueTask<ConcurrentDictionary<object, Alumno>> GetAlumnosAsync()
//        {
//            string query = QueryFactory.GetQuery<Alumno>();

//            ConcurrentDictionary<object, Alumno> alumnos =
//                await DbModelFactory.GetModelsDictAsync<Alumno, GenericKeyFields>(
//                    query, Conn, GenericKeyFields.tipo_documento, GenericKeyFields.nro_documento, GenericKeyFields.propuesta);

//            return alumnos;
//        }
//        public async ValueTask<ConcurrentDictionary<object, Persona>> GetPersonasAsync()
//        {
//            string query = QueryFactory.GetQuery<Persona>();

//            ConcurrentDictionary<object, Persona> personas =
//                await DbModelFactory.GetModelsDictAsync<Persona, GenericKeyFields>
//                 (query, Conn, GenericKeyFields.tipo_documento, GenericKeyFields.nro_documento);

//            return personas;
//        }
//        public async ValueTask<ConcurrentDictionary<object, PropuestaElementos>> GetPropuestasElementosAsync()
//        {
//            string query = QueryFactory.GetQuery<PropuestaElementos>();

//            ConcurrentDictionary<object, PropuestaElementos> propuestas =
//                await DbModelFactory.GetModelsTreeAsync<PropuestaElementos>(query, Conn);

//            return propuestas;
//        }
//        public async ValueTask<ConcurrentDictionary<object, ActaExamen>> GetActasExamenesAsync()
//        {
//            string query = QueryFactory.GetQuery<ActaExamen>();

//            ConcurrentDictionary<object, ActaExamen> actasExamenes =
//                await DbModelFactory.GetModelsTreeAsync<ActaExamen>(query, Conn);

//            return actasExamenes;
//        }
//        public async ValueTask<ConcurrentDictionary<object, ActaCursada<ActaRegularesDetalle>>> GetActasRegularesAsync()
//        {
//            string query = QueryFactory.GetQuery<ActaRegularesDetalle>();

//            ConcurrentDictionary<object, ActaCursada<ActaRegularesDetalle>> actasExamenes =
//                await DbModelFactory.GetModelsTreeAsync<ActaCursada<ActaRegularesDetalle>>(query, Conn);

//            return actasExamenes;
//        }
//        public async ValueTask<ConcurrentDictionary<object, ActaCursada<ActaRegularPromocionDetalle>>> GetActasPromocionAsync()
//        {
//            string query = QueryFactory.GetQuery<ActaRegularPromocionDetalle>();

//            ConcurrentDictionary<object, ActaCursada<ActaRegularPromocionDetalle>> actasExamenes =
//                await DbModelFactory.GetModelsTreeAsync<ActaCursada<ActaRegularPromocionDetalle>>(query, Conn);

//            return actasExamenes;
//        }
//        public async ValueTask<List<Propuesta>> GetPropuestasAsync()
//        {
//            string query = QueryFactory.GetQuery<Propuesta>();

//            List<Propuesta> propuestas =
//                await DbModelFactory.GetModelsListAsync<Propuesta>(query, Conn);

//            return propuestas;
//        }
//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaMesasExamen()
//        {

//            using var cmd = new NpgsqlCommand("select nombre from negocio.sga_mesas_examen", await Conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> periodos = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                periodos.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }

//            return periodos;
//        }
//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaPeriodosNames()
//        {

//            using var cmd = new NpgsqlCommand("select nombre from negocio.sga_periodos", await Conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> periodos = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                periodos.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }

//            return periodos;
//        }
//        public async ValueTask<ConcurrentDictionary<string, int>> GetElementsCodes()
//        {
//            using var cmd = new NpgsqlCommand("select elemento from negocio.sga_elementos", await Conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> codes = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                codes.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }


//            Conn.Dispose();

//            return codes;
//        }
//        public async ValueTask<ConcurrentDictionary<string, string>> GetSgaElementosCodes()
//        {
//            using var cmd = new NpgsqlCommand("select codigo from negocio.sga_elementos", await Conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, string> codes = new ConcurrentDictionary<string, string>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                codes.TryAdd(rdr.GetValue(0).ToString(), index.ToString());
//                index++;
//            }

//            Conn.Dispose();

//            return codes;
//        }
//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaEscalasNotas()
//        {
//            using var cmd = new NpgsqlCommand("select escala_nota from negocio.sga_escalas_notas", await Conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> escalas = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                escalas.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }

//            Conn.Dispose();


//            return escalas;
//        }
//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaAnioAcademicos()
//        {

//            using var cmd = new NpgsqlCommand("select anio_academico from negocio.sga_anios_academicos", await Conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> anios = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                anios.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }

//            Conn.Dispose();


//            return anios;
//        }
//    }
//}
