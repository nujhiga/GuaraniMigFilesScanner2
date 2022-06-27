using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class TurnoExamen : DbModel
    {
        public int Tipo { get; set; }
        public Periodo Periodo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaExamenInicio { get; set; }
        public DateTime FechaExamenFin { get; set; }
        public DateTime PublicacionMesas { get; set; }
        public DateTime FechaInactivacion { get; set; }

    }
}


