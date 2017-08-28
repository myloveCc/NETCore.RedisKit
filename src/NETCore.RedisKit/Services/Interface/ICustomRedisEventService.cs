using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using StackExchange.Redis;

namespace NETCore.RedisKit.Services
{
    public interface ICustomRedisEventService
    {
        /// <summary>
        /// Raised when nodes are explicitly requested to reconfigure via broadcast; this usually means master/slave changes
        /// </summary>
        /// <param name="point"></param>
        void RedisConfigurationChangedBroadcast(EndPoint point);

        /// <summary>
        /// Raised when a hash-slot has been relocated
        /// </summary>
        /// <param name="oldPoint"></param>
        /// <param name="newPoint"></param>
        void RedisHashSlotMoved(EndPoint oldPoint, EndPoint newPoint);

        /// <summary>
        ///   Redis internal error 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="innerEx"></param>
        /// <param name="origin"></param>
        void RedisInternalError(EndPoint point, Exception innerEx, string origin);

        /// <summary>
        ///  Redis connection failded 
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="point"></param>
        /// <param name="innerEx"></param>
        /// <param name="failureType"></param>
        void RedisConnectionFailed(string connectionType, EndPoint point, Exception innerEx, string failureType);

        /// <summary>
        /// Raised whenever a physical connection is established
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="point"></param>
        /// <param name="innerEx"></param>
        /// <param name="failureType"></param>
        void RedisConnectionRestored(string connectionType, EndPoint point, Exception innerEx, string failureType);

        /// <summary>
        /// Redis configuration changed
        /// </summary>
        /// <param name="point"></param>
        void RedisConfigurationChanged(EndPoint point);
    }
}
