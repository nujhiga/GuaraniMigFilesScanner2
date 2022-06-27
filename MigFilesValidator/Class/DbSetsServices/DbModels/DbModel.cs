using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels
{
    public class DbModel
    {
        public int ID { get; set; }

        public readonly string Name;


        public DbModel()
        {
            Name = GetType().Name;
        }
        public override string ToString()
        {
            return $"[obj: {Name} id: {ID}]";
        }
    }
    
    public class DbModelComparer : IEqualityComparer<DbModel>
    {
        public bool Equals([AllowNull] DbModel x, [AllowNull] DbModel y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode([DisallowNull] DbModel obj)
        {
            return obj.GetHashCode();
        }
    }

    
}
