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

        public bool IsValidString(string str)
        {
            return str != null && str.Trim() != string.Empty;
        }

        public Type Create(Type type)
        {
            if (!IsValidString(type.Name) || type.Topic == null ||
                type.AdditionalFields == null)
            {
                throw new BusinessLogicException("Error: Type had empty fields");
            }
            Topic topicInDB = topicRespository.Get(type.Topic.Id);
            if(topicInDB == null)
            {
                throw new BusinessLogicException("Error: Topic does not exist");
            }
            Type typeInDB = typeRepository.GetByCondition(t => t.Name == type.Name &&
            t.Topic == type.Topic);
            if(typeInDB != null)
            {
                throw new BusinessLogicException("Error: Type with same name already registered");
            }
            if(type.AdditionalFields.Count != 0)
            {
                foreach(AdditionalField af in type.AdditionalFields)
                {
                    if(af.Range == null || af.Type != type || !IsValidString(af.Name))
                    {
                        throw new BusinessLogicException("Error: AdditionalField had empty fields");
                    }
                    additionalFieldRespository.Add(af);
                }
                additionalFieldRespository.SaveChanges();
            }
            typeRepository.Add(type);
            typeRepository.SaveChanges();
            return type;
        }
    }
}