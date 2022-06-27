using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class LibroActas : DbModel
    {
        public string NroLibro { get; set; }
        public int NroTomo { get; set; }
        public string Nombre { get; set; }
        public string Activo { get; set; }
        public DateTime FechaActivacion { get; set; }
        public DateTime FechaFinVigencia { get; set; }
        public AnioAcademico AnioAcademico { get; set; }
    }
}
