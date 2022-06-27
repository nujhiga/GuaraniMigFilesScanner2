using System;

namespace GuaraniMigFilesScanner.Class.DbSetsServices.Models
{
    public class Periodo
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int AioAcademico { get; set; }
        DateTime? Inicio { get; set; }
        DateTime? Fin { get; set; }
    }
}
