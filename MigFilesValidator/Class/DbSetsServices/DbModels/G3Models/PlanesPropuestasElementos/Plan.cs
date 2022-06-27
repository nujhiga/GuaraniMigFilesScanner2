using PasifaeG3Migrations.Class.DbSetsServices.DbModels.Interfaces;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels
{
    public class Plan : DbModel, ICodeStateModel
    {
        public string Codigo { get; set; }
        public string Estado { get; set; }
        public int Version { get; set; }
        public int VersionActual { get; set; }
        public string VersionCodigo { get; set; }
        public string VersionEstado { get; set; }

        public override string ToString()
        {
            string baseStr = base.ToString();

            return $"{baseStr} Ver. {Version}-Cod. {Codigo}-Est. {Estado}-VerAct. {VersionActual}-VerActCod. {VersionCodigo}-VerActEst. {VersionEstado}";
        }
    }
}
