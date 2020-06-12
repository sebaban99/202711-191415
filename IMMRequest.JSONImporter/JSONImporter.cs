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

        public JSONImporter()
        {
            ImporterName = "JSON Importer";
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
                JsonConverter[] converters = { new CustomJSONConverter() };
                importedArea = JsonConvert.DeserializeObject<List<AreaImpModel>>(json, new JsonSerializerSettings() { Converters = converters });
            }
            catch (Exception)
            {
                throw new ImportException("Error on Area import: JSON file was corrput or invalid");
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
