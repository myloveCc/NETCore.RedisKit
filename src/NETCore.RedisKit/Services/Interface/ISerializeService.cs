using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Services
{
    public interface ISerializeService
    {
        /// <summary>
        /// Object serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">Serialize value</param>
        /// <returns>Serialized string</returns>
        string ObjectSerialize<T>(T val);

        /// <summary>
        /// Object deserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">Serialized string</param>
        /// <returns>Object</returns>
        T ObjectDserialize<T>(string val);

    }
}
