using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Domain;
using IMMRequest.Importer;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface IImportationLogic
    {
        List<IImporter> GetImportationsMethods(string path);
        List<AreaImpModel> ImportFile(IImporter importerSelected, ImportInfoDTO importInfo);
        IImporter GetImporter(string name);
    }
}
