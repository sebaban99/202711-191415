using IMMRequest.BusinessLogic.Interfaces;
using IMMRequest.Domain;
using IMMRequest.Importer;
using System;
using System.Collections.Generic;
using System.Text;
using Type = IMMRequest.Domain.Type;

namespace IMMRequest.BusinessLogic
{
    public class ImpElementParser : IImpElementParser
    {
        public List<Area> ParseElements(List<AreaImpModel> areaImpModels)
        {
            List<Area> realAreas = new List<Area>();
            foreach (AreaImpModel areaImp in areaImpModels)
            {
                Area realArea = new Area();
                realArea.Name = areaImp.Name;
                if (areaImp.Topics != null)
                {
                    foreach (TopicImpModel topicImp in areaImp.Topics)
                    {
                        Topic realTopic = new Topic();
                        realTopic.Name = topicImp.Name;
                        realTopic.Area = realArea;
                        if(topicImp.Types != null)
                        {
                            foreach (TypeImpModel typeImp in topicImp.Types)
                            {
                                Type realType = new Type();
                                realType.AdditionalFields = new List<AdditionalField>();
                                realType.Name = typeImp.Name;
                                realType.Topic = realTopic;
                                realTopic.Types.Add(realType);
                            }
                            realArea.Topics.Add(realTopic);
                        }
                    }
                    realAreas.Add(realArea);
                }
            }
            return realAreas;
        }
    }
}
