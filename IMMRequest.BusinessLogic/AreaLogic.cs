using System;
using System.Collections.Generic;
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

        public AreaLogic(IRepository<Area> areaRespository, IRepository<Topic> topicRepository, ITypeRepository typeRepository)
        {
            this.areaRepository = areaRespository;
            this.topicRepository = topicRepository;
            this.typeRepository = typeRepository;
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
            List<Area> areasInDB = (List<Area>)areaRepository.GetAll();
            foreach(Area area in areasInDB)
            {
                List<Topic> areaTopics = (List<Topic>)topicRepository.GetAllByCondition(t => t.Area.Name == area.Name);
                area.Topics = areaTopics;
                foreach(Topic topic in area.Topics)
                {
                    List<Type> typesInTopic = (List<Type>)typeRepository.GetAllByCondition(t => t.Topic.Name == topic.Name && 
                        t.Topic.Area.Name == topic.Area.Name);
                    topic.Types = typesInTopic;
                }
                areasInDB.Add(area);
            }
            return areasInDB;
        }
    }
}
