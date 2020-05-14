using System;
using System.Collections.Generic;
using IMMRequest.DataAccess.Interfaces;
using System.Linq.Expressions;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;
using IMMRequest.Domain;

namespace IMMRequest.BusinessLogic
{
    public class AdminLogic : IAdminLogic
    {
        private IRepository<Admin> adminRepository;
        private IAdminValidatorHelper adminValidator; 

        public AdminLogic(IRepository<Admin> adminRepository)
        {
            this.adminRepository = adminRepository;
            this.adminValidator = new AdminValidatorHelper(adminRepository);
        }

        public Admin GetByCondition(Expression<Func<Admin, bool>> expression)
        {
            try
            {
                return adminRepository.GetByCondition(expression);
            }
            catch (DataAccessException)
            {
                throw new BusinessLogicException("Error: could not retrieve the specific Admin");
            }
        }

        public Admin Create(Admin admin)
        {
            admin.Id = Guid.NewGuid();
            adminValidator.ValidateAdd(admin);
            adminRepository.Add(admin);
            adminRepository.SaveChanges();
            return admin;
        }

        public Admin Get(Guid id)
        {
            Admin adminById = adminRepository.Get(id);
            if(adminById == null)
            {
                throw new BusinessLogicException("Error: Invalid ID, Admin does not exist");
            }
            return adminById;
        }

        public IEnumerable<Admin> GetAll()
        {
            return adminRepository.GetAll();
        }

        public Admin Update(Admin admin)
        {
            adminValidator.ValidateUpdate(admin);
            var adminToUpdate = Get(admin.Id);
            adminToUpdate.Name = admin.Name;
            adminToUpdate.Email = admin.Email;
            adminToUpdate.Password = admin.Password;
            adminValidator.ValidateAdminObject(adminToUpdate);
            adminRepository.Update(adminToUpdate);
            adminRepository.SaveChanges();
            return adminToUpdate;
        }

        public void Remove(Admin admin)
        {
            adminValidator.ValidateDelete(admin);
            adminRepository.Remove(admin);
            adminRepository.SaveChanges();
        }
    }
}