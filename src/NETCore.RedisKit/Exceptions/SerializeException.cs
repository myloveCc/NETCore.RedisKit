using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.RedisKit.Exceptions
{
    /// <summary>
    /// Serizlize exception
    /// </summary>
    public class SerializeException:Exception
    {
        private string errorMsg;
        private Exception innerException;

        public SerializeException()
        {

        }

        public SerializeException(string msg) : base(msg)
        {
            this.errorMsg = msg;
        }

        public SerializeException(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            this.errorMsg = msg;
        }

        public string GetError()
        {
            return errorMsg;
        }
    }
}
