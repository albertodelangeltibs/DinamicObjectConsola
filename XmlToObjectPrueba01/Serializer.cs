using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace XmlToObjectPrueba01
{
    class Serializer
    {
        // Convert xml to Object
        // Param name = XmlToDeserialize
        public T Deserialize<T>(string XmlToDeserialize) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(XmlToDeserialize))
            {
                return (T)ser.Deserialize(sr);
            }
        }


        // Convert object to xml string
        // param name = ObjectToSerialize
        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWrite = new StringWriter())
            {
                xmlSerializer.Serialize(textWrite, ObjectToSerialize);
                return textWrite.ToString();

            }
        }
    }
}
