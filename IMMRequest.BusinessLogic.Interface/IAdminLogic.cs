using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface IAdminLogic : ILogic<Admin>
    {
        Admin GetByCondition(Expression<Func<Admin, bool>> expression);

        Admin Create(Admin admin);

        void Remove(Admin admin);

        Admin Update(Admin admin);

        IEnumerable<ReportTypeAElement> GenerateReportA(DateTime from, DateTime until, string email);

        IEnumerable<ReportTypeBElement> GenerateReportB(DateTime from, DateTime until);
    }
}
