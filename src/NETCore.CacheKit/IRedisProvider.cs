using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit
{
    public interface IRedisProvider
    {
        bool IsShowLog { get; }


        ConnectionMultiplexer Redis { get; }
    }
}
