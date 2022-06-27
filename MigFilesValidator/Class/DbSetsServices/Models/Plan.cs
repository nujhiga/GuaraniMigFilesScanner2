namespace GuaraniMigFilesScanner.Class.DbSetsServices.Models
{
    public class Plan
    {
        public int ID { get; set; }
        public int Version { get; set; }
        public string Codigo { get; set; }
        public char Estado { get; set; }
        public int VersionActual { get; set; }
        public string VersionCodigo { get; set; }
        public char VersionEstado { get; set; }
    }
}
