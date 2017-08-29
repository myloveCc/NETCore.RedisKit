using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using StackExchange.Redis;

namespace NETCore.RedisKit.Configuration
{
    /// <summary>
    /// redis options
    /// </summary>
    public class RedisKitOptions
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
        public string EndPoints { get; set; }

        /// <summary>
        /// Connect will not create a connection while no servers are available
        /// </summary>
        /// <value><c>true</c> if abort on connect fail; otherwise, <c>false</c>.</value>
        public bool AbortOnConnectFail { get; set; } = true;

        /// <summary>
        /// Enables a range of commands that are considered risky
        /// </summary>
        /// <value></value>
        public bool AllowAdmin { get; set; } = false;

        /// <summary>
        /// Optional channel prefix for all pub/sub operations
        /// </summary>
        /// <value>The channel prefix.</value>
        public string ChannelPrefix { get; set; } = null;
        /// <summary>
        /// connect retry, default is 3
        /// </summary>
        public int ConnectRetry { get; set; } = 3;

        /// <summary>
        /// Broadcast channel name for communicating configuration changes
        /// </summary>
        /// <value>The configuration channel.</value>
        public string ConfigurationChannel { get; set; } = "__Booksleeve_MasterChanged";

        /// <summary>
        /// time out ,defalult is 200 miniseconds
        /// </summary>
        public int ConnectTimeout { get; set; } = 200;

        /// <summary>
        /// async time out ,defalult is 200 miniseconds
        /// </summary>
        public int SyncTimeout { get; set; } = 200;

        /// <summary>
        /// Default database index, from 0 to databases - 1
        /// </summary>
        /// <value>The default database.</value>
        public int? DefaultDatabase { get; set; } = null;

        /// <summary>
        /// keep alive ,default is 180 miniseconds
        /// </summary>
        public int KeepAlive { get; set; } = 180;

        /// <summary>
        /// Identification for the connection within redis
        /// </summary>
        /// <value>The name of the client.</value>
        public string ClientName { get; set; } = null;

        /// <summary>
        /// redis password 
        /// </summary>
        public virtual string Password { get; set; } = null;

        /// <summary>
        /// Type of proxy in use (if any); for example “twemproxy”
        /// </summary>
        /// <value>The proxy.</value>
        public Proxy Proxy { get; set; } = Proxy.None;

        /// <summary>
        /// Specifies that DNS resolution should be explicit and eager, rather than implicit
        /// </summary>
        /// <value><c>true</c> if resolve dns; otherwise, <c>false</c>.</value>
        public bool ResolveDns { get; set; } = false;

        /// <summary>
        /// Not currently implemented (intended for use with sentinel)
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; } = null;

        /// <summary>
        ///Specifies that SSL encryption should be used
        /// </summary>
        /// <value><c>true</c> if ssl; otherwise, <c>false</c>.</value>
        public bool Ssl { get; set; } = false;

        /// <summary>
        /// Enforces a particular SSL host identity on the server’s certificate
        /// </summary>
        /// <value>The ssl host.</value>
        public string SslHost { get; set; } = null;

        /// <summary>
        /// Ssl/Tls versions supported when using an encrypted connection. Use ‘|’ to provide multiple values.
        /// </summary>
        /// <value>The ssl protocols.</value>
        public SslProtocols? SslProtocols { get; set; } = null;

        /// <summary>
        /// Key to use for selecting a server in an ambiguous master scenario
        /// </summary>
        /// <value>The tie breaker1.</value>
        public string TieBreaker { get; set; } = "__Booksleeve_TieBreak";

        /// <summary>
        /// redis version
        /// </summary>
        public string DefaultVersion { get; set; } = "2.0";

        /// <summary>
        /// Size of the output buffer
        /// </summary>
        /// <value>The write buffer.</value>
        public int WriteBuffer { get; set; } = 4096;

        /// <summary>
        /// Is show the redis log
        /// </summary>
        public bool IsShowLog { get; set; } = false;

    }
}
