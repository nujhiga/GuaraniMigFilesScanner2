using PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces;
using System.Collections.Generic;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class ActaCursada<T> : Acta where T : IActaDetalle
    {
        public PeriodoLectivo PeriodoLectivo { get; set; }
        public Comision Comision { get; set; }
        public List<T> Detalles { get; set; }
        public ActaCursada()
        {
            Detalles = new List<T>();
        }
    }
}
