using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToObjectPrueba01
{
    public partial class PropiedadesFactura
    {
        public int Id { get; set; }

        public int IdGeneral { get; set; }

        public int IdConcepto { get; set; }

        public int IdOrden { get; set; }

        public string Valor { get; set; }

        public string Dato { get; set; }

        public string RCC { get; set; }

        public string FolioABA { get; set; }

        public string Addenda { get; set; }

    }
}
