using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IMMRequest.Exceptions;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class RequestValidatorHelper : IRequestValidatorHelper
    {
        private IRepository<Type> typeRepository; 

        public RequestValidatorHelper(IRepository<Type> typeRepository)
        {
            this.typeRepository = typeRepository;
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

        public bool AreEmptyFields(Request request)
        {
            return !IsValidString(request.Details) || !IsValidEmail(request.Email)
                    || !IsValidName(request.Name) || !IsValidPhone(request.Phone);
        }

        public void ValidateAdd(Request request)
        {
            ValidateRequestObject(request);
            ValidateType(request);
            ValidateAFValues(request);
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[0-9!#()*/\-+\s]{6,}$");
        }

        public void ValidateRequestObject(Request request)
        {
            if (AreEmptyFields(request))
            {
                throw new BusinessLogicException("Error: Request had empty fields");
            }
        }

        private bool IsTypeValid(Guid id)
        {
            Type typeInDB = typeRepository.Get(id);
            return typeInDB != null && typeInDB.IsActive;
        }

        public void ValidateType(Request request)
        {
            if (!IsTypeValid(request.TypeId))
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

        private void ValidateAFVObject(AFValue afv)
        {
            if (afv.Values == null || afv.Values.Count == 0)
            {
                throw new BusinessLogicException("Error: One Request's additional field value was empty");
            }
        }

        private bool IsNumberAFV(List<AFValueItem> values)
        {
            foreach(AFValueItem item in values)
            {
                if (!Regex.IsMatch(item.Value, @"^[0-9]$"))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDateAFV(List<AFValueItem> values)
        {
            bool isDateTime = true;
            foreach (AFValueItem item in values)
            {
                if (isDateTime)
                {
                    try
                    {
                        DateTime date = DateTime.Parse(item.Value);
                    }
                    catch (FormatException)
                    {
                        isDateTime = false;
                    }
                }
                else
                {
                    break;
                }
            }
            return isDateTime;
        }

        private void ValidateTextRange(AdditionalField addField, AFValue afv)
        {
            if (addField.Range.Count != 0)
            {
                bool isInRange = true;
                bool notFound;
                foreach (AFValueItem item in afv.Values)
                {
                    notFound = true;
                    foreach(AFRangeItem r in addField.Range)
                    {
                        if(r.Value == item.Value)
                        {
                            notFound = false;
                            break;
                        }
                    }
                    if (notFound)
                    {
                        isInRange = false;
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
            if (IsNumberAFV(afv.Values))
            {
                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
            }
            if (IsDateAFV(afv.Values))
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
                bool isInRange = true;
                foreach(AFValueItem item in afv.Values)
                {
                    if (isInRange)
                    {
                        isInRange = Int32.Parse(item.Value) >= minValue && 
                            Int32.Parse(item.Value) <= maxValue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (!isInRange)
                {
                    throw new BusinessLogicException($"Error: One Request's additional field value was invalid, check {addField.Name}'s range");
                }
            }
        }

        private void ValidateIntegerAFV(AdditionalField addField, AFValue afv)
        {
            if (!IsNumberAFV(afv.Values))
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
                bool isInRange = true;
                foreach (AFValueItem item in afv.Values)
                {
                    if (isInRange)
                    {
                        try
                        {
                            DateTime afvDate = DateTime.Parse(item.Value);
                            isInRange = afvDate >= minDate && afvDate <= maxDate;
                        }
                        catch (FormatException)
                        {
                            isInRange = false;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (!isInRange)
                {
                    throw new BusinessLogicException($"Error: One Request's additional field value was invalid, check {addField.Name}'s range");
                }
            }
        }

        private void ValidateDateAFV(AdditionalField addFieldById, AFValue afv)
        {
            if (!IsDateAFV(afv.Values))
            {
                throw new BusinessLogicException("Error: One Request's additional field value was invalid, check data type");
            }
            ValidateDateRange(addFieldById, afv);
        }

        private void AreValuesValid(Request request, Type type)
        {
            AdditionalField addFieldById;
            foreach (AFValue afv in request.AddFieldValues)
            {
                addFieldById = type.AdditionalFields.Find(x => x.Id == afv.AdditionalField.Id);
                ValidateAFVObject(afv);
                if(addFieldById.FieldType == FieldType.Bool)
                {
                    ValidateBoolAFV(afv);
                }
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

        private void ValidateBoolAFV(AFValue afv)
        {
            if(afv.Values.Count != 1 || !afv.Values[0].Value.Equals("True") && !afv.Values[0].Value.Equals("False"))
            {
                throw new BusinessLogicException("Error: One boolean Additional Field type value was not valid");
            }
        }

        public void ValidateAFValues(Request request)
        {
            Type typeById = typeRepository.Get(request.TypeId);
            if (typeById.AdditionalFields != null && typeById.AdditionalFields.Count != 0)
            {
                AreAFValuesEmpty(request.AddFieldValues);
                AreValuesValid(request, typeById);
            }
        }

        public void ValidateUpdate(Request request, Request requestToUpdate)
        {
            if (!IsStatusUpdateValid(requestToUpdate.Status, request.Status))
            {
                throw new BusinessLogicException("Error: Invalid Status update, Request's new status must be next or prior to old status");
            }
            if (request.Description != requestToUpdate.Description &&
                !IsValidString(request.Description))
            {
                throw new BusinessLogicException("Error: Invalid Description update, Request's new description was empty or contained only symbols");
            }
        }

        private bool IsStatusUpdateValid(Status oldStatus, Status newStatus)
        {
            if (newStatus == oldStatus)
            {
                return true;
            }
            else if (oldStatus == Status.Creada)
            {
                return newStatus == Status.Revision;
            }
            else if (oldStatus == Status.Revision)
            {
                return newStatus == Status.Creada ||
                    newStatus == Status.Aceptada || newStatus == Status.Denegada;
            }
            else if (oldStatus == Status.Aceptada || oldStatus == Status.Denegada)
            {
                return newStatus == Status.Aceptada || newStatus == Status.Revision ||
                    newStatus == Status.Denegada || newStatus == Status.Finalizada;
            }
            else
            {
                return newStatus == Status.Aceptada || newStatus == Status.Denegada;
            }
        }
    }
}
