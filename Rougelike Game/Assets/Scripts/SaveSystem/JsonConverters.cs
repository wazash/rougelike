using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace NewSaveSystem
{
    public class Vector3JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vector3);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            float x = jo["x"].Value<float>();
            float y = jo["y"].Value<float>();
            float z = jo["z"].Value<float>();
            return new Vector3(x, y, z);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector3 v = (Vector3)value;
            JObject jo = new()
            {
                { "x", v.x },
                { "y", v.y },
                { "z", v.z }
            };
            jo.WriteTo(writer);
        }
    }

    public class Vector2JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Vector2);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            float x = jo["x"].Value<float>();
            float y = jo["y"].Value<float>();
            return new Vector2(x, y);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector2 v = (Vector2)value;
            JObject jo = new()
            {
                { "x", v.x },
                { "y", v.y }
            };
            jo.WriteTo(writer);
        }
    }

}
