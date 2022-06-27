using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class LlamadoMesa : DbModel
    {
        public Llamado Llamado { get; set; }
        public MesaExamen MesaExamen { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Estado { get; set; }

    }
}
