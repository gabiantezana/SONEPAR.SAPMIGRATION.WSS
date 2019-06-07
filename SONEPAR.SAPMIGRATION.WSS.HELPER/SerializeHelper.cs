using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public static class SerializeHelper
    {
        public static dynamic XMLToObject(string xml, Type objectType, Boolean validateAttr = false)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            if (String.IsNullOrEmpty(xml))
                throw new Exception("XML passed as parameter is null.");
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
                if (strReader != null)
                    strReader.Close();
            }

            //Validar atributos
            if (validateAttr)
                new ValidationHelper().Validate(obj);
            return obj;

            //Convertir atributos default
            //TODOG:
        }

        public static string ToXML(ApplicationResponse response)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(response.GetType());
            serializer.Serialize(stringwriter, response);
            return stringwriter.ToString();
        }

    }
}
