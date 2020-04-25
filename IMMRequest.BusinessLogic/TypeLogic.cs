using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class TypeLogic
    {
        private IRepository<Type> typeRepository;
        private IRepository<Topic> topicRespository;
        private IRepository<AdditionalField> additionalFieldRespository;

        public TypeLogic(IRepository<Type> typeRepository, IRepository<Topic> topicRespository,
            IRepository<AdditionalField> additionalFieldRespository)
        {
            this.typeRepository = typeRepository;
            this.topicRespository = topicRespository;
            this.additionalFieldRespository = additionalFieldRespository;
        }

        private bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty &&
                (!Regex.IsMatch(str, @"^[0-9]$") &&
                Regex.IsMatch(str, @"^[A-Za-z0-9!()'/.,\s-]{1,}$"));
        }

        private bool AreEmptyFields(Type type)
        {
            return !IsValidString(type.Name) || type.Topic == null ||
                type.AdditionalFields == null;
        }

        private void ValidateTypeObject(Type type)
        {
            if (AreEmptyFields(type))
            {
                throw new BusinessLogicException("Error: Type had empty fields");
            }
        }

        private bool IsTopicRegistered(Guid id)
        {
            Topic topicInDB = topicRespository.Get(id);
            return topicInDB != null;
        }

        private void ValidateTopic(Type type)
        {
            if (!IsTopicRegistered(type.Id))
            {
                throw new BusinessLogicException("Error: Topic does not exist");
            }
        }

        private bool IsTypeRegistered(Type type)
        {
            Type typeInDB = typeRepository.GetByCondition(t => t.Name == type.Name &&
            t.Topic == type.Topic);
            return typeInDB != null;
        }

        private void ValidateType(Type type)
        {
            if (IsTypeRegistered(type))
            {
                throw new BusinessLogicException("Error: Type with same name already registered");
            }
        }

        private void ValidateTextRange(AdditionalField af)
        {
            bool areTextsValid = true;
            for (int i = 0; i < af.Range.Count && areTextsValid; i++)
            {
                try
                {
                    if (!IsValidString(af.Range[i].Value))
                    {
                        areTextsValid = false;
                    }
                }
                catch (Exception)
                {
                    areTextsValid = false;
                }
            }
            if (!areTextsValid)
            {
                throw new BusinessLogicException($"Error: {af.Name}'s range has invalid text values");
            }
        }

        private static void ValidateIntegerRange(AdditionalField af)
        {
            bool areIntsValid = true;
            try
            {
                int minValue = Int32.Parse(af.Range[0].Value);
                int maxValue = Int32.Parse(af.Range[1].Value);
                areIntsValid = minValue < maxValue;
            }
            catch (Exception)
            {
                areIntsValid = false;
            }
            if (!areIntsValid)
            {
                throw new BusinessLogicException($"Error: {af.Name}'s range has invalid integer values");
            }
        }

        private static void ValidateDateRange(AdditionalField af)
        {
            bool areDatesValid = true;
            try
            {
                DateTime minDate = DateTime.Parse(af.Range[0].Value);
                DateTime maxDate = DateTime.Parse(af.Range[1].Value);
                areDatesValid = minDate.Day < maxDate.Day;
            }
            catch (Exception)
            {
                areDatesValid = false;
            }
            if (!areDatesValid)
            {
                throw new BusinessLogicException($"Error: {af.Name}'s range has invalid date values");
            }
        }

        private void ValidateAFRange(AdditionalField af)
        {
            if (af.Range.Count != 0)
            {
                if (af.FieldType == FieldType.Texto)
                {
                    ValidateTextRange(af);
                }
                else if (af.Range.Count == 2)
                {
                    if (af.FieldType == FieldType.Fecha)
                    {
                        ValidateDateRange(af);
                    }
                    else
                    {
                        ValidateIntegerRange(af);
                    }
                }
                else
                {
                    throw new BusinessLogicException($"Error: {af.Name}'s range has too many date or integer values");
                }
            }
        }

        private bool IsAFValid(AdditionalField af, Type type)
        {
            return af.Range != null && af.Type == type && IsValidString(af.Name);
        }

        private void ValidateAdditionalFields(Type type)
        {
            if (type.AdditionalFields.Count != 0)
            {
                foreach (AdditionalField af in type.AdditionalFields)
                {
                    if (!IsAFValid(af, type))
                    {
                        throw new BusinessLogicException("Error: AdditionalField had empty fields");
                    }
                    ValidateAFRange(af);
                }
            }
        }

        private void ValidateAdd(Type type)
        {
            ValidateTypeObject(type);
            ValidateTopic(type);
            ValidateType(type);
            ValidateAdditionalFields(type);
        }

        private void AddAdditionalFields(Type type)
        {
            foreach (AdditionalField af in type.AdditionalFields)
            {
                additionalFieldRespository.Add(af);
            }
            if (type.AdditionalFields.Count != 0)
            {
                additionalFieldRespository.SaveChanges();
            }
        }

        public Type Create(Type type)
        {
            ValidateAdd(type);
            AddAdditionalFields(type);
            typeRepository.Add(type);
            typeRepository.SaveChanges();
            return type;
        }

        public Type Get(Guid id)
        {
            Type typeById = typeRepository.Get(id);
            if (typeById == null)
            {
                throw new BusinessLogicException("Error: Invalid ID, Type does not exist");

            }
            return typeById;
        }

        public IEnumerable<Type> GetAll()
        {
            return typeRepository.GetAll();
        }

        public void Remove(Type type)
        {
            ValidateRemove(type);
            typeRepository.Remove(type);
            typeRepository.SaveChanges();
        }

        private void ValidateRemove(Type type)
        {
            Type typeById = typeRepository.Get(type.Id);
            if (typeById == null)
            {
                throw new BusinessLogicException("Error: Type to delete doesn't exist");
            }
        }
    }
}