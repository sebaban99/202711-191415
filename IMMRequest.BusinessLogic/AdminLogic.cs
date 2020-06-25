using System;
using System.Collections.Generic;
using IMMRequest.DataAccess.Interfaces;
using System.Linq.Expressions;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;
using System.Linq;

namespace IMMRequest.BusinessLogic
{
    public class AdminLogic : IAdminLogic
    {
        private IRepository<Admin> adminRepository;
        private IRequestRepository requestRepository;
        private IAdminValidatorHelper adminValidator; 

        public AdminLogic(IRepository<Admin> adminRepository, IRequestRepository requestRepository)
        {
            this.adminRepository = adminRepository;
            this.requestRepository = requestRepository;
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

        public IEnumerable<ReportTypeAElement> GenerateReportA(DateTime from, DateTime until, string email)
        {
            List<ReportTypeAElement> report = new List<ReportTypeAElement>();
            List<Status> differentStatus = new List<Status>();
            List<Request> requestsByCondition = (List<Request>)requestRepository.GetAllByCondition(r => r.CreationDate >= from && r.CreationDate <= until && r.Email.Equals(email)).ToList();
            if (requestsByCondition.Count != 0)
            {
                foreach (Request r in requestsByCondition)
                {
                    if (!differentStatus.Contains(r.Status))
                    {
                        differentStatus.Add(r.Status);
                        ReportTypeAElement newReportElement = new ReportTypeAElement();
                        newReportElement.Amount = 1;
                        newReportElement.Status = r.Status;
                        newReportElement.RequestNumbers.Add(r.RequestNumber);
                        report.Add(newReportElement);
                    }
                    else
                    {
                        foreach (ReportTypeAElement rAElement in report)
                        {
                            if (rAElement.Status.Equals(r.Status))
                            {
                                rAElement.Amount++;
                                rAElement.RequestNumbers.Add(r.RequestNumber);
                                break;
                            }
                        }
                    }
                }
            }
            return report;
        }

        public IEnumerable<ReportTypeBElement> GenerateReportB(DateTime from, DateTime until)
        {
            List<ReportTypeBElement> report = new List<ReportTypeBElement>();
            List<Type> differentTypes = new List<Type>();
            List<Request> requestsByCondition = (List<Request>)requestRepository.GetAllByCondition(r => r.CreationDate >= from && r.CreationDate <= until).ToList();
            if(requestsByCondition.Count != 0)
            {
                foreach(Request r in requestsByCondition)
                {
                    if (!differentTypes.Contains(r.Type))
                    {
                        differentTypes.Add(r.Type);
                        ReportTypeBElement newReportElement = new ReportTypeBElement();
                        newReportElement.Amount = 1;
                        newReportElement.Type = r.Type;
                        InsertWithOrder(report, newReportElement);
                    }
                    else
                    {
                        foreach(ReportTypeBElement rBElement in report)
                        {
                            if (rBElement.Type.Equals(r.Type))
                            {
                                rBElement.Amount++;
                                break;
                            }
                        }
                    }
                }
            }
            return report;
        }

        private void InsertWithOrder(List<ReportTypeBElement> report, ReportTypeBElement newReportElement)
        {
            bool wasInserted = false;
            for (int i = 0; i < report.Count && !wasInserted; i++)
            {
                if(report[i].Amount == 1)
                {
                    if(report[i].Type.CreationDate > newReportElement.Type.CreationDate)
                    {
                        report.Insert(i, newReportElement);
                        wasInserted = true;
                    }
                }
            }
            if (!wasInserted)
            {
                report.Add(newReportElement);
            }
        }
    }
}