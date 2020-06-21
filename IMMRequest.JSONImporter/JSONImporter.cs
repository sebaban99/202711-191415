using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Importer;
using IMMRequest.Exceptions;
using System.IO;
using Newtonsoft.Json;

namespace IMMRequest.JSONImporter
{
    public class JSONImporter : IImporter
    {
        public string ImporterName { get; set; }
        public List<ImportationField> RequiredFields { get; set; }

        public JSONImporter()
        {
            ImporterName = "JSON Importer";
            RequiredFields = new List<ImportationField>();
            ImportationField impField = new ImportationField()
            {
                NameOfField = "Path to file to be imported:",
                FieldType = "string"
            };
            RequiredFields.Add(impField);
        }

        public List<AreaImpModel> ImportFile(string path)
        {
            List<AreaImpModel> importedArea = new List<AreaImpModel>();
            ValidatePath(path);
            try
            {
                string json = "";
                StreamReader reader = new StreamReader(path);
                json = reader.ReadToEnd();
                importedArea = JsonConvert.DeserializeObject<List<AreaImpModel>>(json);
            }
            catch (Exception)
            {
                throw new ImportException("Error on Import: JSON file was corrupt or invalid");
            }
            return importedArea;
        }

        private static void ValidatePath(string path)
        {
            if (!path.EndsWith(".json"))
            {
                throw new ImportException("Error on import: File chosen was not .json");
            }
        }
    }
}
