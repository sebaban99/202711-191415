using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public interface IAdminLogic : ILogic<Admin>
    {
        void Remove(Admin admin);

        Admin Update(Admin admin);
    }
}
