using Npgsql;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
using PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces;
using PasifaeG3Migrations.Class.DbSetsServices.DbModels;
using PasifaeG3Migrations.Class.DbSetsServices.Factories;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.AuxiliarStructs;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.MDP;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.SGA;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.Views;
using PasifaeG3Migrations.Class.Extras;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using GuaraniMigFilesScanner.Class.Connections;

namespace GuaraniMigFilesScanner
{

    public static class FileModelFactory
    {
        /*
        public static IEnumerable<PersonaFile> GetPersonasFileModels(MigFile migFile) => GetFileModels<PersonaFile>(migFile);
        public static IEnumerable<AlumnoFile> GetAlumnosFileModels(MigFile migFile)  => GetFileModels<AlumnoFile>(migFile);
        public static IEnumerable<ActaExamenFile> GetActasExamenFileModels(MigFile migFile) => GetFileModels<ActaExamenFile>(migFile);
        public static IEnumerable<ActaExamenDetalleFile> GetActasExamenDetalleFileModels(MigFile migFile)=> GetFileModels<ActaExamenDetalleFile>(migFile);
        *//*
        private static TModel FromFields<TModel>(FieldData[] fields, int requieredFields)
        {
            TModel def = Activator.CreateInstance<TModel>();

            var tModelProps = typeof(TModel).GetProperties();

            foreach (var prop in tModelProps)
            {
                //if (Enum.TryParse(typeof(TEnum), prop.Name, out object tenum))
                //{
                //    object propVal = fields.ReadField(tenum, requieredFields);

                //    if (propVal.GetType() == typeof(ErrCode))
                //    {
                //        if (!propVal.Equals(ErrCode.NullNullable))
                //        {
                //            Console.WriteLine(propVal + " - " + prop.Name);
                //        }
                //        continue;
                //    }

                //    prop.SetValue(def, propVal);
                //}
            }

            return def;
        }*/

        //private static TModel FromFields<TModel, TEnum>(FieldData<TEnum>[] fields, int requieredFields)
        //   where TEnum : Enum
        //{

        //    TModel def = Activator.CreateInstance<TModel>();

        //    var tModelProps = typeof(TModel).GetProperties();

        //    foreach (var prop in tModelProps)
        //    {
        //        if (Enum.TryParse(typeof(TEnum), prop.Name, out object tenum))
        //        {
        //            object propVal = fields.ReadField(tenum, requieredFields);

        //            if (propVal.GetType() == typeof(ErrCode))
        //            {
        //                if (!propVal.Equals(ErrCode.NullNullable))
        //                {
        //                    Console.WriteLine(propVal + " - " + prop.Name);
        //                }
        //                continue;
        //            }

        //            prop.SetValue(def, propVal);
        //        }
        //    }

        //    return def;
        //}
        /*
        private static IEnumerable<TModel> GetFileModels<TModel>(MigFile migFile) 
        {
            int lCount = 1;
            var auxLines = migFile.Lines.Where(l => !string.IsNullOrWhiteSpace(l));
            int linesCount = auxLines.Count();
            int l = 0;
            var obj = new object();

            Collection<TModel> models = new Collection<TModel>();

            auxLines.AsParallel().AsOrdered().ForAll((line) =>
            {
                Console.Write($"\r > Cargando Registro {lCount} de {linesCount} Registros.");

                string[] lData;

                lock (obj) lData = line.Split('|');

                FieldData[] fields = BackgroupScanner.GetFields(migFile, lData, l);

                TModel model = FromFields<TModel>(fields, migFile.RequiredFields);

                lock (models) models.Add(model);

                Interlocked.Increment(ref l);
                Interlocked.Increment(ref lCount);
            });

            return models;
        }
        */
        //private static IEnumerable<TModel> GetFileModels<TModel, TFile>(MigFile<TFile> migFile) where TFile : Enum
        //{
        //    int lCount = 1;
        //    var auxLines = migFile.Lines.Where(l => !string.IsNullOrWhiteSpace(l));
        //    int linesCount = auxLines.Count();
        //    int l = 0;
        //    var obj = new object();

        //    Collection<TModel> models = new Collection<TModel>();

        //    auxLines.AsParallel().AsOrdered().ForAll((line) =>
        //    {
        //        Console.Write($"\r > Cargando Registro {lCount} de {linesCount} Registros.");

        //        string[] lData;

        //        lock (obj) lData = line.Split('|');

        //        FieldData<TFile>[] fields = BackgroupScanner.GetFields(migFile, lData, l);

        //        TModel model = FromFields<TModel, TFile>(fields, migFile.RequiredFields);

        //        lock (models) models.Add(model);

        //        Interlocked.Increment(ref l);
        //        Interlocked.Increment(ref lCount);
        //    });

        //    return models;
        //}
    }

    //public static class FileModelFactory
    //{

    //    public static IEnumerable<PersonaFile> GetPersonasFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetFileModels<PersonaFile, TFile>(migFile);
    //    public static IEnumerable<AlumnoFile> GetAlumnosFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetFileModels<AlumnoFile, TFile>(migFile);
    //    public static IEnumerable<ActaExamenFile> GetActasExamenFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetFileModels<ActaExamenFile, TFile>(migFile);
    //    public static IEnumerable<ActaExamenDetalleFile> GetActasExamenDetalleFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetFileModels<ActaExamenDetalleFile, TFile>(migFile);

    //    private static TModel FromFields<TModel, TEnum>(FieldData<TEnum>[] fields, int requieredFields)
    //       where TEnum : Enum
    //    {

    //        TModel def = Activator.CreateInstance<TModel>();

    //        var tModelProps = typeof(TModel).GetProperties();

    //        foreach (var prop in tModelProps)
    //        {
    //            if (Enum.TryParse(typeof(TEnum), prop.Name, out object tenum))
    //            {
    //                object propVal = fields.ReadField(tenum, requieredFields);

    //                if (propVal.GetType() == typeof(ErrCode))
    //                {
    //                    if (!propVal.Equals(ErrCode.NullNullable))
    //                    {
    //                        Console.WriteLine(propVal + " - " + prop.Name);
    //                    }
    //                    continue;
    //                }

    //                prop.SetValue(def, propVal);
    //            }
    //        }

    //        return def;
    //    }

    //    private static IEnumerable<TModel> GetFileModels<TModel, TFile>(MigFile<TFile> migFile) where TFile : Enum
    //    {
    //        int lCount = 1;
    //        var auxLines = migFile.Lines.Where(l => !string.IsNullOrWhiteSpace(l));
    //        int linesCount = auxLines.Count();
    //        int l = 0;
    //        var obj = new object();

    //        Collection<TModel> models = new Collection<TModel>();

    //        auxLines.AsParallel().AsOrdered().ForAll((line) =>
    //        {
    //            Console.Write($"\r > Cargando Registro {lCount} de {linesCount} Registros.");

    //            string[] lData;

    //            lock (obj) lData = line.Split('|');

    //            FieldData<TFile>[] fields = BackgroupScanner.GetFields(migFile, lData, l);

    //            TModel model = FromFields<TModel, TFile>(fields, migFile.RequiredFields);

    //            lock (models) models.Add(model);

    //            Interlocked.Increment(ref l);
    //            Interlocked.Increment(ref lCount);
    //        });

    //        return models;
    //    }
    //}






