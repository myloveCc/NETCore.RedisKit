using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Pipeline extension methods for adding rediskit
    /// </summary>
    public static class RedisKitApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds RedisKit to the pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        //public static IApplicationBuilder UseRedisKit(this IApplicationBuilder app)
        //{
        //    app.Validate();
        //    return app;
        //}

        internal static void Validate(this IApplicationBuilder app)
        {
            ILoggerFactory loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            ILogger logger = loggerFactory.CreateLogger("RedisKit.Startup");

            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                //TODO
            }
        }
    }
}
