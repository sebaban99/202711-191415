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
            if(!AreEmptyFields(admin))
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

        private bool AreEmptyFields(Admin admin)
        {
            return IsStringValid(admin.Email) && IsStringValid(admin.Name) &&
                IsStringValid(admin.Password);
        }

        public bool IsStringValid(string str)
        {
            return str != null && str != string.Empty;
        }
    }
}