    public static class DbModelFactory
    {
        public static async ValueTask<ConcurrentDictionary<object, T>> GetModelsTreeAsync<T>(string query, Connection conn)
        {
            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

            using var dr = await cmd.ExecuteReaderAsync();

            ConcurrentDictionary<object, T> models = null;

            if (typeof(T).FullName.Contains(nameof(ActaRegularesDetalle)))
            {
                return await GetActasCursada<ActaRegularesDetalle>(dr) as ConcurrentDictionary<object, T>;
            }
            else if (typeof(T).FullName.Contains(nameof(ActaRegularPromocionDetalle)))
            {
                return await GetActasCursada<ActaRegularPromocionDetalle>(dr) as ConcurrentDictionary<object, T>;
            }

            switch (typeof(T).Name)
            {
                case nameof(PropuestaElementos):
                    models = await GetPropuestasElementosAsync(dr, VwElementosPlan.propuesta) as ConcurrentDictionary<object, T>;
                    break;

                case nameof(ActaExamen):
                    models = await GetActasExamenes(dr) as ConcurrentDictionary<object, T>;
                    break;
            }

            return models;
        }

        public static async ValueTask<IEnumerable<T>> GetEnumerableModelsAsync<T>(string query, Connection conn) where T : DbModel
        {
            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

            using var dr = await cmd.ExecuteReaderAsync();

            Collection<T> models = new Collection<T>();

            while (await dr.ReadAsync())
            {
                if (!dr.HasRows) continue;

                T model = GetModel<T>(dr);

                models.Add(model);
            }

            conn.Dispose();

            return models;
        }

        public static async ValueTask<ConcurrentDictionary<object, T>> GetModelsDictAsync<T, TKeys>
            (string query, Connection conn, params TKeys[] keys) where TKeys : Enum where T : DbModel
        {
            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

            using var dr = await cmd.ExecuteReaderAsync();

            ConcurrentDictionary<object, T> models = new();
            {}
            while (await dr.ReadAsync())
            {
                if (!dr.HasRows) continue;

                T model = GetModel<T>(dr);

                string key = DbModelKeyBuilder.BuildKey(dr, keys);
                { }
                models.TryAdd(key, model);
            }
            { }
            conn.Dispose();

            return models;
        }



        /*
        public static async ValueTask<ConcurrentDictionary<object, T>> GetModelsDictAsync<T, TKey>
            (string query, Connection conn, TKey keyType) where TKey : Enum where T : DbModel
        {
            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

            using var dr = await cmd.ExecuteReaderAsync();

            ConcurrentDictionary<object, T> models = new();

            if (typeof(T) == typeof(PropuestaElementos))
                return await GetPropuestasElementosAsync(dr, VwElementosPlan.propuesta) as ConcurrentDictionary<object, T>;

            while (await dr.ReadAsync())
            {
                if (!dr.HasRows) continue;

                T model = GetModel<T>(dr);

                object key = dr.Get<object>(keyType);

                models.TryAdd(key, model);
            }

            conn.Dispose();

            return models;
        }*/



        private static T GetModel<T>(NpgsqlDataReader dr) where T : DbModel
        {
            T model = default;

            switch (typeof(T).Name)
            {
                case nameof(Persona):
                    model = GetPersona(dr) as T;
                    break;

                case nameof(Alumno):
                    model = GetAlumno(dr) as T;
                    break;

                case nameof(Plan):
                    //model = GetPropuesta(dr) as T;
                    break;

                case nameof(Propuesta):
                    model = GetPropuesta(dr) as T;
                    break;

                case nameof(Elemento):
                    model = GetElemento(dr) as T;
                    break;

                case nameof(EscalaNota):
                    model = GetEscalaNota(dr) as T;
                    break;

                case nameof(PeriodoLectivo):
                    model = GetPeriodoLectivo(dr) as T;
                    break;

                case nameof(Periodo):
                    model = GetPeriodo(dr) as T;
                    break;

                case nameof(TurnosExamen):
                    model = GetTurnosExamen(dr) as T;
                    break;

                case nameof(MesaExamen):
                    model = GetMesasExamen(dr) as T;
                    break;

                case nameof(Comision):
                    model = GetComision(dr) as T;
                    break;

                case nameof(AnioAcademico):
                    model = GetAnioAcademico(dr) as T;
                    break;

                case nameof(LibroActas):
                    model = GetLibrosActas(dr) as T;
                    break;
            }

            return model;
        }
        private static LibroActas GetLibroActa(NpgsqlDataReader dr)
        {
            LibroActas libroActa = new()
            {
                ID = dr.Get<int>(SgaLibrosActas.libro),

                NroLibro = dr.Get<string>(SgaLibrosActas.nro_libro),

                NroTomo = dr.Get<int>(SgaLibrosTomos.nro_tomo),

                Activo = dr.Get<string>(SgaLibrosActas.es_libro_activo)
            };

            return libroActa;
        }
        private static LibroActas GetLibrosActas(NpgsqlDataReader dr)
        {
            LibroActas libroActas = new()
            {
                ID = dr.Get<int>(SgaLibrosActas.libro),

                NroLibro = dr.Get<string>(SgaLibrosActas.nro_libro),

                Nombre = dr.Get<string>(SgaLibrosActas.nombre),

                FechaActivacion = dr.Get<DateTime>(SgaLibrosActas.fecha_activacion),

                FechaFinVigencia = dr.Get<DateTime>(SgaLibrosActas.fecha_fin_vigencia),

                Activo = dr.Get<string>(SgaLibrosActas.es_libro_activo),

                AnioAcademico = GetAnioAcademico(dr)
            };

            return libroActas;
        }
        private static Comision GetComision(NpgsqlDataReader dr)
        {
            Comision comision = new()
            {
                ID = dr.Get<int>(SgaComisiones.comision),

                Nombre = dr.Get<string>(SgaComisiones.nombre),

                InscripcionHab = dr.Get<string>(SgaComisiones.inscripcion_habilitada),

                InscripcionCerrada = dr.Get<string>(SgaComisiones.inscripcion_cerrada),

                Turno = dr.Get<int>(SgaComisiones.turno),

                Ubicacion = dr.Get<int>(SgaComisiones.ubicacion),

                Anio = GetAnioAcademico(dr),

                Periodo = GetPeriodoLectivo(dr),

                Elemento = GetElemento(dr)
            };

            return comision;
        }
        private static Comision GetComisionActaRegularesPromocion(NpgsqlDataReader dr)
        {
            Comision comision = new()
            {
                ID = dr.Get<int>(SgaComisiones.comision),

                Nombre = dr.Get<string>(AuxSgaComisiones.cNombre),

                Elemento = GetElementoActa(dr)
            };

            return comision;
        }

        private static AnioAcademico GetAnioAcademico(NpgsqlDataReader dr)
            => new() { Anio = dr.Get<decimal>(SgaPeriodos.anio_academico) };

        //private static Periodo GetPeriodo(NpgsqlDataReader dr)
        //{
        //    Periodo periodo = new()
        //    {
        //        ID = dr.Get<int>(SgaPeriodos.periodo),

        //        Nombre = dr.Get<string>(SgaPeriodos.nombre),

        //        Inicio = dr.Get<DateTime>(SgaPeriodos.fecha_inicio),

        //        Fin = dr.Get<DateTime>(SgaPeriodos.fecha_fin),

        //        Anio = GetAnioAcademico(dr)
        //    };

        //    return periodo;
        //}

