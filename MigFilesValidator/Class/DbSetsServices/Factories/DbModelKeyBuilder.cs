using Npgsql;

using PasifaeG3Migrations.Class.DbSetsServices.DbModels;
using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
using PasifaeG3Migrations.Class.DbSetsServices.G3Structures.AuxiliarStructs;
using PasifaeG3Migrations.Class.Extras;

using System;
using System.Text;

namespace PasifaeG3Migrations.Class.DbSetsServices.Factories
{
    public static class DbModelKeyBuilder
    {
        public static string BuildKey<TKeys>(NpgsqlDataReader dr, params TKeys[] keys) where TKeys : Enum
        {
            StringBuilder sb = new();

            foreach (var k in keys)
            {
                object aux = dr.Get<object>(k);

                if (aux is DateTime date)
                {
                    sb.Append($"{date:dd/MM/yyyy}-");
                }
                else
                {
                    sb.Append($"{aux}-");
                }
            }

            return sb.ToString();
        }
        public static string BuildKey<T>(NpgsqlDataReader dr) where T : DbModel
        {
            string key = string.Empty;

            if (typeof(T).FullName.Contains(nameof(ActaRegularesDetalle)) ||
                typeof(T).FullName.Contains(nameof(ActaRegularPromocionDetalle)))
            {
                return BuildActaCursadaKey(dr);
            }

            switch (typeof(T).Name)
            {
                case nameof(ActaExamen):
                    key = BuildActaExamenKey(dr);
                    break;
            }

            return key;
        }

        private static string BuildActaExamenKey(NpgsqlDataReader dr)
        {
            GenericKeyFields[] keys = { GenericKeyFields.turno_examen_nombre, GenericKeyFields.anio_academico,
                                        GenericKeyFields.nombre, GenericKeyFields.codigo, GenericKeyFields.fecha };
            StringBuilder sb = new();

            foreach (var k in keys)
            {
                object aux = dr.Get<object>(k);

                if (aux is DateTime date)
                {
                    sb.Append($"{date:dd/MM/yyyy}-");
                }
                else
                {
                    sb.Append($"{aux}-");
                }
            }

            return sb.ToString();
        }

        //anio_academico + periodo_lectivo_nombre + comision_nombre + actividad_codigo.
        private static string BuildActaCursadaKey(NpgsqlDataReader dr)
        {
            GenericKeyFields[] keys = { GenericKeyFields.anio_academico,
                                        GenericKeyFields.pNombre, GenericKeyFields.cNombre,
                                        GenericKeyFields.codigo };
            return BuildKey(dr, keys);
        }

    }
}
