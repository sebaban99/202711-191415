using System;
using System.Linq;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic.Interfaces
{
    public interface ISessionLogic
    {
        bool ValidateLogin(string email, string password);

        bool ValidateSession(Guid token);

        void GenerateSession(Admin loggedAdmin);
        
    }
}
