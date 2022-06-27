using System;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class Acta : DbModel  
    {
        public string NroActa { get; set; }
        public LibroActas Libro { get; set; }
        public int RenglonesFolio { get; set; }
        public Elemento Elemento { get; set; }
        public EscalaNota EscalaNota { get; set; }
        public AnioAcademico AnioAcademico { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }

    }
}
