using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NETCore.RedisKit;
using NETCore.RedisKit.Configuration;
using NETCore.RedisKit.Loging;
using NETCore.RedisKit.Infrastructure;
using NETCore.RedisKit.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisKitServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setUpAction">The setup action.</param>
        /// <param name="isShowLog">Is show log</param>
        /// <returns></returns>
        public static IRedisKitBuilder AddRedisKit(this IServiceCollection services, Action<RedisKitOptions> setUpAction)
        {
            var builder = new RedisKitBuilder(services);

            builder.Services.TryAddScoped<IRedisLogger, RedisLogger>();
            builder.Services.TryAddScoped<IRedisService, RedisService>();
            builder.Services.TryAddScoped<ISerializeService, DefaultJosnSerializeService>();

            builder.Services.Configure(setUpAction);

            builder.Services.TryAddSingleton<IRedisProvider, RedisProvider>();

            //add platform require servcies
            builder.AddRequiredPlatformServices();

            return builder;
        }
    }
}
