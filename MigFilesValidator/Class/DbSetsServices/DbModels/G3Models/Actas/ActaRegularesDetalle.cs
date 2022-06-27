using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class ActaRegularesDetalle : ActaRegularPromocionDetalle
    {
        public decimal Asistencia { get; set; }
        public DateTime FechaVigencia { get; set; }
        public int CondRegularidad { get; set; }
    }
}
