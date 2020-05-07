using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public interface IAdminValidatorHelper
    {
        void ValidateAdd(Admin admin);
        void ValidateDelete(Admin admin);
        void ValidateUpdate(Admin admin);
        void ValidateAdminObject(Admin admin);
    }
}
