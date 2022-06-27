using System.Collections.Generic;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class ActaExamen : Acta
    {
        public MesaExamen MesaExamen { get; set; }
        public TurnoExamen TurnoExamen { get; set; }
        public List<ActaExamenDetalle> Detalles { get; set; }
        public ActaExamen()
        {
            Detalles = new List<ActaExamenDetalle>();
        }
    }
}
