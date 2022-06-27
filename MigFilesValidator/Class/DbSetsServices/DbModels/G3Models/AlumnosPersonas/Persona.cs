using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class Persona : DbModel
    {
        public int TipoDocumento { get; set; }
        public string NroDocumento { get; set; }

        public bool Equals(Persona p2)
        {
            return new PersonaComparer().Equals(this, p2);
        }

        public override string ToString()
        {
            string baseStr = base.ToString();

            return $"{baseStr} {TipoDocumento}-{NroDocumento}";
        }
    }

    public class PersonaComparer : IEqualityComparer<Persona>
    {
        public bool Equals([AllowNull] Persona x, [AllowNull] Persona y)
        {
            return x.TipoDocumento == y.TipoDocumento && x.NroDocumento.Equals(y.NroDocumento, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode([DisallowNull] Persona obj)
        {
            return obj.GetHashCode();
        }
    }
}
