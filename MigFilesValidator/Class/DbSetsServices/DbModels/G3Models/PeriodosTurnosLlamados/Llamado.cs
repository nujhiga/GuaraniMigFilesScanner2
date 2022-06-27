namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class Llamado : DbModel
    {
        public Periodo Periodo { get; set; }
        public TurnoExamen TurnoExamen { get; set; }
    }
}
