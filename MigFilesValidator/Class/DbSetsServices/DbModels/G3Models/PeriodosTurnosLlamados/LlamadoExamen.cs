using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class LlamadoExamen : DbModel
    {
        public Periodo Periodo { get; set; }
        public string Nombre { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
    }
}
