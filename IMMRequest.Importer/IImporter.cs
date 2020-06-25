using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.Importer
{
    public interface IImporter
    {
        List<AreaImpModel> ImportFile(ImportInfoDTO path);
        string ImporterName { get; set; }
        List<ImportationField> RequiredFields { get; set; }
    }
}