        private static PeriodoLectivo GetPeriodoLectivo(NpgsqlDataReader dr)
        {
            PeriodoLectivo periodoLectivo = new()
            {
                ID = dr.Get<int>(VwPeriodosLectivos.periodo_lectivo),

                Nombre = dr.Get<string>(VwPeriodosLectivos.nombre),

                Inicio = dr.Get<DateTime>(VwPeriodosLectivos.fecha_inicio),

                Fin = dr.Get<DateTime>(VwPeriodosLectivos.fecha_fin),

                Anio = GetAnioAcademico(dr)
            };

            return periodoLectivo;
        }
        private static PeriodoLectivo GetPeriodoLectivoActaCursada(NpgsqlDataReader dr)
        {
            PeriodoLectivo periodoLectivo = new()
            {
                ID = dr.Get<int>(SgaPeriodos.periodo),

                Nombre = dr.Get<string>(AuxSgaPeriodos.pNombre)
            };

            return periodoLectivo;
        }
        private static EscalaNota GetEscalaNota(NpgsqlDataReader dr)
        {
            EscalaNota escalaNota = new()
            {
                ID = dr.Get<int>(VwEscalasNotas.escala_nota),

                Numerica = dr.Get<string>(VwEscalasNotas.es_numerica),

                CantidadDecimales = dr.Get<short>(VwEscalasNotas.cantidad_decimales),

                SeparadorDecimal = dr.Get<string>(VwEscalasNotas.separador_decimal),

                NotaInicial = dr.Get<decimal>(VwEscalasNotas.nota_inicial),

                NotaFinal = dr.Get<decimal>(VwEscalasNotas.nota_final),

               // Estado = dr.Get<string>(VwEscalasNotas.estado),

                Resultado = dr.Get<string>(VwEscalasNotas.resultado)
            };

            return escalaNota;
        }
        private static EscalaNota GetEscalaNotaActa(NpgsqlDataReader dr)
        {
            EscalaNota escalaNota = new()
            {
                ID = dr.Get<int>(VwEscalasNotas.escala_nota)
            };

            return escalaNota;
        }
        private static Persona GetPersona(NpgsqlDataReader dr)
        {
            Persona per = new()
            {
                ID = dr.Get<int>(MdpPersonasDocumentos.persona),

                NroDocumento = dr.Get<string>(MdpPersonasDocumentos.nro_documento),

                TipoDocumento = dr.Get<short>(MdpPersonasDocumentos.tipo_documento)
            };

            return per;
        }
        private static Alumno GetAlumno(NpgsqlDataReader dr)
        {
            Alumno alumno = new()
            {
                ID = dr.Get<int>(VwAlumnos.alumno),

                Propuesta = GetPropuestaAlumno(dr),

                Persona = GetPersona(dr),

                Ubicacion = dr.Get<int>(VwAlumnos.ubicacion)
            };

            return alumno;
        }
        private static Elemento GetElemento(NpgsqlDataReader dr)
        {
            Elemento elemento = new()
            {
                ID = dr.Get<int>(SgaElementos.elemento),

                Codigo = dr.Get<string>(SgaElementos.codigo),

               // Estado = dr.Get<string>(SgaElementos.estado)
            };
            return elemento;
        }

        private static Elemento GetElementoMesa(NpgsqlDataReader dr)
        {
            Elemento elemento = new()
            {
                ID = dr.Get<int>(VwMesasExamen.mesa_examen_elemento)
            };
            return elemento;
        }

        private static Elemento GetElementoActa(NpgsqlDataReader dr)
        {
            Elemento elemento = new()
            {
                ID = dr.Get<int>(SgaElementos.elemento),

                Codigo = dr.Get<string>(SgaElementos.codigo),

               // Estado = dr.Get<string>(AuxSgaElementos.eEstado)
            };
            return elemento;
        }
        private static Propuesta GetPropuesta(NpgsqlDataReader dr)
        {
            Propuesta prop = new()
            {
                ID = dr.Get<int>(VwPlanes.propuesta),

                Codigo = dr.Get<string>(VwPlanes.propuesta_codigo),

                Estado = dr.Get<string>(VwPlanes.propuesta_estado),

                Plan = GetPropuestaPlan(dr)
            };

            { }

            return prop;
        }
        private static Propuesta GetPropuestaAlumno(NpgsqlDataReader dr)
        {
            Propuesta propuesta = new()
            {
                ID = dr.Get<int>(VwAlumnos.propuesta),

                Codigo = dr.Get<string>(VwAlumnos.propuesta_codigo),

                Plan = GetAlumnoPlan(dr)
            };
            return propuesta;
        }
        private static Plan GetPropuestaPlan(NpgsqlDataReader dr)
        {
            Plan plan = new()
            {
                ID = dr.Get<int>(VwPlan.plan),

                Codigo = dr.Get<string>(VwPlan.plan_codigo),

                VersionActual = dr.Get<int>(VwPlan.plan_version_actual),

                Estado = dr.Get<string>(VwPlan.plan_estado),

                Version = dr.Get<int>(VwPlan.plan_version),

                //VersionCodigo = dr.Get<string>(VwPlan.version_codigo),

                VersionEstado = dr.Get<string>(VwPlan.version_estado)
            };
            { }
            return plan;
        }

        private static Plan GetElementoPlan(NpgsqlDataReader dr)
        {
            Plan plan = new()
            {
                ID = dr.Get<int>(VwElementosPlan.plan),

                Codigo = dr.Get<string>(VwElementosPlan.plan_codigo),

                VersionActual = dr.Get<int>(VwElementosPlan.plan_version),

                Estado = dr.Get<string>(VwElementosPlan.plan_estado),

                Version = int.Parse(dr.Get<string>(VwElementosPlan.plan_version_version)),

                VersionEstado = dr.Get<string>(VwElementosPlan.plan_version_estado)
            };

            return plan;
        }
        private static Plan GetAlumnoPlan(NpgsqlDataReader dr)
        {
            Plan plan = new()
            {
                ID = dr.Get<int>(VwAlumnos.plan),

                Codigo = dr.Get<string>(VwAlumnos.plan_codigo),

                Version = dr.Get<int>(VwAlumnos.plan_version)
            };

            return plan;
        }
        private static TurnosExamen GetTurnosExamen(NpgsqlDataReader dr)
        {
            TurnosExamen turno = new()
            {
                ID = dr.Get<int>(AuxTurnoExamen.turno_examen),

                Nombre = dr.Get<string>(AuxTurnoExamen.turno_examen_nombre),

                Inicio = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_inicio),

                Fin = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_fin),

                Anio = GetAnioAcademico(dr),

                Periodo = GetPeriodo(dr),

                Llamado = GetLlamado(dr),
            };

