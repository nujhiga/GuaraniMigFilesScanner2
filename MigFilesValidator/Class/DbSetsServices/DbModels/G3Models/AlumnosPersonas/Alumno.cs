using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class Alumno : DbModel
    {
        public Persona Persona { get; set; }
        public Propuesta Propuesta { get; set; }
        public int Ubicacion { get; set; }
        public class AlumnoComparer : IEqualityComparer<Alumno>
        {
            public bool Equals([AllowNull] Alumno x, [AllowNull] Alumno y)
            {
                if (x.Persona == null || y.Persona == null) return false;

                if (!x.Persona.Equals(y.Persona)) return false;

                if (x.Propuesta == null || y.Propuesta == null) return false;

                if (x.Propuesta.Plan == null || y.Propuesta.Plan == null) return false;

                //if (x.Propuesta.Plan.Version != y.Propuesta.Plan.Version) return eq;

                if (x.Propuesta.Plan.VersionActual != y.Propuesta.Plan.VersionActual) return false;

                return true;
            }

            public int GetHashCode([DisallowNull] Alumno obj)
            {
                return obj.GetHashCode();
            }
        }

        public override string ToString()
        {
            string baseStr = base.ToString();

            return $"{baseStr} Per. {Persona}-Prop. {Propuesta}";
        }
    }
}
