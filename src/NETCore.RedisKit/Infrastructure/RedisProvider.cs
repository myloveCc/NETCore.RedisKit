using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using NETCore.RedisKit.Configuration;
using NETCore.RedisKit.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace NETCore.RedisKit.Infrastructure
{
    public class RedisProvider:IRedisProvider
    {
        /// <summary>
        /// Rediskit options
        /// </summary>
        private readonly RedisKitOptions _RedisKitOptions;

        /// <summary>
        /// Exchange redis config options
        /// </summary>
        private readonly ConfigurationOptions _ConfigurationOptions;

        /// <summary>
        /// Redis connectionMultiplexer
        /// </summary>
        private ConnectionMultiplexer _ConnectionMultiplexer;

        /// <summary>
        /// Custom redis event service
        /// </summary>
        private readonly ICustomRedisEventService _CustomRedisEventService;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _Logger;

        #region ctor
        public RedisProvider(RedisKitOptions options, IServiceProvider serviceProvider, ILogger<RedisProvider> _logger)
        {
            _RedisKitOptions = options;
            //init exchange redis config options
            _ConfigurationOptions = InitConfigurationOptions();

            _CustomRedisEventService = serviceProvider.GetService<ICustomRedisEventService>();
            _Logger = _logger;
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
                if (_ConnectionMultiplexer == null)
                {
                    _ConnectionMultiplexer = lazyConnection().Value;

                    _ConnectionMultiplexer.ConfigurationChangedBroadcast += Redis_ConfigurationChangedBroadcast;
                    _ConnectionMultiplexer.HashSlotMoved += Redis_HashSlotMoved;
                    _ConnectionMultiplexer.ConfigurationChanged += Redis_ConfigurationChanged;
                    _ConnectionMultiplexer.ConnectionFailed += Redis_ConnectionFailed;
                    _ConnectionMultiplexer.ConnectionRestored += Redis_ConnectionRestored;
                    _ConnectionMultiplexer.InternalError += Redis_InternalError;
                }

                if (!_ConnectionMultiplexer.IsConnected)
                {
                    throw new Exception("Redis can't connect to server");
                }

                return _ConnectionMultiplexer;
            }
        }

        /// <summary>
        /// Raised when nodes are explicitly requested to reconfigure via broadcast; this usually means master/slave changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redis_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
            _Logger.LogInformation($"Redis configuration changed broadcast");

            if (_CustomRedisEventService != null)
            {
                _CustomRedisEventService.RedisConfigurationChangedBroadcast(e.EndPoint);
            }
        }

        /// <summary>
        /// Raised when a hash-slot has been relocated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redis_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            _Logger.LogInformation($"Redis hash slot moved");

            if (_CustomRedisEventService != null)
            {
                _CustomRedisEventService.RedisHashSlotMoved(e.OldEndPoint, e.NewEndPoint);
            }
        }

        /// <summary>
        /// Redis internal error event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redis_InternalError(object sender, InternalErrorEventArgs e)
        {
            _Logger.LogError($"Redis internal error");

            if (_CustomRedisEventService != null)
            {
                _CustomRedisEventService.RedisInternalError(e.EndPoint, e.Exception, e.Origin);
            }
        }

        /// <summary>
        /// Redis connection failded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Redis_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            _Logger.LogError($"Redis connection failed");

            if (_CustomRedisEventService != null)
            {
                _CustomRedisEventService.RedisConnectionFailed(e.ConnectionType.ToString(), e.EndPoint, e.Exception, e.FailureType.ToString());
            }
        }

        /// <summary>
        ///  Raised whenever a physical connection is established(重连)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redis_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            _Logger.LogInformation($"Redis connection restored");

            if (_CustomRedisEventService != null)
            {
                _CustomRedisEventService.RedisConnectionRestored(e.ConnectionType.ToString(), e.EndPoint, e.Exception, e.FailureType.ToString());
            }
        }

        /// <summary>
        /// Redis configuration changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redis_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            _Logger.LogInformation($"Redis cofiguration changed");

            if (_CustomRedisEventService != null)
            {
                _CustomRedisEventService.RedisConfigurationChanged(e.EndPoint);
            }
        }

        /// <summary>
        /// Lazy load
        /// </summary>
        /// <returns></returns>
        private Lazy<ConnectionMultiplexer> lazyConnection()
        {
            //TODO 连接失败需要处理

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
