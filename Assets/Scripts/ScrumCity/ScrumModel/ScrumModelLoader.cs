using System;
using System.IO;
using System.Xml.Serialization;

namespace ScrumCity
{
    public class ScrumModelLoader
    {

        public static Release LoadReleaseFromXML(string scrumXmlFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Release));
            Release release;

            using(Stream reader = new FileStream(scrumXmlFile, FileMode.Open))
            {
                release = (Release)ser.Deserialize(reader);
            }

            return release;
        }
    }
}
