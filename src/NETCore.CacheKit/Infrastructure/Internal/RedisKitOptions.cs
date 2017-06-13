using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Infrastructure.Internal
{
    /// <summary>
    /// redis options
    /// </summary>
    public class RedisKitOptions : IRedisKitOptions
    {
        /// <summary>
        /// ctor
        /// </summary>
        public RedisKitOptions()
        {

        }

        /// <summary>
        /// endpoints ,much split with ","
        /// </summary>
        public virtual string EndPoints { get; set; }

        /// <summary>
        /// connect retry, default is 3
        /// </summary>
        public virtual int ConnectRetry { get; set; } = 3;

        /// <summary>
        /// keep alive ,default is 180 miniseconds
        /// </summary>
        public virtual int KeepAlive { get; set; } = 180;

        /// <summary>
        /// redis version
        /// </summary>
        public virtual string Version { get; set; } = "3.0.5";

        /// <summary>
        /// time out ,defalult is 200 miniseconds
        /// </summary>
        public virtual int ConnectTimeout { get; set; } = 1000;

        /// <summary>
        /// async time out ,defalult is 200 miniseconds
        /// </summary>
        public virtual int SyncTimeout { get; set; } = 1000;

        /// <summary>
        /// redis password 
        /// </summary>
        public virtual string Password { get; set; } = "";
    }
}
