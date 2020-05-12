using System;
using System.Linq;
using IMMRequest.DataAccess;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public interface ISessionLogic
    {
        bool ValidateLogin(string email, string password);

        bool ValidateSession(Guid token);

        void GenerateSession(Admin loggedAdmin);
        
    }
}
