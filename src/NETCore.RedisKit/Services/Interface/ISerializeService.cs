using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Services
{
    public interface ISerializeService
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="value">序列化对象</param>
        /// <returns>序列化之后的字符串</returns>
        string ObjectSerialize<T>(T val);

        /// <summary>
        /// 对象反序列化
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="value">序列化字符串</param>
        /// <returns>反序列化之后的对象</returns>
        T ObjectDserialize<T>(string val);

    }
}
