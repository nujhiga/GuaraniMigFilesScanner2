using PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class Propuesta : DbModel, ICodeStateModel
    {
        public string Codigo { get; set; }
        public string Estado { get; set; }
        public Plan Plan { get; set; }

        public override string ToString()
        {
            string baseStr = base.ToString();

            return $"{baseStr} Cod. {Codigo}-Est. {Estado} {Plan}";
        }
    }
}
