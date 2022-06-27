using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class AnioAcademico : DbModel
    {
        public decimal Anio { get; set; }

    }
    public class AnioAcademicoComparer : IEqualityComparer<AnioAcademico>
    {
        public bool Equals([AllowNull] AnioAcademico x, [AllowNull] AnioAcademico y)
        {
            return x.Anio == y.Anio;
        }

        public int GetHashCode([DisallowNull] AnioAcademico obj)
        {
            return obj.GetHashCode();
        }
    }
}
