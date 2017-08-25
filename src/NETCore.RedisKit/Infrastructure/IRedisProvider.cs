using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Infrastructure
{
    public interface IRedisProvider
    {
        bool IsShowLog { get; }


        ConnectionMultiplexer Redis { get; }
    }
}
