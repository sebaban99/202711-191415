using IMMRequest.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface ITopicLogic
    {
        Topic Create(Topic topic);
    }
}
