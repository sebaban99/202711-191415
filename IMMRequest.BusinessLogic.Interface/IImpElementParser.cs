using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Domain;
using IMMRequest.Importer;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface IImpElementParser
    {
        List<Area> ParseElements(List<AreaImpModel> areaImpModel);
    }
}
