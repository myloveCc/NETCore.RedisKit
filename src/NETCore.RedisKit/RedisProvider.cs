using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using NETCore.RedisKit.Infrastructure.Internal;
using System.Linq;

namespace NETCore.RedisKit
{
    public class RedisProvider : IRedisProvider
    {
        /// <summary>
        /// rediskit options
        /// </summary>
        private readonly RedisKitOptions _RedisKitOptions;

        /// <summary>
        /// exchange redis config options
        /// </summary>
        private readonly ConfigurationOptions _ConfigurationOptions;

        /// <summary>
        /// is show redis service log
        /// </summary>
        public bool IsShowLog { get; private set; }

        #region ctor
        public RedisProvider(RedisKitOptions options, bool isShowLog)
        {
            _RedisKitOptions = options;
            IsShowLog = isShowLog;

            //init exchange redis config options
            _ConfigurationOptions = InitConfigurationOptions();
        }

        #endregion

        #region ConnectionMultiplexer

        //Redis实例
        public ConnectionMultiplexer Redis
        {
            get
            {
                return lazyConnection().Value;
            }
        }



        private Lazy<ConnectionMultiplexer> lazyConnection()
        {
            return new Lazy<ConnectionMultiplexer>(() =>
             {
                 //可以配置日志路径
                 return ConnectionMultiplexer.Connect(_ConfigurationOptions);
             });
        }

        #endregion

        #region Config
        /// <summary>
        /// 初始化配置信息
        /// </summary>
        /// <returns></returns>
        private ConfigurationOptions InitConfigurationOptions()
        {
            ConfigurationOptions config = new ConfigurationOptions
            {
                CommandMap = CommandMap.Create(new HashSet<string>
                { // EXCLUDE a few commands
                    "INFO", "CONFIG", "CLUSTER",
                    "PING", "ECHO", "CLIENT"
                }, available: false),
                //重试连接
                ConnectRetry = _RedisKitOptions.ConnectRetry,
                //心跳间隔
                KeepAlive = _RedisKitOptions.KeepAlive,
                //版本号
                DefaultVersion = new Version(_RedisKitOptions.DefaultVersion),
                //连接超时
                ConnectTimeout = _RedisKitOptions.ConnectTimeout,
                //异步超时
                SyncTimeout = _RedisKitOptions.SyncTimeout,
                //密码
                Password = _RedisKitOptions.Password
            };

            //添加终结点
            var endPoints = SplitEndPoint(_RedisKitOptions.EndPoints, ",");
            foreach (var item in endPoints)
            {
                config.EndPoints.Add(item);
            }
            return config;
        }

        /// <summary>
        /// Redis连接终结点集合
        /// </summary>
        /// <param name="endpoints">终结点集合字符串</param>
        /// <param name="split">分割符</param>
        /// <returns></returns>
        private IEnumerable<string> SplitEndPoint(string endpoints, string split)
        {
            var endpointsArray = endpoints.Split(split.ToArray());

            if (endpointsArray.Any())
            {
                return endpointsArray.ToList();
            }
            else
            {
                throw new Exception("Can't found any EndPoints in RedisKitOptions");
            }
        }

        #endregion
    }
}
