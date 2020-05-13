using System;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;

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
            return sessionRepository.ValidateSession(adminId);
        }

        public Admin ValidateLogin(string email, string password)
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
                    return loggedAdmin;
                }
            }
            return null;
        }
        
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