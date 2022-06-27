using PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class Elemento : DbModel, ICodeStateModel
    {
        public string Codigo { get; set; }
        public string Estado { get; set; }
    }
}
