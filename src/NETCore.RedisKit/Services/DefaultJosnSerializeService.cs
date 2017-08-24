using System;
using System.Collections.Generic;
using System.Text;
using NETCore.RedisKit.Exceptions;
using Newtonsoft.Json;

namespace NETCore.RedisKit
{
    public class DefaultJosnSerializeService:ISerializeService
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="value">序列化对象</param>
        /// <returns>序列化之后的json字符串</returns>
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
                throw new SerializeException("default json service serialize error", ex);
            }
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="value">json字符串</param>
        /// <returns>反序列化之后的对象</returns>
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

                throw new SerializeException("default json service dserialize error", ex);
            }
           
        }
    }
}
