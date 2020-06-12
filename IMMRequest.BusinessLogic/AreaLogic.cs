using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class AreaLogic : IAreaLogic
    {
        private IRepository<Area> areaRespository;
        private IAreaValidatorHelper areaValidator;

        public AreaLogic(IRepository<Area> areaRespository)
        {
            this.areaRespository = areaRespository;
            areaValidator = new AreaValidatorHelper(areaRespository);
        }

        public Area Create(Area area)
        {
            areaValidator.ValidateAdd(area);
            area.Id = Guid.NewGuid();
            areaRespository.Add(area);
            areaRespository.SaveChanges();
            return area;
        }
    }
}
