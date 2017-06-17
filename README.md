# NETCore.RedisKit
[StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) extensions. Easy use redis in your asp.net core project.

# Install with nuget

To install NETCore.RedisKit, run the following command in the [Package Manager Console](https://docs.microsoft.com/zh-cn/nuget/tools/package-manager-console)

```
Install-Package NETCore.RedisKit
```
---

# RedisKit Options 


| Configuration string      | ConfigurationOptions    | Default           | Meaning                   |
| :------------------------ | :---------------------- | :---------------- | :------------------------ |
| `endPoints={string}`      |`EndPoints`              | --                | Redis server endPoint string,multi endPoint split with ',' like '127.0.0.1:6379,127.0.0.1:6380'|
| `abortConnect={bool}`     | `AbortOnConnectFail`    | `true`（`false` on Azure） | If true, Connect will not create a connection while no servers are available |
| `allowAdmin={bool}`       | `AllowAdmin`	          | `false`            | Enables a range of commands that are considered risky|
| `channelPrefix={string}`  | `ChannelPrefix`         | `null`             | Optional channel prefix for all pub/sub operations |
| `connectRetry={int}`      |	`ConnectRetry`          |	`3`                | The number of times to repeat connect attempts during initial `Connect` |
| `connectTimeout={int}`    | `ConnectTimeout`	      | `1000`             | Timeout (ms) for connect operations |
| `configChannel={string}`  |	`ConfigurationChannel`   |	`__Booksleeve_MasterChanged` | Broadcast channel name for communicating configuration changes |
| `defaultDatabase={int}`   |	`DefaultDatabase`       |	`0`             | Default database index, from `0` to databases `- 1`|
| `keepAlive={int}`         |	`KeepAlive`             |	`-1	`              | Time (seconds) at which to send a message to help keep sockets alive |
| `name={string}`           |	`ClientName`            |	`null`             | Identification for the connection within redis |
| `password={string}`       |	`Password`              |	`null`             | Password for the redis server |
| `proxy={proxy type}`      |	`Proxy`                 |	`Proxy.None`       | Type of proxy in use (if any); for example “twemproxy” |
| `resolveDns={bool}`       |	`ResolveDns`            |	`false`            | Specifies that DNS resolution should be explicit and eager, rather than implicit |
| `serviceName={string}`	  | `ServiceName`           |	`null`             | Not currently implemented (intended for use with sentinel) |
| `ssl={bool}`              |	`Ssl`                   |	`false`            | Specifies that SSL encryption should be used |
| `sslHost={string}`        | `SslHost`               |	`null`             | Enforces a particular SSL host identity on the server’s certificate |
| `sslProtocols={enum}`	   | `SslProtocols`           |	`SslProtocols.None`| Ssl/Tls versions supported when using an encrypted connection. Use ‘\|’ to provide multiple values. |
| `syncTimeout={int}`      | `SyncTimeout`            |	`1000`             | Time (ms) to allow for synchronous operations |
| `tiebreaker={string}`    | `TieBreaker1`            |	`__Booksleeve_TieBreak` |	Key to use for selecting a server in an ambiguous master scenario  |
| `version={string}`       | `DefaultVersion`         |	`(3.0 in Azure, else 2.0)` | Redis version level (useful when the server does not make this available) |
| `writeBuffer={int}`      |	`WriteBuffer`           |	`4096`             |	Size of the output buffer |


- You can find more information about[Exchange.Redis Configration](https://stackexchange.github.io/StackExchange.Redis/Configuration)

# How to use

## Add RedisKit in startup like 

```csharp
public void ConfigureServices(IServiceCollection services)
{
  // Add framework services.
  services.AddMvc();

  services.AddRedisKit(optionsBuilder =>
  {
      optionsBuilder.UseRedis(
        options: new RedisKitOptions()
        {
            EndPoints = "127.0.0.1:6379"
        },
        isShowLog: true);
  });
}

```

## Use RedisService like 

```csharp
private readonly IRedisService _RedisService;
public HomeController(IRedisService redisService)
{
    _RedisService = redisService;
}

public IActionResult Index()
{
    _RedisService.StringSet("hello", "world");
    return View();
}

```

# LICENSE

MIT
