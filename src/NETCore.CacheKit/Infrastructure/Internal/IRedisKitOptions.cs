using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Infrastructure.Internal
{
    public interface IRedisKitOptions
    {
        /// <summary>
        /// Redis连接终结点，多个终结点直接使用","隔开
        /// </summary>
        string EndPoints { get; set; }

        /// <summary>
        /// 连接失败，重试连接次数
        /// </summary>
        int ConnectRetry { get; set; }

        /// <summary>
        /// 心跳包间隔
        /// </summary>
        int KeepAlive { get; set; }

        /// <summary>
        /// Redis版本
        /// </summary>
        string Version { get; set; } 

        /// <summary>
        /// 连接超时时间
        /// </summary>
        int ConnectTimeout { get; set; }

        /// <summary>
        /// 异步超时时间
        /// </summary>
        int SyncTimeout { get; set; }

        /// <summary>
        /// Redis连接密码
        /// </summary>
        string Password { get; set; }

    }
}
