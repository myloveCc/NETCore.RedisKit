using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NETCore.RedisKit.Configuration;
using NETCore.RedisKit.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering core services
    /// </summary>
    public static class RedisKitBuilderExtensionsCore
    {
        /// <summary>
        /// Adds the required platform services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IRedisKitBuilder AddRequiredPlatformServices(this IRedisKitBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<RedisKitOptions>>().Value);

            return builder;
        }
    }
}
