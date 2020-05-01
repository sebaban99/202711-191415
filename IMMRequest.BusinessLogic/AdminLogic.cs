﻿using System;
using System.Collections.Generic;
using IMMRequest.DataAccess;
using System.Linq.Expressions;
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

        private void ValidateAdminObject(Admin admin)
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

        public Admin GetByCondition(Expression<Func<Admin, bool>> expression)
        {
            try
            {
                return adminRepository.GetByCondition(expression);
            }
            catch (BusinessLogicException)
            {
                throw new BusinessLogicException("Error: could not retrieve the specific Admin");
            }
        }

        private void ValidateAdd(Admin admin)
        {
            ValidateAdminObject(admin);
            if (IsEmailRegistered(admin))
            {
                throw new BusinessLogicException("Error: Admin with same email already registered");
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

        private bool AreEmptyFields(Admin admin)
        {
            return IsValidString(admin.Email) && IsValidString(admin.Name) &&
            IsValidString(admin.Password);
        }

        private bool IsValidString(string str)
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

        public Admin Create(Admin admin)
        {
            ValidateAdd(admin);
            adminRepository.Add(admin);
            adminRepository.SaveChanges();
            return admin;
        }

        public Admin Get(Guid id)
        {
            Admin adminById = adminRepository.Get(id);
            if(adminById == null)
            {
                throw new BusinessLogicException("Error: Invalid ID, Type does not exist");
            }
            return adminById;
        }

        public IEnumerable<Admin> GetAll()
        {
            return adminRepository.GetAll();
        }

        public Admin Update(Admin admin)
        {
            ValidateUpdate(admin);
            var adminToUpdate = Get(admin.Id);
            adminToUpdate.Name = admin.Name;
            adminToUpdate.Email = admin.Email;
            adminToUpdate.Password = admin.Password;
            ValidateAdminObject(adminToUpdate);
            adminRepository.Update(adminToUpdate);
            adminRepository.SaveChanges();
            return adminToUpdate;
        }

        private void ValidateUpdate(Admin admin)
        {
            var adminById = Get(admin.Id);
            if (adminById == null)
            {
                throw new BusinessLogicException("Error: Admin to update doesn't exist");
            }
        }
        public void Remove(Admin admin)
        {
            ValidateDelete(admin);
            adminRepository.Remove(admin);
            adminRepository.SaveChanges();
        }

        private void ValidateDelete(Admin admin)
        {
            var adminById = Get(admin.Id);
            if (adminById == null)
            {
                throw new BusinessLogicException("Error: Admin to delete doesn't exist");
            }
        }
    }
}