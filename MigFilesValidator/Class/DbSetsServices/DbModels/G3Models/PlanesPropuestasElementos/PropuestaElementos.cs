using System.Collections.Generic;

namespace PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models
{
    public class PropuestaElementos : Propuesta
    {
        public List<Elemento> Elementos { get; set; }
    }
}
