using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using IMMRequest.DataAccess;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class RequestLogic : IRequestLogic
    {
        private IRepository<Request> requestRepository;
        private IRepository<AFValue> aFValueRepository;
        private IRepository<Type> typeRepository;
        private IRequestValidatorHelper requestValidator;

        public RequestLogic(IRepository<Request> requestRepository,
            IRepository<AFValue> aFValueRepository, IRepository<Type> typeRepository)
        {
            this.requestRepository = requestRepository;
            this.aFValueRepository = aFValueRepository;
            this.typeRepository = typeRepository;
            requestValidator = new RequestValidatorHelper(requestRepository, aFValueRepository, typeRepository);
        }

        private void AddAFValues(Request request)
        {
            foreach (AFValue af in request.AddFieldValues)
            {
                aFValueRepository.Add(af);
            }
            if (request.AddFieldValues.Count() != 0)
            {
                aFValueRepository.SaveChanges();
            }
        }

        private int AssignRequestNumber()
        {
            return requestRepository.GetAll().ToList().Count() + 1;
        }

        public int Create(Request request)
        {
            requestValidator.ValidateRequestObject(request);
            requestValidator.ValidateType(request);
            requestValidator.ValidateAFValues(request);
            request.Status = Status.Creada;
            request.RequestNumber = AssignRequestNumber();
            AddAFValues(request);
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