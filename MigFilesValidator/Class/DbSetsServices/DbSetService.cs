//using Npgsql;
//using System;
//using System.Collections.Concurrent;
//using System.Threading.Tasks;
//using GuaraniMigFilesScanner.Class.Connections;
//using GuaraniMigFilesScanner.Class.DbSetsServices.Models;

//namespace GuaraniMigFilesScanner.Class.DbSetsServices
//{
//    public enum VwPlanesFields
//    {
//        propuesta,
//        propuesta_codigo,
//        plan,
//        plan_codigo,
//        plan_version,
//        plan_version_actual
//    }

//    public enum SgaPeriodosFields
//    {
//        periodo,
//        nombre,
//        anio_academico,
//        fecha_inicio,
//        fecha_fin
//    }

//    public class DbSetService
//    {
//        private Connection _conn;

//        public DbSetService(Connection conn)
//        {
//            _conn = conn;
//        }

//        public async ValueTask<NpgsqlCommand> GetCommandAsync(GuaraniTable gTable, Connection conn)
//        {
//            NpgsqlCommand cmd = null;

//            switch (gTable)
//            {
//                case GuaraniTable.sga_periodos:
//                    cmd = new NpgsqlCommand(Extensions.GetQuery<SgaPeriodosFields>(gTable), await conn.OpenAsync());
//                    break;
//                case GuaraniTable.vw_planes:
//                    cmd = new NpgsqlCommand(Extensions.GetQuery<VwPlanesFields>(gTable), await conn.OpenAsync());
//                    break;
//            }

//            return cmd;
//        }
//        public async ValueTask<NpgsqlDataReader> GetReaderAsync(GuaraniTable gTable, Connection conn)
//        {
//            using var cmd = await GetCommandAsync(gTable, _conn);
//            return await cmd.ExecuteReaderAsync();
//        }

//        //public async ValueTask<ConcurrentDictionary<int, Periodo>> GetSgaPeriodos()
//        //{
//        //    using var rdr = await GetReaderAsync(GuaraniTable.sga_periodos, _conn);

//        //}

//        public async ValueTask<ConcurrentDictionary<int, Propuesta>> GetVwPlanes()
//        {
//            using var cmd = new NpgsqlCommand(Extensions.GetQuery<VwPlanesFields>(GuaraniTable.vw_planes), await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<int, Propuesta> propuestas = new ConcurrentDictionary<int, Propuesta>();

//            while (await rdr.ReadAsync())
//            {
//                object[] values = new object[6];
//                rdr.GetValues(values);

//                int propID = values[0] == DBNull.Value ? -1 : (int)values[0];
//                string propCod = values[1] == DBNull.Value ? string.Empty : values[1].ToString();
//                int planID = values[2] == DBNull.Value ? -1 : (int)values[2];
//                string planCod = values[3] == DBNull.Value ? string.Empty : values[3].ToString();
//                int planVer = values[4] == DBNull.Value ? -1 : (int)values[4];
//                int planVerAct = values[5] == DBNull.Value ? -1 : (int)values[5];

//                Propuesta prop = new Propuesta()
//                {
//                    ID = propID,
//                    Codigo = propCod,
//                    Plan = new Plan()
//                    {
//                        ID = planID,
//                        Codigo = planCod,
//                        Version = planVer,
//                        VersionActual = planVerAct
//                    }
//                };

//                propuestas.TryAdd(planID, prop);
//            };

//            _conn.Dispose();

//            return propuestas;

//        }

//        public async ValueTask<ConcurrentDictionary<string, int>> GetAlumnosDb()
//        {
//            using var cmd = new NpgsqlCommand("select p.tipo_documento, p.nro_documento, a.propuesta from negocio.mdp_personas_documentos p join negocio.sga_alumnos a on p.persona = a.persona", await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> dnis = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                string reg = $"{rdr.GetValue(0)}-{rdr.GetValue(1)}-{rdr.GetValue(2)}";
//                dnis.TryAdd(reg, index);

//                index++;
//            }

//            _conn.Dispose();

//            return dnis;
//        }

//        public async ValueTask<ConcurrentDictionary<string, int>> GetPersonasDb()
//        {
//            using var cmd = new NpgsqlCommand("select tipo_documento, nro_documento from negocio.mdp_personas_documentos", await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> dnis = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                string reg = $"{rdr.GetValue(0)}-{rdr.GetValue(1)}";
//                dnis.TryAdd(reg, index);

//                index++;
//            }

//            _conn.Dispose();

//            return dnis;
//        }

//        public async ValueTask<ConcurrentDictionary<string, int>> GetMdpPersonasDocumentos()
//        {
//            using var cmd = new NpgsqlCommand("select nro_documento from negocio.mdp_personas_documentos", await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> dnis = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                dnis.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }

//            _conn.Dispose();

//            return dnis;
//        }

//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaPeriodosNames()
//        {

//            using var cmd = new NpgsqlCommand("select nombre from negocio.sga_periodos", await _conn.OpenAsync());

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
//            using var cmd = new NpgsqlCommand("select elemento from negocio.sga_elementos", await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> codes = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                codes.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }


//            _conn.Dispose();

//            return codes;
//        }
//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaElementosCodes()
//        {
//            using var cmd = new NpgsqlCommand("select codigo from negocio.sga_elementos", await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> codes = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                codes.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }


//            _conn.Dispose();

//            return codes;
//        }

//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaEscalasNotas()
//        {
//            using var cmd = new NpgsqlCommand("select escala_nota from negocio.sga_escalas_notas", await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> escalas = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                escalas.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }

//            _conn.Dispose();


//            return escalas;
//        }

//        public async ValueTask<ConcurrentDictionary<string, int>> GetSgaAnioAcademicos()
//        {

//            using var cmd = new NpgsqlCommand("select anio_academico from negocio.sga_anios_academicos", await _conn.OpenAsync());

//            using var rdr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<string, int> anios = new ConcurrentDictionary<string, int>();

//            int index = 0;

//            while (await rdr.ReadAsync())
//            {
//                anios.TryAdd(rdr.GetValue(0).ToString(), index);

//                index++;
//            }

//            _conn.Dispose();


//            return anios;
//        }

//    }
//}
