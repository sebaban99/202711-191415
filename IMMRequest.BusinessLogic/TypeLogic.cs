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
        private ITypeValidatorHelper typeValidator;

        public TypeLogic(ITypeRepository typeRepository, IRepository<Topic> topicRespository)
        {
            this.typeRepository = typeRepository;
            this.topicRespository = topicRespository;
            typeValidator = new TypeValidatorHelper(typeRepository, topicRespository);
        }

        private void FormatAdditionalFields(Type type)
        {
            foreach (AdditionalField af in type.AdditionalFields)
            {
                af.Id = Guid.NewGuid();
                af.Type = type;
                if (af.Range.Count != 0)
                {
                    foreach (Range range in af.Range)
                    {
                        range.Id = Guid.NewGuid();
                        range.AdditionalField = af;
                    }
                }
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
            FormatAdditionalFields(type);
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