using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NETCore.RedisKit.Configuration;
using NETCore.RedisKit.Infrastructure;

namespace NETCore.RedisKit.Loging
{
    public class RedisLogger:IRedisLogger
    {
        private ILogger _Logger;
        private readonly RedisKitOptions _RedisKitOptions;
        private bool _IsShowLog = false;

        public RedisLogger(ILoggerFactory loggerFactory, RedisKitOptions redisKitOptions)
        {
            _Logger = loggerFactory.CreateLogger("NETCore.RedisKit.Logging");
            _RedisKitOptions = redisKitOptions;
            _IsShowLog = _RedisKitOptions.IsShowLog;
        }

        public void LogCritical(EventId eventId, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogCritical(eventId, message, args);
            }
        }

        public void LogCritical(EventId eventId, Exception exception, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogCritical(eventId, exception, message, args);
            }
        }

        public void LogCritical(string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogCritical(message, args);
            }
        }

        public void LogDebug(EventId eventId, Exception exception, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogDebug(eventId, exception, message, args);
            }
        }

        public void LogDebug(string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogDebug(message, args);
            }
        }

        public void LogDebug(EventId eventId, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogDebug(eventId, message, args);
            }
        }

        public void LogError(EventId eventId, Exception exception, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogError(eventId, exception, message, args);
            }
        }

        public void LogError(EventId eventId, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogError(eventId, message, args);
            }
        }

        public void LogError(string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogError(message, args);
            }
        }

        public void LogInformation(EventId eventId, Exception exception, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogInformation(eventId, exception, message, args);
            }
        }

        public void LogInformation(EventId eventId, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogInformation(eventId, message, args);
            }
        }

        public void LogInformation(string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogInformation(message, args);
            }
        }

        public void LogTrace(string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogTrace(message, args);
            }
        }

        public void LogTrace(EventId eventId, Exception exception, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogTrace(eventId, exception, message, args);
            }
        }

        public void LogTrace(EventId eventId, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogTrace(eventId, message, args);
            }
        }

        public void LogWarning(EventId eventId, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogTrace(eventId, message, args);
            }
        }

        public void LogWarning(string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogWarning(message, args);
            }
        }

        public void LogWarning(EventId eventId, Exception exception, string message, params object[] args)
        {
            if (_IsShowLog)
            {
                _Logger.LogWarning(eventId, exception, message, args);
            }
        }
    }
}
