using System.Collections.Generic;

namespace PasifaeG3Migrations.Class.DbSetsServices
{

    public class DbCatalogSet
    {
        public List<int> MdpTiposDocumentos { get; set; }
        public List<int> MugLocalidades { get; set; }
        public List<int> MugPaises { get; set; }

        public readonly char[] ActasResultados;

        public readonly char[] TiposInscripcion;

        public readonly char[] Alcances;

        public readonly char[] ActasEstados;

        public readonly char[] InstanciaRegular;

        public readonly char[] ActasCursadaEstados;

        public DbCatalogSet()
        {
            MdpTiposDocumentos = new List<int>();

            MugLocalidades = new List<int>();

            MugPaises = new List<int>();

            ActasResultados = new char[] { 'A', 'R', 'U' };

            TiposInscripcion = new char[] { '3', '4' };

            Alcances = new char[] { 'T', 'R', 'P' };

            ActasEstados = new char[] { 'A', 'C' };

            ActasCursadaEstados = new char[] { 'A', 'P' };

            InstanciaRegular = new char[] { 'S', 'N' };
        }

    }
}
