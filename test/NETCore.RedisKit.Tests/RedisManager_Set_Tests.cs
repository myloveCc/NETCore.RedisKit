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
    public class _RedisService_Set_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_Set_Tests()
        {
            IRedisProvider redisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            }, true);


            IRedisKitLogger logger = new RedisKitLogger(new LoggerFactory(), redisProvider);

            _RedisService = new RedisService(redisProvider, logger);
        }

        [Fact(DisplayName = "新增单个值到Set集合")]
        public async Task SetAddAsyncTest()
        {
            var test_key = "test_set_add";

            await _RedisService.SetRemoveAllAsync(test_key);

            var addResult = _RedisService.SetAddAsync(test_key, "1111111").Result;
            Assert.True(addResult);

            addResult = _RedisService.SetAddAsync(test_key, "1111111").Result;
            Assert.False(addResult);

            var delResult = _RedisService.SetRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "新增集合到Set集合")]
        public async Task SetAddRanageAsyncTest()
        {
            var test_key = "test_set_add_range";
            await _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333", "11111" };
            var addResult = _RedisService.SetAddRanageAsync(test_key, values).Result;
            Assert.Equal(3, addResult);
            await _RedisService.SetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Set集合中的值")]
        public async Task SetRemoveAsyncTest()
        {
            var test_key = "test_set_remove";
            await _RedisService.SetRemoveAllAsync(test_key);

            var addResult = _RedisService.SetAddAsync(test_key, "1111111").Result;
            Assert.True(addResult);

            var removeResult = _RedisService.SetRemoveAsync<string>(test_key, "1111111").Result;
            Assert.True(removeResult);
            await _RedisService.SetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Set集合中的多个值")]
        public async Task SetRemoveRangeAsyncTest()
        {
            var test_key = "test_set_remove_range";
            await _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333", "11111" };
            var addResult = _RedisService.SetAddRanageAsync(test_key, values).Result;
            Assert.Equal(3, addResult);

            var removeRangeResult = _RedisService.SetRemoveRangeAsync<string>(test_key, new List<string>() { "11111", "22222" }).Result;
            Assert.Equal(2, removeRangeResult);

            var setCount = _RedisService.SetCountAsync(test_key).Result;
            Assert.Equal(1, setCount);

            await _RedisService.SetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Set集合全部值")]
        public async Task SetRemoveAllAsync()
        {
            var test_key = "test_set_remove_all";
            await _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333", "11111" };
            var addResult = _RedisService.SetAddRanageAsync(test_key, values).Result;
            Assert.Equal(addResult, 3);

            var delResult = _RedisService.SetRemoveAllAsync(test_key).Result;
            Assert.True(delResult);

            var setCount = _RedisService.SetCountAsync(test_key).Result;
            Assert.Equal(0, setCount);
        }

        [Fact(DisplayName = "Set集合合并(多个集合合并)")]
        public async Task SetCombineAsyncTest()
        {
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";
            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            await _RedisService.SetAddRanageAsync(test_key1, values1);
            await _RedisService.SetAddRanageAsync(test_key2, values2);

            var unionResult = _RedisService.SetCombineAsync<string>(new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Union, CommandFlags.PreferSlave).Result;

            Assert.Equal(4, unionResult.Count());
            Assert.Contains("11111", unionResult);
            Assert.Contains("22222", unionResult);
            Assert.Contains("33333", unionResult);
            Assert.Contains("44444", unionResult);

            var intersectResult = _RedisService.SetCombineAsync<string>(new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Intersect, CommandFlags.PreferSlave).Result;
            Assert.Equal(2, intersectResult.Count());
            Assert.Contains("22222", intersectResult);
            Assert.Contains("33333", intersectResult);

            //第一个Set不存在与其他Set的元素
            var differentResult = _RedisService.SetCombineAsync<string>(new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Difference, CommandFlags.PreferSlave).Result;
            Assert.Single(differentResult);
            Assert.Contains("11111", differentResult);


            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);
        }

        [Fact(DisplayName = "Set集合合并(两个集合合并)")]
        public async Task SetCombineAsyncTwoSetTest()
        {
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";
            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            await _RedisService.SetAddRanageAsync(test_key1, values1);
            await _RedisService.SetAddRanageAsync(test_key2, values2);

            var unionResult = _RedisService.SetCombineAsync<string>(test_key1, test_key2, SetOperation.Union).Result;

            Assert.Equal(4, unionResult.Count());
            Assert.Contains("11111", unionResult);
            Assert.Contains("22222", unionResult);
            Assert.Contains("33333", unionResult);
            Assert.Contains("44444", unionResult);

            var intersectResult = _RedisService.SetCombineAsync<string>(test_key1, test_key2, SetOperation.Intersect).Result;
            Assert.Equal(2, intersectResult.Count());
            Assert.Contains("22222", intersectResult);
            Assert.Contains("33333", intersectResult);

            //第一个Set不存在与其他Set的元素
            var differentResult = _RedisService.SetCombineAsync<string>(test_key1, test_key2, SetOperation.Difference).Result;
            Assert.Single(differentResult);
            Assert.Contains("11111", differentResult);


            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);
        }

        [Fact(DisplayName = "Set集合合并（多个集合）并存储到新集合")]
        public async Task SetCombineStoreAsyncTest()
        {
            var test_store_key = "test_set_combine_store";
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";

            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            await _RedisService.SetAddRanageAsync(test_key1, values1);
            await _RedisService.SetAddRanageAsync(test_key2, values2);

            var unionResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { }, SetOperation.Union).Result;
            Assert.Equal(0, unionResult);
            unionResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, null, SetOperation.Union).Result;
            Assert.Equal(0, unionResult);

            unionResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Union).Result;
            Assert.Equal(4, unionResult);

            var storeResult = _RedisService.SetGetAllAsync<string>(test_store_key).Result;
            Assert.Equal(4, storeResult.Count());
            Assert.Contains("11111", storeResult);
            Assert.Contains("22222", storeResult);
            Assert.Contains("33333", storeResult);
            Assert.Contains("44444", storeResult);

            await _RedisService.SetRemoveAllAsync(test_store_key);

            var intersectResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Intersect, CommandFlags.PreferSlave).Result;
            Assert.Equal(2, intersectResult);

            storeResult = _RedisService.SetGetAllAsync<string>(test_store_key).Result;
            Assert.Equal(2, storeResult.Count());
            Assert.Contains("22222", storeResult);
            Assert.Contains("33333", storeResult);

            await _RedisService.SetRemoveAllAsync(test_store_key);
            //第一个Set不存在与其他Set的元素
            var differentResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Difference, CommandFlags.PreferSlave).Result;
            Assert.Equal(1, differentResult);

            storeResult = _RedisService.SetGetAllAsync<string>(test_store_key).Result;
            Assert.Single(storeResult);
            Assert.Contains("11111", storeResult);

            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);
            await _RedisService.SetRemoveAllAsync(test_store_key);
        }

        [Fact(DisplayName = "将值从一个Set移动到另外一个Set")]
        public async Task SetMoveAsyncTest()
        {
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";

            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            await _RedisService.SetAddRanageAsync(test_key1, values1);
            await _RedisService.SetAddRanageAsync(test_key2, values2);


            var moveResult = _RedisService.SetMoveAsync(test_key1, test_key2, "11111").Result;
            Assert.True(moveResult);

            var checkSetResult = _RedisService.SetExistsAsync(test_key1, "11111").Result;
            Assert.False(checkSetResult);

            checkSetResult = _RedisService.SetExistsAsync(test_key2, "11111").Result;
            Assert.True(checkSetResult);

            await _RedisService.SetRemoveAllAsync(test_key1);
            await _RedisService.SetRemoveAllAsync(test_key2);
        }

        [Fact(DisplayName = "集合过期时间点")]
        public async Task SetExpireAtAsyncTest()
        {
            var test_key = "test_set_expire_at";
            await _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333" };
            await _RedisService.SetAddRanageAsync(test_key, values);

            var expireResult = _RedisService.SetExpireAtAsync(test_key, DateTime.Now.AddSeconds(5)).Result;
            Assert.True(expireResult);

            Task.Factory.StartNew(() =>
            {
                Task.Delay(6).Wait();

                var setCount = _RedisService.SetCountAsync(test_key).Result;

                Assert.Equal(setCount, 0);
            });
            _RedisService.SetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "集合过期时间段")]
        public async Task SetExpireInAsyncTest()
        {
            var test_key = "test_set_expire_in";
            await _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333" };
            await _RedisService.SetAddRanageAsync(test_key, values);

            var expireResult = _RedisService.SetExpireInAsync(test_key, new TimeSpan(0, 0, 5)).Result;
            Assert.True(expireResult);

            Task.Factory.StartNew(() =>
            {
                Task.Delay(6).Wait();

                var setCount = _RedisService.SetCountAsync(test_key).Result;

                Assert.Equal(setCount, 0);
            });
            _RedisService.SetRemoveAllAsync(test_key);
        }
    }
}
