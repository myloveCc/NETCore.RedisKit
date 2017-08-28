using System;
using Microsoft.Extensions.Logging;
using Moq;
using NETCore.RedisKit.Configuration;
using NETCore.RedisKit.Infrastructure;

namespace NETCore.RedisKit.Tests
{
    public class CommonManager
    {
        private readonly Mock<IServiceProvider> _ServiceProvider = new Mock<IServiceProvider>();
        private readonly Mock<ILogger<RedisProvider>> _Logger = new Mock<ILogger<RedisProvider>>();
        public IRedisProvider _RedisProvider { get; }

        public static readonly CommonManager Instance = new CommonManager();
        private CommonManager()
        {
            _RedisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            }, _ServiceProvider.Object, _Logger.Object);
        }
    }
}
