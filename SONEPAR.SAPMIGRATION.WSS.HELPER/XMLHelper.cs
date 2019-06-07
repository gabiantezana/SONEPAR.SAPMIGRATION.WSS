using SONEPAR.SAPMIGRATION.WSS.HELPER;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YAMBOLY.GESTIONRUTAS.HELPER
{
    public class XMLHelper
    {
        public static string GetXMLString(EmbebbedFileName xmlFile, Assembly assembly)
        {
            var resourceFullName = assembly.GetManifestResourceNames().ToList().FirstOrDefault(x => x.Split('.')?[x.Split('.').Count() - 2] == xmlFile.ToString());
            if (string.IsNullOrEmpty(resourceFullName))
                resourceFullName = assembly.GetManifestResourceNames().ToList().OrderBy(x => x).FirstOrDefault(x => x.Contains(xmlFile.ToString()));
            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found: " + xmlFile.ToString());

            using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetXMLString(Assembly assembly, EmbebbedFileName xmlFile)
        {
            var resourceFullName = assembly.GetManifestResourceNames().ToList().FirstOrDefault(x => x.Split('.')?[x.Split('.').Count() - 2] == xmlFile.ToString());
            if (string.IsNullOrEmpty(resourceFullName))
                resourceFullName = assembly.GetManifestResourceNames().ToList().OrderBy(x => x).FirstOrDefault(x => x.Contains(xmlFile.ToString()));
            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found: " + xmlFile.ToString());

            using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
