using System;
using IMMRequest.DataAccess;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class AdminLogic
    {
        private IRepository<Admin> adminRepository;

        public AdminLogic(IRepository<Admin> adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public Admin Create(Admin admin)
        {
            if(admin.Name == "" || admin.Password == "" ||
                admin.Email == "")
            {
                throw new BusinessLogicException("Error: Admin had empty fields");
            }
            else
            {
                adminRepository.Add(admin);
                adminRepository.SaveChanges();
                return admin;
            }
        }
    }
}