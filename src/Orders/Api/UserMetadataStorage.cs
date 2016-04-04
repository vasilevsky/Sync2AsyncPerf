using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace API
{
    public class UserMetadataStorage
    {
        private const string metadataPath = "metadata\\";

        public static async Task PersistAsync(CustomerData data)
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(metadataPath))
                {
                    Directory.CreateDirectory(metadataPath);
                }
            });

            string fileName = metadataPath + Guid.NewGuid() + ".xml";
            using (var xml = XmlWriter.Create(fileName, new XmlWriterSettings() { Async = true }))
            {
                await xml.WriteStartElementAsync("", "customer", "");
                await xml.WriteElementStringAsync("", "name", "", data.Name);
                await xml.WriteElementStringAsync("", "email", "", data.Email);
                await xml.WriteEndElementAsync();
            }

            await Task.Run(() => File.Delete(fileName));
        }

        public static void Persist(CustomerData data)
        {
            if (!Directory.Exists(metadataPath))
            {
                Directory.CreateDirectory(metadataPath);
            }

            string fileName = metadataPath + Guid.NewGuid() + ".xml";
            using (var xml = XmlWriter.Create(fileName))
            {
                xml.WriteStartElement("", "customer", "");
                xml.WriteElementString("", "name", "", data.Name);
                xml.WriteElementString("", "email", "", data.Email);
                xml.WriteEndElement();
            }

            File.Delete(fileName);
        }
    }
}