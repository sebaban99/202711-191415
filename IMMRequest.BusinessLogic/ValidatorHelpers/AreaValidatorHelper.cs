using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Exceptions;

namespace IMMRequest.BusinessLogic
{
    public class AreaValidatorHelper : IAreaValidatorHelper
    {
        private IRepository<Area> areaRepository;

        public AreaValidatorHelper(IRepository<Area> areaRepository)
        {
            this.areaRepository = areaRepository;
        }


        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z\s]{1,}$");
        }

        public bool AreEmptyFields(Area area)
        {
            return !IsValidName(area.Name) || area.Topics != null;
        }

        public void ValidateEntityObject(Area type)
        {
            if (AreEmptyFields(type))
            {
                throw new BusinessLogicException("Error: Area had empty fields");
            }
        }

        private bool IsAreaRegistered(Area area)
        {
            Area areaInDB = areaRepository.GetByCondition(t => t.Name == area.Name);
            return areaInDB != null;
        }

        private void ValidateArea(Area area)
        {
            if (IsAreaRegistered(area))
            {
                throw new BusinessLogicException("Error: Area with same name already registered");
            }
        }

        public void ValidateAdd(Area area)
        {
            ValidateEntityObject(area);
            ValidateArea(area);
        }
    }
}
