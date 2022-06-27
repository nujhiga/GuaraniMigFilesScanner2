namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class Comision : DbModel
    {
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string InscripcionHab { get; set; }
        public string InscripcionCerrada { get; set; }
        public int Turno { get; set; }
        public int Ubicacion { get; set; }
        public AnioAcademico Anio { get; set; }
        public PeriodoLectivo Periodo { get; set; }
        public Elemento Elemento { get; set; }

    }
}
