using NETCore.RedisKit.Loging;
using NETCore.RedisKit;
using Microsoft.Extensions.Logging;
using Xunit;
using StackExchange.Redis;
using System.Threading.Tasks;
using NETCore.RedisKit.Infrastructure;
using NETCore.RedisKit.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using NETCore.RedisKit.Services;

namespace NETCore.RedisKit.Tests
{
    public class _RedisService_String_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_String_Tests()
        {
            IRedisLogger logger = new RedisLogger(new LoggerFactory(), new RedisKitOptions() { IsShowLog = false });

            _RedisService = new RedisService(CommonManager.Instance._RedisProvider, logger, new DefaultJosnSerializeService());
        }

        [Fact(DisplayName = "设置String值")]
        public async Task StringSetAsyncTest()
        {
            var test_key = "test_set";
            var setResult = await _RedisService.ItemSetAsync(test_key, "11111");

            Assert.True(setResult);

            var getValue = _RedisService.ItemGetAsync<string>(test_key).Result;
            Assert.NotEmpty(getValue);
            Assert.NotNull(getValue);
            Assert.Equal("11111", getValue);

            await _RedisService.ItemSetAsync(test_key, "22222");
            getValue = _RedisService.ItemGetAsync<string>(test_key).Result;
            Assert.Equal("22222", getValue);

            var delResult = _RedisService.ItemRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "设置String值，并指定过期时间点")]
        public async Task StringSetAsyncExpireAtTest()
        {
            var test_key = "test_set_expireat";
            var setResult = await _RedisService.ItemSetAsync(test_key, "11111", DateTime.Now.AddSeconds(5));
            Assert.True(setResult);

            var getValue = Task<string>.Factory.StartNew(() =>
            {
                Task.Delay(new TimeSpan(0, 0, 6)).Wait();
                return _RedisService.ItemGetAsync<string>(test_key).Result;
            }).Result;
            Assert.Null(getValue);
        }


        [Fact(DisplayName = "设置String值,并指定过期时间段")]
        public void StringSetAsyncExpireInTest()
        {
            var test_key = "test_set_expirein";
            var setResult = _RedisService.ItemSetAsync(test_key, "11111", new TimeSpan(0, 0, 5)).Result;
            Assert.True(setResult);

            var getValue = Task<string>.Factory.StartNew(() =>
            {
                Task.Delay(new TimeSpan(0, 0, 6)).Wait();
                return _RedisService.ItemGetAsync<string>(test_key).Result;
            }).Result;
            Assert.Null(getValue);
        }

        [Fact(DisplayName = "根据单个key获取String值")]
        public async Task StringGetAsyncTest()
        {
            var test_key = "test_get";

            var getValue = await _RedisService.ItemGetAsync<string>(test_key);
            Assert.Null(getValue);

            var setResult = _RedisService.ItemSetAsync(test_key, "1111").Result;
            Assert.True(setResult);

            getValue = _RedisService.ItemGetAsync<string>(test_key).Result;
            Assert.NotNull(getValue);
            Assert.NotEmpty(getValue);
            Assert.Equal("1111", getValue);

            var delResult = _RedisService.ItemRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "根据Key集合获取多个值")]
        public async Task StringGetAsyncRangeTest()
        {
            var test_key = new List<RedisKey>() { "key1", "key2", "key3", "key4" };

            await _RedisService.ItemSetAsync("key1", "1111");
            await _RedisService.ItemSetAsync("key2", "2222");
            await _RedisService.ItemSetAsync("key3", "3333");

            var values = (await _RedisService.ItemGetAsync<string>(test_key)).ToList();

            Assert.Equal("1111", values[0]);
            Assert.Equal("2222", values[1]);
            Assert.Equal("3333", values[2]);

            await _RedisService.ItemRemoveAsync(test_key);
        }

        [Fact(DisplayName = "根据Key移除缓存")]
        public void StringRemoveAsyncTest()
        {
            var test_key = "test_remove";
            var delResult = _RedisService.ItemRemoveAsync(test_key).Result;

            Assert.False(delResult);

            var result = _RedisService.ItemSetAsync(test_key, "11111").Result;
            delResult = _RedisService.ItemRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "根据Key集合移除缓存")]
        public async Task StringRemoveAsyncRangeTest()
        {
            var test_key = new List<RedisKey>() { "key1", "key2", "key3", "key4" };

            await _RedisService.ItemSetAsync("key1", "1111");
            await _RedisService.ItemSetAsync("key2", "2222");
            await _RedisService.ItemSetAsync("key3", "3333");

            var delResult = await _RedisService.ItemRemoveAsync(test_key);

            Assert.Equal(3, delResult);
        }
    }
}
