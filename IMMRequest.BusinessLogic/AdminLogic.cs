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
            ValidateAdd(admin);
            adminRepository.Add(admin);
            adminRepository.SaveChanges();
            return admin;
        }

        private void ValidateAdd(Admin admin)
        {
            if (!AreEmptyFields(admin))
            {
                throw new BusinessLogicException("Error: Admin had empty fields");
            }
            else if (!IsValidEmail(admin.Email))
            {
                throw new BusinessLogicException("Error: Invalid email format");
            }
            else if (ExistsAdmin(admin))
            {
                throw new BusinessLogicException("Error: Admin with same email already registered");
            }
        }

        private bool ExistsAdmin(Admin admin)
        {
            var adminById = adminRepository.GetByCondition(a => a.Email == admin.Email);
            if (adminById != null)
            {
                return true;
            }
            return false;
        }

        private bool AreEmptyFields(Admin admin)
        {
            return IsValidString(admin.Email) && IsValidString(admin.Name) &&
            IsValidString(admin.Password);
        }

        public bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}