namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class EscalaNota : DbModel
    {
        public string Numerica { get; set; }
        public short CantidadDecimales { get; set; }
        public string SeparadorDecimal { get; set; }
        public decimal NotaInicial { get; set; }
        public decimal NotaFinal { get; set; }
        public string Estado { get; set; }
        public string Resultado { get; set; }
    }
}
