using System;
using System.Linq;
using IMMRequest.DataAccess;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class SessionLogic : ISessionLogic
    {

        private IRepository<Admin> adminRepository;
        private ISessionRepository sessionRepository;

        public SessionLogic(IRepository<Admin> adminRepository,
        ISessionRepository sessionRepository)
        {
            this.adminRepository = adminRepository;
            this.sessionRepository = sessionRepository;
        }

        public bool ValidateSession(Guid adminId)
        {
            if(sessionRepository.ValidateSession(adminId))
            {
                return true;
            }
                return false;
        }

        public bool ValidateLogin(string email, string password)
        {
            if (email != null && password != null)
            {
                Admin loggedAdmin = adminRepository.GetByCondition(a => a.Email == email);
                if (loggedAdmin != null && loggedAdmin.Password.Equals(password))
                {
                    if(!ValidateSession(loggedAdmin.Id))
                    {
                        this.GenerateSession(loggedAdmin);
                    }
                    Session.LoggedAdmin = loggedAdmin;
                    return true;
                }
                return false;
            }
            return false;
        }
        
        //ESTE METODO SE USA LUEGO DE VALIDAR QUE EXISTE UN ADMIN CON EMAIL Y CONTRASEÑA CORRECTOS
        public void GenerateSession(Admin loggedUser)
        {
           
            Session s = new Session(){
                Id = Guid.NewGuid(),
                AdminId = loggedUser.Id
            };
            sessionRepository.Add(s);
            sessionRepository.SaveChanges();
        }
    }
}