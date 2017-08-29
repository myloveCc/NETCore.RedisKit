using System;
using System.Collections.Generic;
using System.Text;
using NETCore.RedisKit.Exceptions;
using Newtonsoft.Json;

namespace NETCore.RedisKit.Services
{
    /// <summary>
    /// Derfault json serialize service
    /// </summary>
    public class DefaultJosnSerializeService:ISerializeService
    {
        /// <summary>
        /// Object serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">Serialize value</param>
        /// <returns>Json string</returns>
        public string ObjectSerialize<T>(T val)
        {
            if (val == null)
            {
                return string.Empty;
            }

            try
            {
                return JsonConvert.SerializeObject(val);
            }
            catch (Exception ex)
            {
                throw new SerializeException("Default json service serialize error", ex);
            }
        }


        /// <summary>
        /// Object deserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">Json string</param>
        /// <returns>Object</returns>

        public T ObjectDserialize<T>(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(val);
            }
            catch (Exception ex)
            {

                throw new SerializeException("Default json service dserialize error", ex);
            }

        }
    }
}
