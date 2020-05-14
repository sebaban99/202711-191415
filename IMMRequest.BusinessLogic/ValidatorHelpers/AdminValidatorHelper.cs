using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Exceptions;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class AdminValidatorHelper : IAdminValidatorHelper
    {
        private IRepository<Admin> adminRepository;

        public AdminValidatorHelper(IRepository<Admin> adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public void ValidateAdminObject(Admin admin)
        {
            if (!AreEmptyFields(admin))
            {
                throw new BusinessLogicException("Error: Admin had empty fields");
            }
            else if (!IsValidEmail(admin.Email))
            {
                throw new BusinessLogicException("Error: Invalid email format");
            }
        }

        public bool IsValidEmail(string email)
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

        private bool IsEmailRegistered(Admin admin)
        {
            var adminById = adminRepository.GetByCondition(a => a.Email == admin.Email);
            if (adminById != null)
            {
                return true;
            }
            return false;
        }

        public void ValidateAdd(Admin admin)
        {
            ValidateAdminObject(admin);
            if (IsEmailRegistered(admin))
            {
                throw new BusinessLogicException("Error: Admin with same email already registered");
            }
        }

        private bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty;
        }

        public bool AreEmptyFields(Admin admin)
        {
            return IsValidString(admin.Email) && IsValidString(admin.Name) &&
           IsValidString(admin.Password);
        }

        public void ValidateDelete(Admin admin)
        {
            Admin adminById = adminRepository.Get(admin.Id);
            if (adminById == null)
            {
                throw new BusinessLogicException("Error: Admin to delete doesn't exist");
            }
        }

        public void ValidateUpdate(Admin admin)
        {
            Admin adminById = adminRepository.Get(admin.Id);
            if (adminById == null)
            {
                throw new BusinessLogicException("Error: Admin to update doesn't exist");
            }
        }
    }
}
