using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Exceptions;
using IMMRequest.Importer;

namespace IMMRequest.BusinessLogic
{
    public class ImportationLogic : IImportationLogic
    {
        List<IImporter> importers { get; set; }

        public ImportationLogic()
        {
            importers = new List<IImporter>();
        }

        public List<IImporter> GetImportationsMethods(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ImportException("Error reading importing file, path was null or empty");
            }
            try
            {
                var pathCurrent = Environment.CurrentDirectory;

                var importersPath = Path.Combine(Environment.CurrentDirectory,path);

                List<string> files = Directory.GetFiles(importersPath, "*.*", SearchOption.AllDirectories)
                  .Where(file => new string[] { ".dll" }
                  .Contains(Path.GetExtension(file)))
                  .ToList();

                foreach (string filePath in files)
                {
                    Assembly myAssembly = Assembly.LoadFile(filePath);
                    foreach (System.Type typeOfFile in myAssembly.GetTypes())
                    {
                        if (typeof(IImporter).IsAssignableFrom(typeOfFile))
                        {
                            IImporter importer = (IImporter)Activator.CreateInstance(typeOfFile);
                            importers.Add(importer);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ImportException("Error importing file, check file or try with different one");
            }
            return importers;
        }

        public IImporter GetImporter(string name)
        {
            foreach (IImporter importer in importers)
            {
                if (importer.ImporterName == name)
                {
                    return importer;
                }
            }

            throw new ImportException("Importation method requested is not supported");
        }

        public List<AreaImpModel> ImportFile(IImporter importerSelected, string path)
        {
            return importerSelected.ImportFile(path);
        }
    }
}
