using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;
using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces
{
    public interface IActaDetalle
    {
        public Folio Folio { get; set; }
        public DateTime FechaDetalle { get; set; }
        public string Nota { get; set; }
        public string Resultado { get; set; }
        public Alumno Alumno { get; set; }
        public short Renglon { get; set; }
    }
}
