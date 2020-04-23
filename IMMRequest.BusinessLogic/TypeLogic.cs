using System;
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
            return str != null && str.Trim() != string.Empty;
        }

        private bool AreEmptyFields(Type type)
        {
            return IsValidString(type.Name) && type.Topic != null &&
                type.AdditionalFields != null;
        }

        private void ValidateTypeObject(Type type)
        {
            if (!AreEmptyFields(type))
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
    }
}