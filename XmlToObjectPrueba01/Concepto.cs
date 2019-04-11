using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlToObjectPrueba01
{
     public class Concepto
    {
        
        public string ClaveProdServ { get; set; }
        public string Cantidad { get; set; } // decimal
        public string ClaveUnidad { get; set; }
        public string Descripcion { get; set; }
        public string ValorUnitario { get; set; } // decimal
        public string Importe { get; set; }// decimal
        public string Descuento { get; set; }// decimal
        public string ComplementoConcepto { get; set; }
        public string NoIdentificacion { get; set; }
        
        //public string CuentaPredial { get; set; }
        
        
        //public string ExtensionData { get; set; }
        
        //public string Impuestos { get; set; }
       
        
        //public string PropertyChanged { get; set; }
        //public string Unidad { get; set; }
        

       
    }
}
