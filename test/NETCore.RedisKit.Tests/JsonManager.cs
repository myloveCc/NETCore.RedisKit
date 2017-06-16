using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NETCore.RedisKit.Tests
{
    public static class JsonManager
    {
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
