using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IMMRequest.Importer;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace IMMRequest.JSONImporter
{
    public class CustomJSONConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, System.Type objType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["ElementType"].Value<string>() == "AreaImpModel")
            {
                return jo.ToObject<AreaImpModel>(serializer);
            }
            else if (jo["ElementType"].Value<string>() == "TopicImpModel")
            {
                return jo.ToObject<TopicImpModel>(serializer);
            }
            else if (jo["ElementType"].Value<string>() == "TypeImpModel")
            {
                return jo.ToObject<TypeImpModel>(serializer);
            }
            else
            {
                return null;
            }
        }

        public override bool CanConvert(System.Type objType)
        {
            return (objType == typeof(AreaImpModel) || objType == typeof(TopicImpModel) ||
                objType == typeof(TypeImpModel));
        }
    }
}
