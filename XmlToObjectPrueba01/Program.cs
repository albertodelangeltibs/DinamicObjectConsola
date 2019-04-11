using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Reflection;
using XmlToObjectPrueba01.ServiceReference1;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;

namespace XmlToObjectPrueba01
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize custom class for xml serialization
            Serializer ser = new Serializer();
            string path = string.Empty;
            string xmlInputData = string.Empty;
            string xmlOutputData = string.Empty;

            // Declare the path for xml file
            path = Directory.GetCurrentDirectory() + @"\Salida.xml";
            xmlInputData = File.ReadAllText(path);

            // Initialize the Class for match xml node with name of class
            Facturas factura = ser.Deserialize<Facturas>(xmlInputData);

            // Initialize listaFactura Object
            List<PropiedadesFactura> listaFact = new List<PropiedadesFactura>();

            // Fill the object with values form xml 
            for (int i = 0; i < factura.Propiedades.Count; i++)
            {
                listaFact.Add(factura.Propiedades[i]);
            }

            // Object's list from facturas
            IEnumerable<PropiedadesFactura> filter1 = listaFact.GroupBy(x => x.FolioABA)
                .Select(y => y.First()).Where(element => element.FolioABA != "0");

            // Object's list from Concepto
            //List<XmlToObjectPrueba01.ServiceReference1.Concepto> IdConceptos =
            //    new List<XmlToObjectPrueba01.ServiceReference1.Concepto>();

            //Object's list from Concepto
            //List<ConceptoClassWCF> IdConceptos =
            //    new List<ConceptoClassWCF>();
            List<object> IdConceptos = new List<object>();
            //const string objectToInstantiate2 =
            //            "XmlToObjectPrueba01.ServiceReference1.Concepto, XmlToObjectPrueba01";
            //var ConceptoClass2 = Type.GetType(objectToInstantiate2);
            //var ConceptoClassWCF = Activator.CreateInstance(ConceptoClass2);

            //List<ConceptoClass2> c = new List<ConceptoClass2>();

            #region Iteration 1 - Facturas
            foreach (PropiedadesFactura facturas in filter1)
            {

                // Object's list from IdGeneral
                IEnumerable<PropiedadesFactura> IGIL = listaFact.GroupBy(e => e.IdGeneral)
                .Select(y => y.First()).Where(e => e.FolioABA == facturas.FolioABA);

                #region Iteration 2 - IdGeneral
                foreach(PropiedadesFactura IdGenerales in IGIL)
                {

                // Object's list from IdGeneral
                IEnumerable<PropiedadesFactura> filter2 = listaFact.Select(
                   element => element).Where(
                   element => element.IdGeneral == IdGenerales.IdGeneral && element.FolioABA == facturas.FolioABA);

                // Object's list from ValorUnitario - Class Name
                IEnumerable<PropiedadesFactura> CNL = listaFact.Select(
                   element => element).Where(
                   element => element.IdGeneral == 
                       IdGenerales.IdGeneral && element.FolioABA == facturas.FolioABA);

                // Add Filter for value unique in the class name for every ClassName

                var ClasesName = new Dictionary<String, object>();

                // Fill the Dictionary with data - Key = Valor && Value = Dato
                foreach (var Datos in CNL)
                {
                    // Regular expresion by get all characters after dot
                    Regex regex = new Regex(@"^\w*");
                    // Get data for Regular expression
                    Match match = regex.Match(Datos.Valor);
                    // Add Keys and Values
                    ClasesName.Add(match.ToString(), Datos.Dato);
                }

                    //// Object's list from IdGeneral
                    //IEnumerable<PropiedadesFactura> filter2 = listaFact.Select(
                    //   element => element).Where(
                    //   element => element.IdGeneral == 4 && element.FolioABA == facturas.FolioABA);

                // Object's list from IdConcepto
                IEnumerable<PropiedadesFactura> filter3 = filter2.GroupBy(x => x.IdConcepto)
                    .Select(y => y.First()).Distinct();
                #region Iteration 2 - IdConcepto inside each Factura
                foreach (PropiedadesFactura createObject in filter3)
                {
                    
                    // Object's list from each IdConcepto inside IdGeneral
                    IEnumerable<PropiedadesFactura> objectCreated = listaFact
                        .Where(e => e.FolioABA == facturas.FolioABA &&
                        e.IdConcepto == createObject.IdConcepto);

                    // Initialize class Concepto from ServicesReference
                    //XmlToObjectPrueba01.ServiceReference1.Concepto ConceptoClassWCF =
                    //new XmlToObjectPrueba01.ServiceReference1.Concepto();
                    const string objectToInstantiate = 
                        "XmlToObjectPrueba01.ServiceReference1." +"Concepto" + ","+ " XmlToObjectPrueba01";
                    var ConceptoClass = Type.GetType(objectToInstantiate);
                    var ConceptoClassWCF = Activator.CreateInstance(ConceptoClass);
                    
                    //ConceptoClass = Type class;

                    // Initialize Dictoriony by object's list from data - Concepto
                    var dictionary = new Dictionary<String, object>();
                    
                    // Fill the Dictionary with data - Key = Valor && Value = Dato
                    foreach (var Datos in objectCreated)
                    {
                        // Regular expresion by get all characters after dot
                        Regex regex = new Regex(@"\w*$");
                        // Get data for Regular expression
                        Match match = regex.Match(Datos.Valor);
                        // Add Keys and Values
                        dictionary.Add(match.ToString(),Datos.Dato);
                    }

                    // Get array properties from Class Concepto - Service Reference
                    PropertyInfo[] properties = typeof(XmlToObjectPrueba01.ServiceReference1.Concepto)
                        .GetProperties();
                    
                    #region Iteration 3 - Set values to Concepto - Services Reference
                    foreach (PropertyInfo proper in properties)
                    {
                        // Filters for properties from object Conceptos - Service Reference
                        if (proper.PropertyType == typeof(String))
                        {
                            if (dictionary.ContainsKey(proper.Name.ToString()))
                            {
                                proper.SetValue(ConceptoClassWCF,
                                dictionary[proper.Name.ToString()]);
                            }
                        }
                        else if (proper.PropertyType == typeof(Decimal))
                        {
                            if (dictionary.ContainsKey(proper.Name.ToString()))
                            {
                                proper.SetValue(ConceptoClassWCF,
                                Convert.ToDecimal(dictionary[proper.Name.ToString()]));
                            }
                        }
                        else if (proper.PropertyType == typeof(int))
                        {
                            if (dictionary.ContainsKey(proper.Name.ToString()))
                            {
                                proper.SetValue(ConceptoClassWCF,
                                Convert.ToInt32(dictionary[proper.Name.ToString()]));
                            }
                        }
                    }
                    #endregion
                    // Add object Concepto to List<Concepto>
                    IdConceptos.Add(ConceptoClassWCF);
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            Console.ReadLine();
        }
    }
}
