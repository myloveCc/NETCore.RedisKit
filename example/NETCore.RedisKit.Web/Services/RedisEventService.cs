using System;
using System.Net;
using NETCore.RedisKit.Services;

namespace NETCore.RedisKit.Web
{
    public class RedisEventService:ICustomRedisEventService
    {
        public void RedisConfigurationChanged(EndPoint point)
        {
            var address = point.ToString();
        }

        public void RedisConfigurationChangedBroadcast(EndPoint point)
        {
            var address = point.ToString();
        }

        public void RedisConnectionFailed(string connectionType, EndPoint point, Exception innerEx, string failureType)
        {
            var conType = connectionType;
        }

        public void RedisConnectionRestored(string connectionType, EndPoint point, Exception innerEx, string failureType)
        {
            var conType = connectionType;
        }

        public void RedisHashSlotMoved(EndPoint oldPoint, EndPoint newPoint)
        {
            var oldAddress = oldPoint.ToString();
            var newAddress = newPoint.ToString();
        }

        public void RedisInternalError(EndPoint point, Exception innerEx, string origin)
        {
            var address = point.ToString();
        }
    }
}
