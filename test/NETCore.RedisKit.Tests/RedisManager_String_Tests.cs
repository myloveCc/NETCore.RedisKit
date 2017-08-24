using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using StackExchange.Redis;
using NETCore.RedisKit.Core.Internal;
using NETCore.RedisKit.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using NETCore.RedisKit.Core;

namespace NETCore.RedisKit.Tests
{
    public class _RedisService_String_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_String_Tests()
        {
            IRedisProvider redisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            }, true);


            IRedisKitLogger logger = new RedisKitLogger(new LoggerFactory(), redisProvider);

            _RedisService = new RedisService(redisProvider, logger);
        }

        [Fact(DisplayName = "设置String值")]
        public async Task StringSetAsyncTest()
        {
            var test_key = "test_set";
            var setResult = await _RedisService.StringSetAsync(test_key, "11111");

            Assert.True(setResult);

            var getValue = _RedisService.StringGetAsync<string>(test_key).Result;
            Assert.NotEmpty(getValue);
            Assert.NotNull(getValue);
            Assert.Equal("11111", getValue);

            await _RedisService.StringSetAsync(test_key, "22222");
            getValue = _RedisService.StringGetAsync<string>(test_key).Result;
            Assert.Equal("22222", getValue);

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "设置String值，并指定过期时间点")]
        public void StringSetAsyncExpireAtTest()
        {
            var test_key = "test_set_expireat";
            var setResult = _RedisService.StringSetAsync(test_key, "11111", DateTime.Now.AddSeconds(5)).Result;
            Assert.True(setResult);

            var getValue = Task<string>.Factory.StartNew(() =>
            {
                Task.Delay(new TimeSpan(0, 0, 6)).Wait();
                return _RedisService.StringGetAsync<string>(test_key).Result;
            }).Result;
            Assert.Null(getValue);
        }


        [Fact(DisplayName = "设置String值,并指定过期时间段")]
        public void StringSetAsyncExpireInTest()
        {
            var test_key = "test_set_expirein";
            var setResult = _RedisService.StringSetAsync(test_key, "11111", new TimeSpan(0, 0, 5)).Result;
            Assert.True(setResult);

            var getValue = Task<string>.Factory.StartNew(() =>
            {
                Task.Delay(new TimeSpan(0, 0, 6)).Wait();
                return _RedisService.StringGetAsync<string>(test_key).Result;
            }).Result;
            Assert.Null(getValue);
        }

        [Fact(DisplayName = "根据单个key获取String值")]
        public void StringGetAsyncTest()
        {
            var test_key = "test_get";

            var getValue = _RedisService.StringGetAsync<string>(test_key).Result;
            Assert.Null(getValue);

            var setResult = _RedisService.StringSetAsync(test_key, "1111").Result;
            Assert.True(setResult);

            getValue = _RedisService.StringGetAsync<string>(test_key).Result;
            Assert.NotNull(getValue);
            Assert.NotEmpty(getValue);
            Assert.Equal(getValue, "1111");

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "根据Key集合获取多个值")]
        public void StringGetAsyncRangeTest()
        {
            var test_key = new List<RedisKey>() { "key1", "key2", "key3", "key4" };

            _RedisService.StringSetAsync("key1", "1111");
            _RedisService.StringSetAsync("key2", "2222");
            _RedisService.StringSetAsync("key3", "3333");

            var values = _RedisService.StringGetAsync<string>(test_key).Result.ToList();

            Assert.Equal("1111", values[0]);
            Assert.Equal("2222", values[1]);
            Assert.Equal("3333", values[2]);

            _RedisService.StringRemoveAsync(test_key);
        }

        [Fact(DisplayName = "根据Key移除缓存")]
        public void StringRemoveAsyncTest()
        {
            var test_key = "test_remove";
            var delResult = _RedisService.StringRemoveAsync(test_key).Result;

            Assert.False(delResult);

            var result = _RedisService.StringSetAsync(test_key, "11111").Result;
            delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "根据Key集合移除缓存")]
        public async Task StringRemoveAsyncRangeTest()
        {
            var test_key = new List<RedisKey>() { "key1", "key2", "key3", "key4" };

            await _RedisService.StringSetAsync("key1", "1111");
            await _RedisService.StringSetAsync("key2", "2222");
            await _RedisService.StringSetAsync("key3", "3333");

            var delResult = await _RedisService.StringRemoveAsync(test_key);

            Assert.Equal(3, delResult);
        }
    }
}
