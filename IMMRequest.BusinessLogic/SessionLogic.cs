using System;
using System.Linq;
using IMMRequest.DataAccess;
using IMMRequest.DataAccess.Repositories;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class SessionLogic : ISessionLogic
    {

        private AdminRepository adminRepository;
        private SessionRepository sessionRepository;

        public SessionLogic(AdminRepository adminRepository,
        SessionRepository sessionRepository)
        {
            this.adminRepository = adminRepository;
            this.sessionRepository = sessionRepository;
        }

        public bool ValidToken(string token)
        {
            if(sessionRepository.ValidToken(token))
            {
                return true;
            }
                return false;
        }

        public bool ValidateEmailAndPassword(string email, string password)
        {
            if (email != null && password != null)
            {
                Admin loggedAdmin = adminRepository.GetByCondition(a => a.Email == email);
                if (loggedAdmin != null && loggedAdmin.Password.Equals(password))
                {
                    this.GenerateToken(loggedAdmin);
                    Session.LoggedAdmin = loggedAdmin;
                    return true;
                }
                return false;
            }
            return false;
        }
        
        //ESTE METODO SE USA LUEGO DE VALIDAR QUE EXISTE UN ADMIN CON EMAIL Y CONTRASEÑA CORRECTOS
        public void GenerateToken(Admin loggedUser)
        {
            if (ValidToken(loggedUser.SessionToken.ToString()))
            {
                loggedUser.SessionToken = Guid.NewGuid();
                adminRepository.ModifyToken(loggedUser);
            }

        }
    }
}