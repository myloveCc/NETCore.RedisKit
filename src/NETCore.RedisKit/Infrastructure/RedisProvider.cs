using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NETCore.RedisKit.Configuration;

namespace NETCore.RedisKit.Infrastructure
{
    public class RedisProvider:IRedisProvider
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
        public RedisProvider(RedisKitOptions options)
        {
            _RedisKitOptions = options;
            IsShowLog = options.IsShowLog;

            //init exchange redis config options
            _ConfigurationOptions = InitConfigurationOptions();
        }

        #endregion

        #region ConnectionMultiplexer

        /// <summary>
        /// Redis conection
        /// </summary>
        public ConnectionMultiplexer Redis
        {
            get
            {
                return lazyConnection().Value;
            }
        }

        /// <summary>
        /// Lazy load
        /// </summary>
        /// <returns></returns>
        private Lazy<ConnectionMultiplexer> lazyConnection()
        {
            return new Lazy<ConnectionMultiplexer>(() =>
             {
                 return ConnectionMultiplexer.Connect(_ConfigurationOptions);
             });
        }

        #endregion

        #region Config
        /// <summary>
        /// Init configuration
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
                AbortOnConnectFail = _RedisKitOptions.AbortOnConnectFail,
                AllowAdmin = _RedisKitOptions.AllowAdmin,
                ChannelPrefix = _RedisKitOptions.ChannelPrefix,
                ConnectRetry = _RedisKitOptions.ConnectRetry,
                ConnectTimeout = _RedisKitOptions.ConnectTimeout,
                ConfigurationChannel = _RedisKitOptions.ConfigurationChannel,
                DefaultDatabase = _RedisKitOptions.DefaultDatabase,
                KeepAlive = _RedisKitOptions.KeepAlive,
                ClientName = _RedisKitOptions.ClientName,
                Password = _RedisKitOptions.Password,
                Proxy = _RedisKitOptions.Proxy,
                ResolveDns = _RedisKitOptions.ResolveDns,
                ServiceName = _RedisKitOptions.ServiceName,
                Ssl = _RedisKitOptions.Ssl,
                SslHost = _RedisKitOptions.SslHost,
                SslProtocols = _RedisKitOptions.SslProtocols,
                SyncTimeout = _RedisKitOptions.SyncTimeout,
                TieBreaker = _RedisKitOptions.TieBreaker,
                DefaultVersion = new Version(_RedisKitOptions.DefaultVersion),
                WriteBuffer = _RedisKitOptions.WriteBuffer
            };

            //add endpoints
            var endPoints = SplitEndPoint(_RedisKitOptions.EndPoints, ",");
            foreach (var item in endPoints)
            {
                config.EndPoints.Add(item);
            }
            return config;
        }

        /// <summary>
        /// split endpotint
        /// </summary>
        /// <param name="endpoints"> endpoint string</param>
        /// <param name="split">split char</param>
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
