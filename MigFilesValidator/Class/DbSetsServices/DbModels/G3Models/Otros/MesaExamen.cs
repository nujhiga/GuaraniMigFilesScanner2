namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class MesaExamen : DbModel
    {
        //public string Nombre { get; set; }
        //public Elemento Elemento { get; set; }
        //public int Ubicacion { get; set; }
        //public AnioAcademico Anio { get; set; }

        public string Nombre { get; set; }
        public Elemento Elemento { get; set; }
        public TurnoExamen Turno { get; set; }
        public int Ubicacion { get; set; }
        public AnioAcademico Anio { get; set; }
    }
}