            return turno;
        }

        private static TurnoExamen GetTurnoExamenMesa(NpgsqlDataReader dr)
        {
            TurnoExamen turnoExamen = new()
            {
                ID = dr.Get<int>(VwMesasExamen.turno_examen),

                Nombre = dr.Get<string>(VwMesasExamen.turno_examen_nombre)
            };

            return turnoExamen;
        }

        private static TurnoExamen GetTurnoExamen(NpgsqlDataReader dr)
        {
            TurnoExamen turnoExamen = new()
            {
                ID = dr.Get<int>(SgaTurnosExamen.turno_examen),

                Nombre = dr.Get<string>(AuxTurnoExamen.turno_examen_nombre),

                FechaExamenInicio = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_inicio),

                FechaExamenFin = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_fin),

                FechaInactivacion = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_inactivacion),

                PublicacionMesas = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_publicacion_mesas)
            };

            return turnoExamen;
        }

        private static MesaExamen GetMesasExamen(NpgsqlDataReader dr)
        {
            {}
            MesaExamen mesaExamen = new()
            {
                ID = dr.Get<int>(VwMesasExamen.mesa_examen),

                Nombre = dr.Get<string>(VwMesasExamen.mesa_examen_nombre),

                Ubicacion = dr.Get<int>(VwMesasExamen.mesa_examen_ubicacion),

                Turno = GetTurnoExamenMesa(dr),

                Elemento = GetElementoMesa(dr),

              //  Anio = GetAnioAcademico(dr)
            };

            return mesaExamen;
        }


        private static MesaExamen GetMesaExamenActa(NpgsqlDataReader dr)
        {
            MesaExamen mesaExamen = new()
            {
                ID = dr.Get<int>(SgaMesasExamen.mesa_examen),

                Nombre = dr.Get<string>(SgaMesasExamen.nombre),

                Ubicacion = dr.Get<int>(SgaMesasExamen.ubicacion),

                Elemento = GetElementoActa(dr)

                //Anio = GetAnioAcademico(dr)
            };

            return mesaExamen;
        }


        private static LlamadoExamen GetLlamado(NpgsqlDataReader dr)
        {
            LlamadoExamen llamado = new()
            {
                ID = dr.Get<int>(AuxLlamadoExamen.llamado),

                Nombre = dr.Get<string>(AuxLlamadoExamen.llamado_nombre),

                Inicio = dr.Get<DateTime>(AuxLlamadoExamen.llamado_fecha_inicio),

                Fin = dr.Get<DateTime>(AuxLlamadoExamen.llamado_fecha_fin),

                Periodo = GetPeriodo(dr)
            };

            return llamado;
        }
        private static Periodo GetPeriodo(NpgsqlDataReader dr)
        {
            Periodo periodo = new()
            {
                ID = dr.Get<int>(SgaPeriodos.periodo),

                Nombre = dr.Get<string>(SgaPeriodos.nombre),

                Inicio = dr.Get<DateTime>(SgaPeriodos.fecha_inicio),

                Fin = dr.Get<DateTime>(SgaPeriodos.fecha_fin),

                Anio = GetAnioAcademico(dr)
            };

            return periodo;
        }
        private static Folio GetFolio(NpgsqlDataReader dr)
        {
            Folio folio = new()
            {
                ID = dr.Get<short>(SgaActasFolios.folio),

                ActaID = dr.Get<int>(SgaActasFolios.id_acta),

                LibroTomo = dr.Get<int>(SgaActasFolios.libro_tomo),

                FolioFisico = dr.Get<int>(SgaActasFolios.folio_fisico)
            };

            return folio;
        }
        private static T GetActa<T>(NpgsqlDataReader dr) where T : Acta, new()
        {
            T acta = new()
            {
                ID = dr.Get<int>(AuxSgaActas.id_acta),

                NroActa = dr.Get<string>(AuxSgaActas.nro_acta),

                RenglonesFolio = dr.Get<short>(AuxSgaActas.renglones_folio),

                Fecha = dr.Get<DateTime>(AuxSgaActas.fecha_generacion),

                Estado = dr.Get<string>(AuxSgaActas.aEstado),

                Libro = GetLibroActa(dr),

                AnioAcademico = GetAnioAcademico(dr),

                Elemento = GetElementoActa(dr),

                EscalaNota = GetEscalaNotaActa(dr)
            };

            return acta;
        }
        private static ActaExamen GetActaExamen(NpgsqlDataReader dr)
        {
            ActaExamen actaExamen = GetActa<ActaExamen>(dr);

            actaExamen.TurnoExamen = GetTurnoExamen(dr);

            actaExamen.MesaExamen = GetMesaExamenActa(dr);

            return actaExamen;
        }
        private static ActaExamenDetalle GetActaExamenDetalle(NpgsqlDataReader dr)
        {
            ActaExamenDetalle actaDetalle = GetActaDetalle<ActaExamenDetalle>(dr);

            actaDetalle.ID = dr.Get<int>(AuxSgaActas.id_acta);

            actaDetalle.TipoInscripcion = dr.Get<short>(AuxSgaActas.instancia);

            return actaDetalle;
        }
        private static T GetActaDetalle<T>(NpgsqlDataReader dr) where T : IActaDetalle, new()
        {
            T detalle = new()
            {
                FechaDetalle = dr.Get<DateTime>(AuxSgaActas.fecha),

                Nota = dr.Get<string>(AuxSgaActas.nota),

                Resultado = dr.Get<string>(AuxSgaActas.resultado),

                Renglon = dr.Get<short>(AuxSgaActas.renglon),

                Alumno = GetAlumno(dr),

                Folio = GetFolio(dr)
            };

            return detalle;
        }
        private static PropuestaElementos GetPropuestaElementos(NpgsqlDataReader dr)
        {
            PropuestaElementos prop = new()
            {
                ID = dr.Get<int>(VwElementosPlan.propuesta),

                Codigo = dr.Get<string>(VwElementosPlan.propuesta_codigo),

                Plan = GetElementoPlan(dr),

                Elementos = new List<Elemento>()
            };

            return prop;
        }
        private static ActaCursada<T> GetActaCursada<T>(NpgsqlDataReader dr) where T : IActaDetalle
        {
            ActaCursada<T> actaRegulares = GetActa<ActaCursada<T>>(dr);

            actaRegulares.Comision = GetComisionActaRegularesPromocion(dr);

            actaRegulares.PeriodoLectivo = GetPeriodoLectivoActaCursada(dr);

            return actaRegulares;
        }
        private static IActaDetalle GetActaRegularesDetalle(NpgsqlDataReader dr)
        {
            ActaRegularesDetalle actaDetalle = GetActaDetalle<ActaRegularesDetalle>(dr);

            actaDetalle.Asistencia = dr.Get<decimal>(AuxSgaActas.pct_asistencia);

            actaDetalle.FechaVigencia = dr.Get<DateTime>(AuxSgaActas.fecha_vigencia);

            actaDetalle.CondRegularidad = dr.Get<int>(AuxSgaActas.cond_regularidad);

            return actaDetalle;
        }
        private static IActaDetalle GetActaPromocionDetalle(NpgsqlDataReader dr)
        {
            ActaRegularPromocionDetalle actaDetalle = GetActaDetalle<ActaRegularPromocionDetalle>(dr);
            return actaDetalle;
        }

        private static async ValueTask<ConcurrentDictionary<object, PropuestaElementos>>
            GetPropuestasElementosAsync<T>(NpgsqlDataReader dr, T keyType) where T : Enum
        {
            ConcurrentDictionary<object, PropuestaElementos> propuestas = new();

            while (await dr.ReadAsync())
            {
                if (!dr.HasRows) continue;

                object key = dr.Get<object>(keyType);

                if (!propuestas.TryGetValue(key, out PropuestaElementos propuesta))
                {
                    propuesta = GetPropuestaElementos(dr);

                    propuesta.Elementos.Add(GetElemento(dr));

                    propuestas.TryAdd(key, propuesta);
                }
                else
                {
                    propuesta.Elementos.Add(GetElemento(dr));
                }
            }

            return propuestas;
        }

        private static async ValueTask<ConcurrentDictionary<object, ActaCursada<T>>>
            GetActasCursada<T>(NpgsqlDataReader dr) where T : IActaDetalle
        {
            ConcurrentDictionary<object, ActaCursada<T>> actas = new();

            while (await dr.ReadAsync())
            {
                if (!dr.HasRows) continue;

                string key = DbModelKeyBuilder.BuildKey<ActaCursada<T>>(dr);

                if (!actas.TryGetValue(key, out ActaCursada<T> acta))
                {
                    acta = GetActaCursada<T>(dr);
                    { }
                    if (typeof(T) == typeof(ActaRegularesDetalle))
                        acta.Detalles.Add((T)GetActaRegularesDetalle(dr));
                    else
                        acta.Detalles.Add((T)GetActaPromocionDetalle(dr));

                    actas.TryAdd(key, acta);
                }
                else
                {
                    if (typeof(T) == typeof(ActaRegularesDetalle))
                        acta.Detalles.Add((T)GetActaRegularesDetalle(dr));
                    else
                        acta.Detalles.Add((T)GetActaPromocionDetalle(dr));
                }
            }

            return actas;
        }

        private static async ValueTask<ConcurrentDictionary<object, ActaExamen>>
            GetActasExamenes(NpgsqlDataReader dr)
        {
            ConcurrentDictionary<object, ActaExamen> actas = new();

            while (await dr.ReadAsync())
            {
                if (!dr.HasRows) continue;

                string key = DbModelKeyBuilder.BuildKey<ActaExamen>(dr);

                if (!actas.TryGetValue(key, out ActaExamen acta))
                {
                    acta = GetActaExamen(dr);

                    acta.Detalles.Add(GetActaExamenDetalle(dr));

                    actas.TryAdd(key, acta);
                }
                else
                {
                    acta.Detalles.Add(GetActaExamenDetalle(dr));
                }
            }

            return actas;
        }
    }
}



