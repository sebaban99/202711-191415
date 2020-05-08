using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public interface IAdminLogic : ILogic<Admin>
    {
        Admin GetByCondition(Expression<Func<Admin, bool>> expression);

        Admin Create(Admin admin);

        void Remove(Admin admin);

        Admin Update(Admin admin);
    }
}
