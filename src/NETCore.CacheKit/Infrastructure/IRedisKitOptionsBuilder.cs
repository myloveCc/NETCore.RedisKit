using Microsoft.Extensions.DependencyInjection;
using NETCore.RedisKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Infrastructure
{
    public interface IRedisKitOptionsBuilder
    {
        /// <summary>
        /// service collection
        /// </summary>
        IServiceCollection serviceCollection { get; }

        /// <summary>
        /// get redis options and add ConnectionMultiplexer to sercice collection
        /// </summary>
        /// <param name="options">redis options</param>
        /// <param name="lifetime"><see cref="ServiceLifetime"/></param>
        /// <returns></returns>
        IRedisKitOptionsBuilder UseRedis(IRedisKitOptions options, ServiceLifetime lifetime = ServiceLifetime.Scoped);
    }
}