//using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
//using Npgsql;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using GuaraniMigFilesScanner.Class.Connections;
//using PasifaeG3Migrations.Class.DbSetsServices.DbModels;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.Views;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.SGA;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.MDP;
//using PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces;
//using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.AuxiliarStructs;
//using PasifaeG3Migrations.Class.Extras;
//using System.Collections.ObjectModel;

//namespace PasifaeG3Migrations.Class.DbSetsServices.Factories
//{

//    /*

//    public static class FileModelFactory
//    {
//        public static IEnumerable<PersonaFile> GetPersonasFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetModels<PersonaFile, TFile>(migFile);
//        public static IEnumerable<AlumnoFile> GetAlumnosFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetModels<AlumnoFile, TFile>(migFile);
//        public static IEnumerable<ActaExamenFile> GetActasExamenFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetModels<ActaExamenFile, TFile>(migFile);
//        public static IEnumerable<ActaExamenDetalleFile> GetActasExamenDetalleFileModels<TFile>(MigFile<TFile> migFile) where TFile : Enum => GetModels<ActaExamenDetalleFile, TFile>(migFile);

//        private static void SetModelIssues<TEnum>(object model, PropertyInfo[] mProperties, IEnumerable<Issue<TEnum>> fissues) where TEnum : Enum
//        {
//            PropertyInfo modelIssuesProp = mProperties.Single(p => p.Name.Equals("Issues"));

//            modelIssuesProp.SetValue(model, fissues);
//        }

//        private static TModel FromFields<TModel, TField>(IEnumerable<Field<TField>> fields, int lindex, int requiredFields) where TField : Enum
//        {
//            TModel model = Activator.CreateInstance<TModel>();

//            var tModelProps = typeof(TModel).GetProperties();

//            ICollection<Issue<TField>> fissues = new Collection<Issue<TField>>();

//            foreach (var prop in tModelProps)
//            {

//                if (!Enum.TryParse(typeof(TField), prop.Name, out object tenum)) continue;

//                object propVal = fields.ReadField(tenum, requiredFields);
//                int findex = fields.GetFindex(tenum);

//                if (propVal.GetType() == typeof(ErrCode))
//                {
//                    if (propVal.Equals(ErrCode.NullNullable)) continue;

//                    fissues.Add(new Issue<TField>((TField)tenum, (ErrCode)propVal, lindex, findex));

//                    continue;
//                }

//                prop.SetValue(model, propVal);
//            }

//            if (!fissues.IsNullOrEmpty()) SetModelIssues(model, tModelProps, fissues);

//            return model;
//        }

//        //PropertyInfo modelIssuesProp = tModelProps.Single(p => p.Name.Equals("Issues"));

//        //ModelIssues<TEnum> missues = new ModelIssues<TEnum>()
//        //{
//        //    Issues = fissues
//        //};

//        //modelIssuesProp.SetValue(model, missues);

//        //private static PropertyInfo GetIEnumerableProperty(object instance)
//        //{
//        //    PropertyInfo[] properties = instance.GetType().GetProperties();

//        //    PropertyInfo ienumerableProperty = properties.Where
//        //        (p => p.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)) &&
//        //        !p.PropertyType.GenericTypeArguments.IsNullOrEmpty()).FirstOrDefault();

//        //    return ienumerableProperty;
//        //}

//        //private static IEnumerable<T> CreateIEnumerable<T>(params object[] collectionValues)
//        //{
//        //    int length = collectionValues.Length;

//        //    Array array = Array.CreateInstance(typeof(T), length);

//        //    for (int i = 0; i < length; i++)
//        //    {
//        //        object value = collectionValues[i];
//        //        array.SetValue(value, i);
//        //    }

//        //    IEnumerable<T> enumResult = Enumerable.Cast<T>(array);

//        //    return enumResult;
//        //}

//        private static IEnumerable<TModel> GetModels<TModel, TFile>(MigFile<TFile> migFile)
//            where TFile : Enum
//        {
//            int lCount = 1;
//            var auxLines = migFile.Lines.Where(l => !string.IsNullOrWhiteSpace(l));
//            int linesCount = auxLines.Count();
//            int lindex = 0;
//            var obj = new object();
//            int rulesCount = migFile.Rules.Count;

//            ICollection<TModel> models = new Collection<TModel>();

//            auxLines.AsParallel().AsOrdered().ForAll((line) =>
//            {
//                Console.Write($"\r > Cargando Registro {lCount} de {linesCount} Registros.");

//                string[] ldata;

//                lock (obj) ldata = line.Split('|');

//                IEnumerable<Field<TFile>> fields = FieldDataFactory.GetFields(ldata, lindex, migFile.Rules);

//                TModel model = FromFields<TModel, TFile>(fields, lindex, rulesCount);

//                lock (models) models.Add(model);

//                Interlocked.Increment(ref lindex);
//                Interlocked.Increment(ref lCount);
//            });

//            return models;
//        }






//    }


//    */



//    public static class DbModelFactory
//    {

//        public static async ValueTask<IEnumerable<T>> GetEnumerableModelsAsync<T>(string query, Connection conn) where T : DbModel
//        {
//            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

//            using var dr = await cmd.ExecuteReaderAsync();

//            Collection<T> models = new Collection<T>();

//            while (await dr.ReadAsync())
//            {
//                if (!dr.HasRows) continue;

//                T model = GetModel<T>(dr);

//                models.Add(model);
//            }

//            conn.Dispose();

//            return models;
//        }

//        public static async ValueTask<ConcurrentDictionary<object, T>> GetModelsTreeAsync<T>(string query, Connection conn)
//        {
//            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

//            using var dr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<object, T> models = null;

//            if (typeof(T).FullName.Contains(nameof(ActaRegularesDetalle)))
//            {
//                return await GetActasCursada<ActaRegularesDetalle>(dr) as ConcurrentDictionary<object, T>;
//            }
//            else if (typeof(T).FullName.Contains(nameof(ActaRegularPromocionDetalle)))
//            {
//                return await GetActasCursada<ActaRegularPromocionDetalle>(dr) as ConcurrentDictionary<object, T>;
//            }

//            switch (typeof(T).Name)
//            {
//                case nameof(PropuestaElementos):
//                    models = await GetPropuestasElementosAsync(dr, VwElementosPlan.propuesta) as ConcurrentDictionary<object, T>;
//                    break;

