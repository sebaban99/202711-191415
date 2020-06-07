using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface ITopicValidatorHelper
    {
        void ValidateAdd(Topic topic);
    }
}
