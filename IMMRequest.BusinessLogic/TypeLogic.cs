using System;
using System.Collections.Generic;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Exceptions;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class TypeLogic : ITypeLogic
    {
        private ITypeRepository typeRepository;
        private IRepository<Topic> topicRespository;
        private IRepository<AdditionalField> additionalFieldRespository;
        private ITypeValidatorHelper typeValidator;

        public TypeLogic(ITypeRepository typeRepository, IRepository<Topic> topicRespository,
            IRepository<AdditionalField> additionalFieldRespository)
        {
            this.typeRepository = typeRepository;
            this.topicRespository = topicRespository;
            this.additionalFieldRespository = additionalFieldRespository;
            typeValidator = new TypeValidatorHelper(typeRepository, topicRespository);
        }

        private void AddAdditionalFields(Type type)
        {
            foreach (AdditionalField af in type.AdditionalFields)
            {
                af.Id = Guid.NewGuid();
                af.Type = type;
                if(af.Range.Count != 0)
                {
                    foreach(Range range in af.Range)
                    {
                        range.Id = Guid.NewGuid();
                        range.AdditionalFieldId = af.Id;
                    }
                }
                additionalFieldRespository.Add(af);
            }
            if (type.AdditionalFields.Count != 0)
            {
                additionalFieldRespository.SaveChanges();
            }
        }

        private void GiveNewTypeFormat(Type type)
        {
            Topic realEntity = topicRespository.Get(type.Topic.Id);
            type.Topic = realEntity;
            type.IsActive = true;
            type.Id = Guid.NewGuid();
        }

        public Type Create(Type type)
        {
            typeValidator.ValidateAdd(type);
            GiveNewTypeFormat(type);
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
            return typeRepository.GetActiveTypes();
        }

        public void Remove(Type type)
        {
            typeValidator.ValidateDelete(type);
            typeRepository.SoftDelete(type);
            typeRepository.SaveChanges();
        }
    }
}