//                case nameof(ActaExamen):
//                    models = await GetActasExamenes(dr) as ConcurrentDictionary<object, T>;
//                    break;
//            }

//            return models;
//        }
//        public static async ValueTask<List<T>> GetModelsListAsync<T>(string query, Connection conn) where T : DbModel
//        {
//            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

//            using var dr = await cmd.ExecuteReaderAsync();

//            List<T> models = new();

//            while (await dr.ReadAsync())
//            {
//                if (!dr.HasRows) continue;

//                T model = GetModel<T>(dr);

//                models.Add(model);
//            }

//            conn.Dispose();

//            return models;
//        }
//        public static async ValueTask<ConcurrentDictionary<object, T>> GetModelsDictAsync<T, TKeys>
//            (string query, Connection conn, params TKeys[] keys) where TKeys : Enum where T : DbModel
//        {
//            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

//            using var dr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<object, T> models = new();

//            while (await dr.ReadAsync())
//            {
//                if (!dr.HasRows) continue;

//                T model = GetModel<T>(dr);

//                string key = DbModelKeyBuilder.BuildKey(dr, keys);
//                { }
//                models.TryAdd(key, model);
//            }
//            { }
//            conn.Dispose();

//            return models;
//        }
//        public static async ValueTask<ConcurrentDictionary<object, T>> GetModelsDictAsync<T, TKey>
//            (string query, Connection conn, TKey keyType) where TKey : Enum where T : DbModel
//        {
//            using var cmd = new NpgsqlCommand(query, await conn.OpenAsync());

//            using var dr = await cmd.ExecuteReaderAsync();

//            ConcurrentDictionary<object, T> models = new();

//            if (typeof(T) == typeof(PropuestaElementos))
//                return await GetPropuestasElementosAsync(dr, VwElementosPlan.propuesta) as ConcurrentDictionary<object, T>;

//            while (await dr.ReadAsync())
//            {
//                if (!dr.HasRows) continue;

//                T model = GetModel<T>(dr);

//                object key = dr.Get<object>(keyType);

//                models.TryAdd(key, model);
//            }

//            conn.Dispose();

//            return models;
//        }

//        private static T GetModel<T>(NpgsqlDataReader dr) where T : DbModel
//        {
//            T model = default;

//            switch (typeof(T).Name)
//            {
//                case nameof(Persona):
//                    model = GetPersona(dr) as T;
//                    break;

//                case nameof(Alumno):
//                    model = GetAlumno(dr) as T;
//                    break;

//                case nameof(Plan):
//                    //model = GetPropuesta(dr) as T;
//                    break;

//                case nameof(Propuesta):
//                    model = GetPropuesta(dr) as T;
//                    break;

//                case nameof(Elemento):
//                    model = GetElemento(dr) as T;
//                    break;

//                case nameof(EscalaNota):
//                    model = GetEscalaNota(dr) as T;
//                    break;

//                case nameof(PeriodoLectivo):
//                    model = GetPeriodoLectivo(dr) as T;
//                    break;

//                case nameof(TurnosExamen):
//                    model = GetTurnosExamen(dr) as T;
//                    break;

//                case nameof(Comision):
//                    model = GetComision(dr) as T;
//                    break;

//                case nameof(AnioAcademico):
//                    model = GetAnioAcademico(dr) as T;
//                    break;

//                case nameof(LibroActas):
//                    model = GetLibrosActas(dr) as T;
//                    break;
//            }

//            return model;
//        }
//        private static LibroActas GetLibroActa(NpgsqlDataReader dr)
//        {
//            LibroActas libroActa = new()
//            {
//                ID = dr.Get<int>(SgaLibrosActas.libro),

//                NroLibro = dr.Get<string>(SgaLibrosActas.nro_libro),

//                NroTomo = dr.Get<int>(SgaLibrosTomos.nro_tomo),

//                Activo = dr.Get<string>(SgaLibrosActas.es_libro_activo)
//            };

//            return libroActa;
//        }
//        private static LibroActas GetLibrosActas(NpgsqlDataReader dr)
//        {
//            LibroActas libroActas = new()
//            {
//                ID = dr.Get<int>(SgaLibrosActas.libro),

//                NroLibro = dr.Get<string>(SgaLibrosActas.nro_libro),

//                Nombre = dr.Get<string>(SgaLibrosActas.nombre),

//                FechaActivacion = dr.Get<DateTime>(SgaLibrosActas.fecha_activacion),

//                FechaFinVigencia = dr.Get<DateTime>(SgaLibrosActas.fecha_fin_vigencia),

//                Activo = dr.Get<string>(SgaLibrosActas.es_libro_activo),

//                AnioAcademico = GetAnioAcademico(dr)
//            };

//            return libroActas;
//        }
//        private static Comision GetComision(NpgsqlDataReader dr)
//        {
//            Comision comision = new()
//            {
//                ID = dr.Get<int>(SgaComisiones.comision),

//                Nombre = dr.Get<string>(SgaComisiones.nombre),

//                InscripcionHab = dr.Get<string>(SgaComisiones.inscripcion_habilitada),

//                InscripcionCerrada = dr.Get<string>(SgaComisiones.inscripcion_cerrada),

//                Turno = dr.Get<int>(SgaComisiones.turno),

//                Ubicacion = dr.Get<int>(SgaComisiones.ubicacion),

//                Anio = GetAnioAcademico(dr),

//                Periodo = GetPeriodoLectivo(dr),

//                Elemento = GetElemento(dr)
//            };

//            return comision;
//        }
//        private static Comision GetComisionActaRegularesPromocion(NpgsqlDataReader dr)
//        {
//            Comision comision = new()
//            {
//                ID = dr.Get<int>(SgaComisiones.comision),

//                Nombre = dr.Get<string>(AuxSgaComisiones.cNombre),

//                Elemento = GetElementoActa(dr)
//            };

//            return comision;
//        }
//        private static AnioAcademico GetAnioAcademico(NpgsqlDataReader dr)
//            => new() { Anio = dr.Get<decimal>(SgaPeriodos.anio_academico) };
//        private static PeriodoLectivo GetPeriodoLectivo(NpgsqlDataReader dr)
//        {
//            PeriodoLectivo periodoLectivo = new()
//            {
//                ID = dr.Get<int>(VwPeriodosLectivos.periodo_lectivo),

//                Nombre = dr.Get<string>(VwPeriodosLectivos.nombre),

//                Inicio = dr.Get<DateTime>(VwPeriodosLectivos.fecha_inicio),

//                Fin = dr.Get<DateTime>(VwPeriodosLectivos.fecha_fin),

//                Anio = GetAnioAcademico(dr)
//            };

//            return periodoLectivo;
//        }
//        private static PeriodoLectivo GetPeriodoLectivoActaCursada(NpgsqlDataReader dr)
//        {
//            PeriodoLectivo periodoLectivo = new()
//            {
//                ID = dr.Get<int>(SgaPeriodos.periodo),

//                Nombre = dr.Get<string>(AuxSgaPeriodos.pNombre)
//            };

//            return periodoLectivo;
//        }
//        private static EscalaNota GetEscalaNota(NpgsqlDataReader dr)
//        {
//            EscalaNota escalaNota = new()
//            {
//                ID = dr.Get<int>(VwEscalasNotas.escala_nota),

//                Numerica = dr.Get<string>(VwEscalasNotas.es_numerica),

//                CantidadDecimales = dr.Get<short>(VwEscalasNotas.cantidad_decimales),

