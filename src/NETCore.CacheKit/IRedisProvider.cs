using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit
{
    public interface IRedisProvider
    {
        ConnectionMultiplexer Redis { get; }
    }
}
