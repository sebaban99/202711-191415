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

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[0-9!#()*/\-+\s]{6,}$");
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

        private bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty 
                && AreValidCharacters(str);
        }

        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z\s]{1,}$");
        }

        private bool AreEmptyFields(Request request)
        {
            return !IsValidString(request.Details) || !IsValidEmail(request.Email)
                    || !IsValidName(request.Name) || !IsValidPhone(request.Phone)
                    || request.Type == null;
        }

        private void ValidateRequestObject(Request request)
        {
            if (AreEmptyFields(request))
            {
                throw new BusinessLogicException("Error: Request had empty fields");
            }
        }

        private bool IsTypeRegistered(Guid id)
        {
            Type typeInDB = typeRepository.Get(id);
            return typeInDB != null;
        }

        private void ValidateType(Request request)
        {
            if (!IsTypeRegistered(request.Type.Id))
            {
                throw new BusinessLogicException("Error: Request's Type does not exist");
            }
        }

        private void AreAFValuesEmpty(List<AFValue> aFValues)
        {
            foreach (AFValue afv in aFValues)
            {
                if (afv == null)
                {
                    throw new BusinessLogicException("Error: One Request's additional field value was empty");
                }
            }
        }

        private void AreAFValuesTypeConsistent(Request request)
        {
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
        }

        private void ValidateAFVObject(AFValue afv)
        {
            if (afv.Value == null || afv.Value.Trim() == string.Empty)
            {
                throw new BusinessLogicException("Error: One Request's additional field value was empty");
            }
        }

        private bool IsNumberAFV(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]$");
        }

        private bool IsDateAFV(string value)
        {
            bool isDateTime = true;
            try
            {
                DateTime date = DateTime.Parse(value);
            }
            catch (FormatException)
            {
                isDateTime = false;
            }
            return isDateTime;
        }

        private void ValidateTextRange(AdditionalField addField, AFValue afv)
        {
            if (addField.Range.Count != 0)
            {
                bool isInRange = false;
                foreach (Range range in addField.Range)
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

        private void ValidateTextAFV(AdditionalField addFieldById, AFValue afv)
        {
            if (IsNumberAFV(afv.Value))
            {
                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
            }
            if (IsDateAFV(afv.Value))
            {
                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
            }
            ValidateTextRange(addFieldById, afv);
        }

        private void ValidateIntegerRange(AdditionalField addField, AFValue afv)
        {
            if (addField.Range.Count != 0)
            {
                int minValue = Int32.Parse(addField.Range[0].Value);
                int maxValue = Int32.Parse(addField.Range[1].Value);
                bool isInRange = Int32.Parse(afv.Value) >= minValue &&
                    Int32.Parse(afv.Value) <= maxValue;
                if (!isInRange)
                {
                    throw new BusinessLogicException("Error: One Request's additional field value was invalid, check fields's range");
                }
            }
        }

        private void ValidateIntegerAFV(AdditionalField addField, AFValue afv)
        {
            if (!IsNumberAFV(afv.Value))
            {
                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
            }
            ValidateIntegerRange(addField, afv);
        }

        private void ValidateDateRange(AdditionalField addField, AFValue afv)
        {
            if (addField.Range.Count != 0)
            {
                DateTime minDate = DateTime.Parse(addField.Range[0].Value);
                DateTime maxDate = DateTime.Parse(addField.Range[1].Value);
                DateTime afvDate = DateTime.Parse(afv.Value);
                bool isInRange = afvDate >= minDate && afvDate <= maxDate;
                if (!isInRange)
                {
                    throw new BusinessLogicException("Error: One Request's additional field value was invalid, check fields's range");
                }
            }
        }

        private void ValidateDateAFV(AdditionalField addFieldById, AFValue afv)
        {
            if(!IsDateAFV(afv.Value))
            {
                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
            }
            ValidateDateRange(addFieldById, afv);
        }

        private void AreValuesValid(Request request)
        {
            AdditionalField addFieldById;
            foreach (AFValue afv in request.AddFieldValues)
            {
                addFieldById = request.Type.AdditionalFields.Find(x => x.Id == afv.AddFieldId);
                ValidateAFVObject(afv);
                if (addFieldById.FieldType == FieldType.Texto)
                {
                    ValidateTextAFV(addFieldById, afv);
                }
                else if (addFieldById.FieldType == FieldType.Entero)
                {
                    ValidateIntegerAFV(addFieldById, afv);
                }
                else if (addFieldById.FieldType == FieldType.Fecha)
                {
                    ValidateDateAFV(addFieldById, afv);
                }
            }
        }

        private void ValidateAFValues(Request request)
        {
            if (request.Type.AdditionalFields.Count != 0)
            {
                AreAFValuesEmpty(request.AddFieldValues);
                AreAFValuesTypeConsistent(request);
                AreValuesValid(request);
            }
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
            ValidateRequestObject(request);
            ValidateType(request);
            ValidateAFValues(request);
            request.Status = Status.Creada;
            request.RequestNumber = AssignRequestNumber();
            AddAFValues(request);
            requestRepository.Add(request);
            requestRepository.SaveChanges();
            return request.RequestNumber;
        }
    }
}