//                SeparadorDecimal = dr.Get<string>(VwEscalasNotas.separador_decimal),

//                NotaInicial = dr.Get<decimal>(VwEscalasNotas.nota_inicial),

//                NotaFinal = dr.Get<decimal>(VwEscalasNotas.nota_final),

//                Estado = dr.Get<string>(VwEscalasNotas.estado),

//                Resultado = dr.Get<string>(VwEscalasNotas.resultado)
//            };

//            return escalaNota;
//        }
//        private static EscalaNota GetEscalaNotaActa(NpgsqlDataReader dr)
//        {
//            EscalaNota escalaNota = new()
//            {
//                ID = dr.Get<int>(VwEscalasNotas.escala_nota)
//            };

//            return escalaNota;
//        }
//        private static Persona GetPersona(NpgsqlDataReader dr)
//        {
//            Persona per = new()
//            {
//                ID = dr.Get<int>(MdpPersonasDocumentos.persona),

//                NroDocumento = dr.Get<string>(MdpPersonasDocumentos.nro_documento),

//                TipoDocumento = dr.Get<short>(MdpPersonasDocumentos.tipo_documento)
//            };

//            return per;
//        }
//        private static Alumno GetAlumno(NpgsqlDataReader dr)
//        {
//            Alumno alumno = new()
//            {
//                ID = dr.Get<int>(VwAlumnos.alumno),

//                Propuesta = GetPropuestaAlumno(dr),

//                Persona = GetPersona(dr),

//                Ubicacion = dr.Get<int>(VwAlumnos.ubicacion)
//            };

//            return alumno;
//        }
//        private static Elemento GetElemento(NpgsqlDataReader dr)
//        {
//            Elemento elemento = new()
//            {
//                ID = dr.Get<int>(SgaElementos.elemento),

//                Codigo = dr.Get<string>(SgaElementos.codigo),

//                Estado = dr.Get<string>(SgaElementos.estado)
//            };
//            return elemento;
//        }
//        private static Elemento GetElementoActa(NpgsqlDataReader dr)
//        {
//            Elemento elemento = new()
//            {
//                ID = dr.Get<int>(SgaElementos.elemento),

//                Codigo = dr.Get<string>(SgaElementos.codigo),

//                Estado = dr.Get<string>(AuxSgaElementos.eEstado)
//            };
//            return elemento;
//        }
//        private static Propuesta GetPropuesta(NpgsqlDataReader dr)
//        {
//            Propuesta prop = new()
//            {
//                ID = dr.Get<int>(VwPlanes.propuesta),

//                Codigo = dr.Get<string>(VwPlanes.propuesta_codigo),

//                Estado = dr.Get<string>(VwPlanes.propuesta_estado),

//                Plan = GetPropuestaPlan(dr)
//            };

//            { }

//            return prop;
//        }
//        private static Propuesta GetPropuestaAlumno(NpgsqlDataReader dr)
//        {
//            Propuesta propuesta = new()
//            {
//                ID = dr.Get<int>(VwAlumnos.propuesta),

//                Codigo = dr.Get<string>(VwAlumnos.propuesta_codigo),

//                Plan = GetAlumnoPlan(dr)
//            };
//            return propuesta;
//        }
//        private static Plan GetPropuestaPlan(NpgsqlDataReader dr)
//        {
//            Plan plan = new()
//            {
//                ID = dr.Get<int>(VwPlan.plan),

//                Codigo = dr.Get<string>(VwPlan.plan_codigo),

//                VersionActual = dr.Get<int>(VwPlan.plan_version_actual),

//                Estado = dr.Get<string>(VwPlan.plan_estado),

//                Version = dr.Get<int>(VwPlan.plan_version),

//                //VersionCodigo = dr.Get<string>(VwPlan.version_codigo),

//                VersionEstado = dr.Get<string>(VwPlan.version_estado)
//            };
//            { }
//            return plan;
//        }

//        private static Plan GetElementoPlan(NpgsqlDataReader dr)
//        {
//            Plan plan = new()
//            {
//                ID = dr.Get<int>(VwElementosPlan.plan),

//                Codigo = dr.Get<string>(VwElementosPlan.plan_codigo),

//                VersionActual = dr.Get<int>(VwElementosPlan.plan_version),

//                Estado = dr.Get<string>(VwElementosPlan.plan_estado),

//                Version = int.Parse(dr.Get<string>(VwElementosPlan.plan_version_version)),

//                VersionEstado = dr.Get<string>(VwElementosPlan.plan_version_estado)
//            };

//            return plan;
//        }
//        private static Plan GetAlumnoPlan(NpgsqlDataReader dr)
//        {
//            Plan plan = new()
//            {
//                ID = dr.Get<int>(VwAlumnos.plan),

//                Codigo = dr.Get<string>(VwAlumnos.plan_codigo),

//                Version = dr.Get<int>(VwAlumnos.plan_version)
//            };

//            return plan;
//        }
//        private static TurnosExamen GetTurnosExamen(NpgsqlDataReader dr)
//        {
//            TurnosExamen turno = new()
//            {
//                ID = dr.Get<int>(AuxTurnoExamen.turno_examen),

//                Nombre = dr.Get<string>(AuxTurnoExamen.turno_examen_nombre),

//                Inicio = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_inicio),

//                Fin = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_fin),

//                Anio = GetAnioAcademico(dr),

//                Periodo = GetPeriodo(dr),

//                Llamado = GetLlamado(dr),
//            };

//            return turno;
//        }
//        private static TurnoExamen GetTurnoExamen(NpgsqlDataReader dr)
//        {
//            TurnoExamen turnoExamen = new()
//            {
//                ID = dr.Get<int>(SgaTurnosExamen.turno_examen),

//                Nombre = dr.Get<string>(AuxTurnoExamen.turno_examen_nombre),

//                FechaExamenInicio = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_inicio),

//                FechaExamenFin = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_fin),

//                FechaInactivacion = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_inactivacion),

//                PublicacionMesas = dr.Get<DateTime>(AuxTurnoExamen.turno_examen_fecha_publicacion_mesas)
//            };

//            return turnoExamen;
//        }
//        private static MesaExamen GetMesaExamen(NpgsqlDataReader dr)
//        {
//            MesaExamen mesaExamen = new()
//            {
//                ID = dr.Get<int>(SgaMesasExamen.mesa_examen),

//                Nombre = dr.Get<string>(SgaMesasExamen.nombre),

//                Ubicacion = dr.Get<int>(SgaMesasExamen.ubicacion),

//                Elemento = GetElementoActa(dr)

//                //Anio = GetAnioAcademico(dr)
//            };

//            return mesaExamen;
//        }
//        private static LlamadoExamen GetLlamado(NpgsqlDataReader dr)
//        {
//            LlamadoExamen llamado = new()
//            {
//                ID = dr.Get<int>(AuxLlamadoExamen.llamado),

//                Nombre = dr.Get<string>(AuxLlamadoExamen.llamado_nombre),

//                Inicio = dr.Get<DateTime>(AuxLlamadoExamen.llamado_fecha_inicio),

//                Fin = dr.Get<DateTime>(AuxLlamadoExamen.llamado_fecha_fin),

//                Periodo = GetPeriodo(dr)
//            };

//            return llamado;
//        }
//        private static Periodo GetPeriodo(NpgsqlDataReader dr)
//        {
//            Periodo periodo = new()
//            {
//                ID = dr.Get<int>(SgaPeriodos.periodo),

//                Nombre = dr.Get<string>(SgaPeriodos.nombre),

