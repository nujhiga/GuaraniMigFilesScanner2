using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class TurnosExamen : DbModel
    {
        public AnioAcademico Anio { get; set; }
        public Periodo Periodo { get; set; }
        public string Nombre { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        //public DateTime PublicacionMesas { get; set; }
        //public DateTime InactivacionMesas { get; set; }
        public LlamadoExamen Llamado { get; set; }
    }
}
