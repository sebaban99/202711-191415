using System;
using System.Collections.Generic;
using System.Text;
using IMMRequest.DataAccess;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class AdminValidatorHelper : BaseValidator<Admin>
    {
        private IRepository<Admin> adminRepository;

        public AdminValidatorHelper(IRepository<Admin> adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public override void ValidateEntityObject(Admin admin)
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

        private bool IsEmailRegistered(Admin admin)
        {
            var adminById = adminRepository.GetByCondition(a => a.Email == admin.Email);
            if (adminById != null)
            {
                return true;
            }
            return false;
        }

        public override void ValidateAdd(Admin admin)
        {
            ValidateEntityObject(admin);
            if (IsEmailRegistered(admin))
            {
                throw new BusinessLogicException("Error: Admin with same email already registered");
            }
        }

        private bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty;
        }

        public override bool AreEmptyFields(Admin admin)
        {
            return IsValidString(admin.Email) && IsValidString(admin.Name) &&
           IsValidString(admin.Password);
        }

        public override void ValidateDelete(Admin admin)
        {
            Admin adminById = adminRepository.Get(admin.Id);
            if (adminById == null)
            {
                throw new BusinessLogicException("Error: Admin to delete doesn't exist");
            }
        }

        public override void ValidateUpdate(Admin admin)
        {
            Admin adminById = adminRepository.Get(admin.Id);
            if (adminById == null)
            {
                throw new BusinessLogicException("Error: Admin to update doesn't exist");
            }
        }
    }
}
