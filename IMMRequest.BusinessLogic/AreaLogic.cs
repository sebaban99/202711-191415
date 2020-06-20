using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.DataAccess.Interfaces;
using IMMRequest.Domain;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class AreaLogic : IAreaLogic
    {
        private IRepository<Area> areaRepository;
        private IRepository<Topic> topicRepository;
        private ITypeRepository typeRepository;
        private IAreaValidatorHelper areaValidator;
        private IRepository<AdditionalField> additionalfieldRepository;
        private IRepository<AFRangeItem> rangeRepository;


        public AreaLogic(IRepository<Area> areaRespository, IRepository<Topic> topicRepository, ITypeRepository typeRepository,
             IRepository<AdditionalField> additionalfieldRepository, IRepository<AFRangeItem> rangeRepository)
        {
            this.areaRepository = areaRespository;
            this.topicRepository = topicRepository;
            this.typeRepository = typeRepository;
            this.additionalfieldRepository = additionalfieldRepository;
            this.rangeRepository = rangeRepository;
            areaValidator = new AreaValidatorHelper(areaRespository);
        }

        public Area Create(Area area)
        {
            areaValidator.ValidateAdd(area);
            area.Id = Guid.NewGuid();
            areaRepository.Add(area);
            areaRepository.SaveChanges();
            return area;
        }

        public IEnumerable<Area> GetAll()
        {
            List<Area> formattedAreas = new List<Area>();
            List<Area> areasInDB = (List<Area>)areaRepository.GetAll();
            foreach(Area area in areasInDB)
            {
                List<Topic> areaTopics = topicRepository.GetAllByCondition(t => t.Area.Name == area.Name).ToList();
                area.Topics = areaTopics;
                foreach(Topic topic in area.Topics)
                {
                    List<Type> typesInTopic = typeRepository.GetAllByCondition(t => t.IsActive && t.Topic.Name == topic.Name && 
                        t.Topic.Area.Name == topic.Area.Name).ToList();
                    topic.Types = typesInTopic;
                    foreach(Type type in topic.Types)
                    {
                        List<AdditionalField> additionalsInType = additionalfieldRepository.GetAllByCondition(
                            a => a.Type.Name == type.Name && a.Type.Topic.Name == type.Topic.Name &&
                            a.Type.Topic.Area.Name == type.Topic.Area.Name).ToList();
                        type.AdditionalFields = additionalsInType;
                        foreach (AdditionalField additional in type.AdditionalFields)
                        {
                            List<AFRangeItem> rangeValues = rangeRepository.GetAllByCondition(
                                r => r.AdditionalField.Id == additional.Id).ToList();
                            additional.Range = rangeValues;
                        }
                    }
                }
                formattedAreas.Add(area);
            }
            return formattedAreas;
        }
    }
}
