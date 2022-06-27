using PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces;
using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class ActaExamenDetalle : DbModel, IActaDetalle
    {
        public Folio Folio { get; set; }
        public DateTime FechaDetalle { get; set; }
        public string Nota { get; set; }
        public string Resultado { get; set; }
        public Alumno Alumno { get; set; }
        public short TipoInscripcion { get; set; }
        public short Renglon { get; set; }
    }
}
