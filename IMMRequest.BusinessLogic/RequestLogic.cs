using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class RequestLogic : IRequestLogic
    {
        private IRequestRepository requestRepository;
        private IRequestValidatorHelper requestValidator;

        public RequestLogic(IRequestRepository requestRepository, ITypeRepository typeRepository,
            IRepository<AdditionalField> afRepository, IRepository<AFRangeItem> rangeRepository)
        {
            this.requestRepository = requestRepository;
            requestValidator = new RequestValidatorHelper(typeRepository, afRepository, rangeRepository);
        }

        private void FormatAFValues(Request request)
        {
            foreach (AFValue af in request.AddFieldValues)
            {
                af.Id = Guid.NewGuid();
                af.Request = request;
                foreach(AFValueItem item in af.Values)
                {
                    item.Id = Guid.NewGuid();
                    item.AFValue = af;
                }
            }
        }

        public int AssignRequestNumber()
        {
            return requestRepository.GetAmountOfElements() + 1;
        }

        private void GiveNewRequestFormat(Request request)
        {
            request.Status = Status.Creada;
            request.RequestNumber = AssignRequestNumber();
            request.Id = Guid.NewGuid();
            request.CreationDate = DateTime.Now;
        }

        public int Create(Request request)
        {
            requestValidator.ValidateAdd(request);
            GiveNewRequestFormat(request);
            FormatAFValues(request);
            requestRepository.Add(request);
            requestRepository.SaveChanges();
            return request.RequestNumber;
        }

        public Request Update(Request request)
        {
            Request requestToUpdate = Get(request.Id);
            requestValidator.ValidateUpdate(request, requestToUpdate);
            requestToUpdate.Status = request.Status;
            requestToUpdate.Description = request.Description;
            requestRepository.Update(requestToUpdate);
            requestRepository.SaveChanges();
            return requestToUpdate;
        }

        public Request Get(Guid id)
        {
            Request requestById = requestRepository.Get(id);
            if (requestById == null)
            {
                throw new BusinessLogicException("Error: Invalid ID, Request does not exist");
            }
            return requestById;
        }

        public IEnumerable<Request> GetAll()
        {
            return requestRepository.GetAll();
        }


        public Request GetByCondition(Expression<Func<Request, bool>> expression)
        {
            try
            {
                return requestRepository.GetByCondition(expression);
            }
            catch (DataAccessException)
            {
                throw new BusinessLogicException("Error: could not retrieve the specific Request");
            }
        }
    }
}