using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Importer;
using IMMRequest.Exceptions;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace IMMRequest.XMLImporter
{
    public class XMLImporter : IImporter
    {
        public string ImporterName { get; set; }
        public List<ImportationField> RequiredFields { get; set; }

        public XMLImporter()
        {
            ImporterName = "XML Importer";
            RequiredFields = new List<ImportationField>();
            ImportationField impField = new ImportationField()
            {
                NameOfField = "Path to file to be imported:",
                FieldType = "string",
                FieldValue = ""
            };
            RequiredFields.Add(impField);
        }

        public List<AreaImpModel> ImportFile(ImportInfoDTO importInfo)
        {
            if(importInfo.requiredFields == null || importInfo.requiredFields.Count != 1)
            {
                throw new ImportException("Error on Importation: Received information is not compatible with required by the importer");
            }
            string path = importInfo.requiredFields[0].FieldValue;
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ImportException("Error on Importation: Path to file was missing or corrupted");
            }
            List<AreaImpModel> importedArea = new List<AreaImpModel>();
            ValidatePath(path);
            try
            {
                string xml = "";
                StreamReader reader = new StreamReader(path);
                xml = reader.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                importedArea = JsonConvert.DeserializeObject<List<AreaImpModel>>(JsonConvert.SerializeXmlNode(doc));
            }
            catch (Exception)
            {
                throw new ImportException("Error on Import: XML file was corrupt or invalid");
            }
            return importedArea;
        }

        private static void ValidatePath(string path)
        {
            if (!path.EndsWith(".xml"))
            {
                throw new ImportException("Error on import: File chosen was not .xml");
            }
        }
    }
}