//                Inicio = dr.Get<DateTime>(SgaPeriodos.fecha_inicio),

//                Fin = dr.Get<DateTime>(SgaPeriodos.fecha_fin),

//                Anio = GetAnioAcademico(dr)
//            };

//            return periodo;
//        }
//        private static Folio GetFolio(NpgsqlDataReader dr)
//        {
//            Folio folio = new()
//            {
//                ID = dr.Get<short>(SgaActasFolios.folio),

//                ActaID = dr.Get<int>(SgaActasFolios.id_acta),

//                LibroTomo = dr.Get<int>(SgaActasFolios.libro_tomo),

//                FolioFisico = dr.Get<int>(SgaActasFolios.folio_fisico)
//            };

//            return folio;
//        }
//        private static T GetActa<T>(NpgsqlDataReader dr) where T : Acta, new()
//        {
//            T acta = new()
//            {
//                ID = dr.Get<int>(AuxSgaActas.id_acta),

//                NroActa = dr.Get<string>(AuxSgaActas.nro_acta),

//                RenglonesFolio = dr.Get<short>(AuxSgaActas.renglones_folio),

//                Fecha = dr.Get<DateTime>(AuxSgaActas.fecha_generacion),

//                Estado = dr.Get<string>(AuxSgaActas.aEstado),

//                Libro = GetLibroActa(dr),

//                AnioAcademico = GetAnioAcademico(dr),

//                Elemento = GetElementoActa(dr),

//                EscalaNota = GetEscalaNotaActa(dr)
//            };

//            return acta;
//        }
//        private static ActaExamen GetActaExamen(NpgsqlDataReader dr)
//        {
//            ActaExamen actaExamen = GetActa<ActaExamen>(dr);

//            actaExamen.TurnoExamen = GetTurnoExamen(dr);

//            actaExamen.MesaExamen = GetMesaExamen(dr);

//            return actaExamen;
//        }
//        private static ActaExamenDetalle GetActaExamenDetalle(NpgsqlDataReader dr)
//        {
//            ActaExamenDetalle actaDetalle = GetActaDetalle<ActaExamenDetalle>(dr);

//            actaDetalle.ID = dr.Get<int>(AuxSgaActas.id_acta);

//            actaDetalle.TipoInscripcion = dr.Get<short>(AuxSgaActas.instancia);

//            return actaDetalle;
//        }
//        private static T GetActaDetalle<T>(NpgsqlDataReader dr) where T : IActaDetalle, new()
//        {
//            T detalle = new()
//            {
//                FechaDetalle = dr.Get<DateTime>(AuxSgaActas.fecha),

//                Nota = dr.Get<string>(AuxSgaActas.nota),

//                Resultado = dr.Get<string>(AuxSgaActas.resultado),

//                Renglon = dr.Get<short>(AuxSgaActas.renglon),

//                Alumno = GetAlumno(dr),

//                Folio = GetFolio(dr)
//            };

//            return detalle;
//        }
//        private static PropuestaElementos GetPropuestaElementos(NpgsqlDataReader dr)
//        {
//            PropuestaElementos prop = new()
//            {
//                ID = dr.Get<int>(VwElementosPlan.propuesta),

//                Codigo = dr.Get<string>(VwElementosPlan.propuesta_codigo),

//                Plan = GetElementoPlan(dr),

//                Elementos = new List<Elemento>()
//            };

//            return prop;
//        }
//        private static ActaCursada<T> GetActaCursada<T>(NpgsqlDataReader dr) where T : IActaDetalle
//        {
//            ActaCursada<T> actaRegulares = GetActa<ActaCursada<T>>(dr);

//            actaRegulares.Comision = GetComisionActaRegularesPromocion(dr);

//            actaRegulares.PeriodoLectivo = GetPeriodoLectivoActaCursada(dr);

//            return actaRegulares;
//        }
//        private static IActaDetalle GetActaRegularesDetalle(NpgsqlDataReader dr)
//        {
//            ActaRegularesDetalle actaDetalle = GetActaDetalle<ActaRegularesDetalle>(dr);

//            actaDetalle.Asistencia = dr.Get<decimal>(AuxSgaActas.pct_asistencia);

//            actaDetalle.FechaVigencia = dr.Get<DateTime>(AuxSgaActas.fecha_vigencia);

//            actaDetalle.CondRegularidad = dr.Get<int>(AuxSgaActas.cond_regularidad);

//            return actaDetalle;
//        }
//        private static IActaDetalle GetActaPromocionDetalle(NpgsqlDataReader dr)
//        {
//            ActaRegularPromocionDetalle actaDetalle = GetActaDetalle<ActaRegularPromocionDetalle>(dr);
//            return actaDetalle;
//        }

//        private static async ValueTask<ConcurrentDictionary<object, PropuestaElementos>>
//            GetPropuestasElementosAsync<T>(NpgsqlDataReader dr, T keyType) where T : Enum
//        {
//            ConcurrentDictionary<object, PropuestaElementos> propuestas = new();

//            while (await dr.ReadAsync())
//            {
//                if (!dr.HasRows) continue;

//                object key = dr.Get<object>(keyType);

//                if (!propuestas.TryGetValue(key, out PropuestaElementos propuesta))
//                {
//                    propuesta = GetPropuestaElementos(dr);

//                    propuesta.Elementos.Add(GetElemento(dr));

//                    propuestas.TryAdd(key, propuesta);
//                }
//                else
//                {
//                    propuesta.Elementos.Add(GetElemento(dr));
//                }
//            }

//            return propuestas;
//        }

//        private static async ValueTask<ConcurrentDictionary<object, ActaCursada<T>>>
//            GetActasCursada<T>(NpgsqlDataReader dr) where T : IActaDetalle
//        {
//            ConcurrentDictionary<object, ActaCursada<T>> actas = new();

//            while (await dr.ReadAsync())
//            {
//                if (!dr.HasRows) continue;

//                string key = DbModelKeyBuilder.BuildKey<ActaCursada<T>>(dr);

//                if (!actas.TryGetValue(key, out ActaCursada<T> acta))
//                {
//                    acta = GetActaCursada<T>(dr);
//                    { }
//                    if (typeof(T) == typeof(ActaRegularesDetalle))
//                        acta.Detalles.Add((T)GetActaRegularesDetalle(dr));
//                    else
//                        acta.Detalles.Add((T)GetActaPromocionDetalle(dr));

//                    actas.TryAdd(key, acta);
//                }
//                else
//                {
//                    if (typeof(T) == typeof(ActaRegularesDetalle))
//                        acta.Detalles.Add((T)GetActaRegularesDetalle(dr));
//                    else
//                        acta.Detalles.Add((T)GetActaPromocionDetalle(dr));
//                }
//            }

//            return actas;
//        }

//        private static async ValueTask<ConcurrentDictionary<object, ActaExamen>>
//            GetActasExamenes(NpgsqlDataReader dr)
//        {
//            ConcurrentDictionary<object, ActaExamen> actas = new();

//            while (await dr.ReadAsync())
//            {
//                if (!dr.HasRows) continue;

//                string key = DbModelKeyBuilder.BuildKey<ActaExamen>(dr);

//                if (!actas.TryGetValue(key, out ActaExamen acta))
//                {
//                    acta = GetActaExamen(dr);

//                    acta.Detalles.Add(GetActaExamenDetalle(dr));

//                    actas.TryAdd(key, acta);
//                }
//                else
//                {
//                    acta.Detalles.Add(GetActaExamenDetalle(dr));
//                }
//            }

//            return actas;
//        }
//    }
//}
