using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using StackExchange.Redis;
using NETCore.RedisKit.Core.Internal;
using NETCore.RedisKit.Infrastructure.Internal;

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
            });

            _RedisService = new RedisService(redisProvider);
        }

        [Fact(DisplayName = "新增单个值到Set集合")]
        public void SetAddAsyncTest()
        {
            var test_key = "test_set_add";

            _RedisService.SetRemoveAllAsync(test_key);

            var addResult = _RedisService.SetAddAsync(test_key, "1111111").Result;
            Assert.True(addResult);

            addResult = _RedisService.SetAddAsync(test_key, "1111111").Result;
            Assert.False(addResult);

            var delResult = _RedisService.SetRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "新增集合到Set集合")]
        public void SetAddRanageAsyncTest()
        {
            var test_key = "test_set_add_range";
            _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333", "11111" };
            var addResult = _RedisService.SetAddRanageAsync(test_key, values).Result;
            Assert.Equal(addResult, 3);
            _RedisService.SetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Set集合中的值")]
        public void SetRemoveAsyncTest()
        {
            var test_key = "test_set_remove";
            _RedisService.SetRemoveAllAsync(test_key);

            var addResult = _RedisService.SetAddAsync(test_key, "1111111").Result;
            Assert.True(addResult);

            var removeResult = _RedisService.SetRemoveAsync<string>(test_key, "1111111").Result;
            Assert.True(removeResult);
            _RedisService.SetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Set集合中的多个值")]
        public void SetRemoveRangeAsyncTest()
        {
            var test_key = "test_set_remove_range";
            _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333", "11111" };
            var addResult = _RedisService.SetAddRanageAsync(test_key, values).Result;
            Assert.Equal(addResult, 3);

            var removeRangeResult = _RedisService.SetRemoveRangeAsync<string>(test_key, new List<string>() { "11111", "22222" }).Result;
            Assert.Equal(removeRangeResult, 2);

            var setCount = _RedisService.SetCountAsync(test_key).Result;
            Assert.Equal(setCount, 1);

            _RedisService.SetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Set集合全部值")]
        public void SetRemoveAllAsync()
        {
            var test_key = "test_set_remove_all";
            _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333", "11111" };
            var addResult = _RedisService.SetAddRanageAsync(test_key, values).Result;
            Assert.Equal(addResult, 3);

            var delResult = _RedisService.SetRemoveAllAsync(test_key).Result;
            Assert.True(delResult);

            var setCount = _RedisService.SetCountAsync(test_key).Result;
            Assert.Equal(setCount, 0);
        }

        [Fact(DisplayName = "Set集合合并(多个集合合并)")]
        public void SetCombineAsyncTest()
        {
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";
            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            _RedisService.SetAddRanageAsync(test_key1, values1);
            _RedisService.SetAddRanageAsync(test_key2, values2);

            var unionResult = _RedisService.SetCombineAsync<string>(new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Union, CommandFlags.PreferSlave).Result;

            Assert.Equal(unionResult.Count(), 4);
            Assert.True(unionResult.Contains("11111"));
            Assert.True(unionResult.Contains("22222"));
            Assert.True(unionResult.Contains("33333"));
            Assert.True(unionResult.Contains("44444"));

            var intersectResult = _RedisService.SetCombineAsync<string>(new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Intersect, CommandFlags.PreferSlave).Result;
            Assert.Equal(intersectResult.Count(), 2);
            Assert.True(intersectResult.Contains("22222"));
            Assert.True(intersectResult.Contains("33333"));

            //第一个Set不存在与其他Set的元素
            var differentResult = _RedisService.SetCombineAsync<string>(new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Difference, CommandFlags.PreferSlave).Result;
            Assert.Equal(differentResult.Count(), 1);
            Assert.True(differentResult.Contains("11111"));


            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);
        }

        [Fact(DisplayName = "Set集合合并(两个集合合并)")]
        public void SetCombineAsyncTwoSetTest()
        {
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";
            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            _RedisService.SetAddRanageAsync(test_key1, values1);
            _RedisService.SetAddRanageAsync(test_key2, values2);

            var unionResult = _RedisService.SetCombineAsync<string>(test_key1, test_key2, SetOperation.Union).Result;

            Assert.Equal(unionResult.Count(), 4);
            Assert.True(unionResult.Contains("11111"));
            Assert.True(unionResult.Contains("22222"));
            Assert.True(unionResult.Contains("33333"));
            Assert.True(unionResult.Contains("44444"));

            var intersectResult = _RedisService.SetCombineAsync<string>(test_key1, test_key2, SetOperation.Intersect).Result;
            Assert.Equal(intersectResult.Count(), 2);
            Assert.True(intersectResult.Contains("22222"));
            Assert.True(intersectResult.Contains("33333"));

            //第一个Set不存在与其他Set的元素
            var differentResult = _RedisService.SetCombineAsync<string>(test_key1, test_key2, SetOperation.Difference).Result;
            Assert.Equal(differentResult.Count(), 1);
            Assert.True(differentResult.Contains("11111"));


            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);
        }

        [Fact(DisplayName = "Set集合合并（多个集合）并存储到新集合")]
        public void SetCombineStoreAsyncTest()
        {
            var test_store_key = "test_set_combine_store";
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";

            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            _RedisService.SetAddRanageAsync(test_key1, values1);
            _RedisService.SetAddRanageAsync(test_key2, values2);

            var unionResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { }, SetOperation.Union).Result;
            Assert.Equal(unionResult, 0);
            unionResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, null, SetOperation.Union).Result;
            Assert.Equal(unionResult, 0);

            unionResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Union).Result;
            Assert.Equal(unionResult, 4);

            var storeResult = _RedisService.SetGetAllAsync<string>(test_store_key).Result;
            Assert.Equal(storeResult.Count(), 4);
            Assert.True(storeResult.Contains("11111"));
            Assert.True(storeResult.Contains("22222"));
            Assert.True(storeResult.Contains("33333"));
            Assert.True(storeResult.Contains("44444"));

            _RedisService.SetRemoveAllAsync(test_store_key);

            var intersectResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Intersect, CommandFlags.PreferSlave).Result;
            Assert.Equal(intersectResult, 2);

            storeResult = _RedisService.SetGetAllAsync<string>(test_store_key).Result;
            Assert.Equal(storeResult.Count(), 2);
            Assert.True(storeResult.Contains("22222"));
            Assert.True(storeResult.Contains("33333"));

            _RedisService.SetRemoveAllAsync(test_store_key);
            //第一个Set不存在与其他Set的元素
            var differentResult = _RedisService.SetCombineStoreAsync<string>(test_store_key, new List<RedisKey>() { test_key1, test_key2 }, SetOperation.Difference, CommandFlags.PreferSlave).Result;
            Assert.Equal(differentResult, 1);

            storeResult = _RedisService.SetGetAllAsync<string>(test_store_key).Result;
            Assert.Equal(storeResult.Count(), 1);
            Assert.True(storeResult.Contains("11111"));

            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);
            _RedisService.SetRemoveAllAsync(test_store_key);
        }

        [Fact(DisplayName = "将值从一个Set移动到另外一个Set")]
        public void SetMoveAsyncTest()
        {
            var test_key1 = "test_set_combine_1";
            var test_key2 = "test_set_combine_2";

            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);

            var values1 = new List<string>() { "11111", "22222", "33333" };
            var values2 = new List<string>() { "22222", "33333", "44444" };

            _RedisService.SetAddRanageAsync(test_key1, values1);
            _RedisService.SetAddRanageAsync(test_key2, values2);


            var moveResult = _RedisService.SetMoveAsync(test_key1, test_key2, "11111").Result;
            Assert.True(moveResult);

            var checkSetResult = _RedisService.SetExistsAsync(test_key1, "11111").Result;
            Assert.False(checkSetResult);

            checkSetResult = _RedisService.SetExistsAsync(test_key2, "11111").Result;
            Assert.True(checkSetResult);

            _RedisService.SetRemoveAllAsync(test_key1);
            _RedisService.SetRemoveAllAsync(test_key2);
        }

        [Fact(DisplayName = "集合过期时间点")]
        public void SetExpireAtAsyncTest()
        {
            var test_key = "test_set_expire_at";
            _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333" };
            _RedisService.SetAddRanageAsync(test_key, values);

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
        public void SetExpireInAsyncTest()
        {
            var test_key = "test_set_expire_in";
            _RedisService.SetRemoveAllAsync(test_key);

            var values = new List<string>() { "11111", "22222", "33333" };
            _RedisService.SetAddRanageAsync(test_key, values);

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
