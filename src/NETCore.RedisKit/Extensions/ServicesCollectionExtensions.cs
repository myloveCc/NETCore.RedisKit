using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NETCore.RedisKit.Shared;
using NETCore.RedisKit.Infrastructure;

namespace NETCore.RedisKit.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddRedisKit(this IServiceCollection serviceCollection, Action<RedisKitOptionsBuilder> optionsAction)
        {
            Guard.ArgumentNotNull(serviceCollection, nameof(serviceCollection));
            Guard.ArgumentNotNull(optionsAction, nameof(optionsAction));
      
            optionsAction.Invoke(new RedisKitOptionsBuilder(serviceCollection));

            return serviceCollection;
        }
    }
}
