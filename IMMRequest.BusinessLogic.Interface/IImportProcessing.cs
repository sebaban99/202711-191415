using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface IImportProcessing
    {
        void ProcessImportedElements(List<Area> elementsToImport);
    }
}
