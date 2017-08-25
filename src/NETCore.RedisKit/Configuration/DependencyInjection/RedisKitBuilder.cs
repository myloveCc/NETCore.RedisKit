using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NETCore.RedisKit.Shared;
using NETCore.RedisKit.Loging;
using NETCore.RedisKit.Infrastructure;

namespace NETCore.RedisKit.Configuration
{
    public class RedisKitBuilder : IRedisKitBuilder
    {
        /// <summary>
        ///  Gets the services.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisKitBuilder"/> class.
        /// </summary>
        /// <param name="services">service collection</param>
        public RedisKitBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
}
