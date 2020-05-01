using System;
using System.Linq;
using IMMRequest.DataAccess;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public interface ISessionLogic
    {
        bool ValidateEmailAndPassword(string email, string password);

        bool ValidToken(string token);

        void GenerateToken(Admin loggedAdmin);
        
    }
}
