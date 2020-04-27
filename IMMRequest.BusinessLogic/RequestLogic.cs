using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class RequestLogic
    {
        private IRepository<Request> requestRepository;
        private IRepository<AFValue> aFValueRepository;
        private IRepository<Type> typeRepository;

        public RequestLogic(IRepository<Request> requestRepository,
            IRepository<AFValue> aFValueRepository, IRepository<Type> typeRepository)
        {
            this.requestRepository = requestRepository;
            this.aFValueRepository = aFValueRepository;
            this.typeRepository = typeRepository;
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

        private bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty 
                && AreValidCharacters(str);
        }

        private bool AreValidCharacters(string str)
        {
            string character = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                character = str[i] + "";
                if (Regex.IsMatch(character, @"^[A-Za-z]$"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z\s]{1,}$");
        }
        public int Create(Request request)
        {
            if (!IsValidString(request.Details) || !IsValidEmail(request.Email)
                || !IsValidName(request.Name) || request.Type == null)
            {
                throw new BusinessLogicException("Error: Request had empty fields");
            }
            Type typeById = typeRepository.Get(request.Type.Id);
            if (typeById == null)
            {
                throw new BusinessLogicException("Error: Request's Type does not exist");
            }
            if (request.Type.AdditionalFields.Count != 0)
            {
                foreach (AFValue afv in request.AddFieldValues)
                {
                    if (afv == null)
                    {
                        throw new BusinessLogicException("Error: One Request's additional field value was empty");
                    }
                }
                bool notFound;
                foreach (AdditionalField af in request.Type.AdditionalFields)
                {
                    notFound = true;
                    foreach (AFValue afv in request.AddFieldValues)
                    {
                        if (afv.AddFieldId == af.Id)
                        {
                            notFound = false;
                            break;
                        }
                    }
                    if (notFound)
                    {
                        throw new BusinessLogicException("Error: One Request's additional field was not from type");
                    }
                }
                AdditionalField addFieldById;
                foreach (AFValue afv in request.AddFieldValues)
                {
                    addFieldById = request.Type.AdditionalFields.Find(x => x.Id == afv.AddFieldId);
                    bool hasRange = addFieldById.Range.Count != 0;
                    if (afv.Value == null || afv.Value.Trim() == string.Empty)
                    {
                        throw new BusinessLogicException("Error: One Request's additional field value was empty");
                    }
                    if (addFieldById.FieldType == FieldType.Texto)
                    {
                        if (Regex.IsMatch(afv.Value, @"^[0-9]$"))
                        {
                            throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
                        }
                        bool isDateTime = true;
                        try
                        {
                            DateTime date = DateTime.Parse(afv.Value);
                        }
                        catch (FormatException)
                        {
                            isDateTime = false;
                        }
                        if (isDateTime)
                        {
                            throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
                        }
                        if (hasRange)
                        {
                            bool isInRange = false;
                            foreach(Range range in addFieldById.Range)
                            {
                                if (range.Value.Equals(afv.Value))
                                {
                                    isInRange = true;
                                    break;
                                }
                            }
                            if (!isInRange)
                            {
                                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check fields's range");
                            }
                        }
                    }
                    else if (addFieldById.FieldType == FieldType.Entero)
                    {
                        if (!Regex.IsMatch(afv.Value, @"^[0-9]$"))
                        {
                            throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
                        }
                        if(hasRange)
                        {
                            int minValue = Int32.Parse(addFieldById.Range[0].Value);
                            int maxValue = Int32.Parse(addFieldById.Range[1].Value);
                            bool isInRange = Int32.Parse(afv.Value) >= minValue &&
                                Int32.Parse(afv.Value) <= maxValue;
                            if (!isInRange)
                            {
                                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check fields's range");
                            }
                        }
                    }
                    else if (addFieldById.FieldType == FieldType.Fecha)
                    {
                        try
                        {
                            DateTime date = DateTime.Parse(afv.Value);
                        }
                        catch (FormatException)
                        {
                            throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
                        }
                        if (hasRange)
                        {
                            DateTime minDate = DateTime.Parse(addFieldById.Range[0].Value);
                            DateTime maxDate = DateTime.Parse(addFieldById.Range[1].Value);
                            DateTime afvDate = DateTime.Parse(afv.Value);
                            bool isInRange = afvDate >= minDate && afvDate <= maxDate;
                            if (!isInRange)
                            {
                                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check fields's range");
                            }
                        }
                    }
                }
            }
            request.Status = Status.Creada;
            request.RequestNumber = AssignRequestNumber();
            requestRepository.Add(request);
            foreach (AFValue af in request.AddFieldValues)
            {
                aFValueRepository.Add(af);
            }
            if (request.AddFieldValues.Count() != 0)
            {
                aFValueRepository.SaveChanges();
            }
            requestRepository.SaveChanges();
            return request.RequestNumber;
        }

        private int AssignRequestNumber()
        {
            return requestRepository.GetAll().ToList().Count() + 1;
        }
    }
}