using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using NETCore.RedisKit.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IRedisKitBuilder
    {
        /// <summary>
        /// service collection
        /// </summary>
        IServiceCollection Services { get; }
    }
}
