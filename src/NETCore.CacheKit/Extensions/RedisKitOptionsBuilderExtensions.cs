using Microsoft.Extensions.DependencyInjection;
using NETCore.RedisKit.Infrastructure;
using NETCore.RedisKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using NETCore.RedisKit.Shared;

namespace NETCore.RedisKit.Extensions
{
    internal static class RedisKitOptionsBuilderExtensions
    {
        public static IRedisKitOptionsBuilder UseRedis(IRedisKitOptionsBuilder builder, RedisKitOptions options, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Guard.ArgumentNotNull(builder, nameof(builder));
            Guard.ArgumentNotNull(options, nameof(options));

            return builder.UseRedis(options, lifetime);
        }
    }
}
