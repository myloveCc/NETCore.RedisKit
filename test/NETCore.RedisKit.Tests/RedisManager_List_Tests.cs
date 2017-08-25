using NETCore.RedisKit.Core.Internal;
using NETCore.RedisKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using NETCore.RedisKit.Core;

namespace NETCore.RedisKit.Tests
{
    public class _RedisService_List_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_List_Tests()
        {
            IRedisProvider redisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            }, true);


            IRedisKitLogger logger = new RedisKitLogger(new LoggerFactory(), redisProvider);

            _RedisService = new RedisService(redisProvider, logger);
        }

        [Fact(DisplayName = "在List值之前（左侧）插入")]
        public async Task ListInsertLeftAsyncTest()
        {
            var test_key = "test_list_insert_left";

            await _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListLeftPushAsync(test_key, "1111111").Result;
            Assert.Equal(1, pushResult);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "2222222").Result;
            Assert.Equal(2, pushResult);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "3333333").Result;
            Assert.Equal(3, pushResult);

            var insertBeforeResult = _RedisService.ListInsertLeftAsync(test_key, "44444444", "1111111").Result;
            Assert.Equal(4, insertBeforeResult);

            var firstValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal("1111111", firstValue);

            var secondValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal("44444444", secondValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }


        [Fact(DisplayName = "在List值右侧插入")]
        public async Task ListInsertRightAsyncTest()
        {
            var test_key = "test_list_insert_right";

            await _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListLeftPushAsync(test_key, "1111111").Result;
            Assert.Equal(1, pushResult);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "2222222").Result;
            Assert.Equal(2, pushResult);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "3333333").Result;
            Assert.Equal(3, pushResult);

            var insertAfterResult = _RedisService.ListInsertRightAsync(test_key, "44444444", "1111111").Result;
            Assert.Equal(4, insertAfterResult);

            var firstValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal("44444444", firstValue);

            var secondValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal("1111111", secondValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List左侧添加值")]
        public async Task ListLeftPushAsyncTest()
        {
            var test_key = "test_list_push_left";

            await _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListLeftPushAsync(test_key, "1111111").Result;
            Assert.Equal(1, pushResult);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "2222222").Result;
            Assert.Equal(2, pushResult);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "3333333").Result;
            Assert.Equal(3, pushResult);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal("1111111", rightValue);

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal("3333333", leftValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List左侧添加多个值集合")]
        public async Task ListLeftPushAsyncRangeTest()
        {
            var test_key = "test_list_push_left_range";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(3, pushResult);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal("1111111", rightValue);

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal("3333333", leftValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }


        [Fact(DisplayName = "在List右侧添加值")]
        public  async Task ListRightPushAsyncTest()
        {
            var test_key = "test_list_push_right";

            await _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListRightPushAsync(test_key, "1111111").Result;
            Assert.Equal(1, pushResult);

            pushResult = _RedisService.ListRightPushAsync(test_key, "2222222").Result;
            Assert.Equal(2, pushResult);

            pushResult = _RedisService.ListRightPushAsync(test_key, "3333333").Result;
            Assert.Equal(3, pushResult);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal("3333333", rightValue);

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal("1111111", leftValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }


        [Fact(DisplayName = "在List右侧添加多个值集合")]
        public async Task ListRightPushAsyncRangeTest()
        {
            var test_key = "test_list_push_right_range";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListRightPushRanageAsync(test_key, values).Result;
            Assert.Equal(3, pushResult);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal("3333333", rightValue);

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal("1111111", leftValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List左侧取出一个值")]
        public async Task ListLeftPopAsyncTest()
        {
            var test_key = "test_list_pop_left";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(3, pushResult);

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;

            Assert.Equal("3333333", leftValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List右侧取出一个值")]
        public async Task ListRightPopAsyncTest()
        {
            var test_key = "test_list_pop_right";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(3, pushResult);

            var leftValue = _RedisService.ListRightPopAsync<string>(test_key).Result;

            Assert.Equal("1111111", leftValue);

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List移除指定值（可能存在多个）")]
        public async Task ListRemoveAsyncTest()
        {
            var test_key = "test_list_remove";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "1111111" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(5, pushResult);

            var removeResult = _RedisService.ListRemoveAsync(test_key, "1111111").Result;
            Assert.Equal(3, removeResult);

            var countResult = _RedisService.ListCountAsync(test_key).Result;
            Assert.Equal(2, countResult);

            await _RedisService.ListRemoveAllAsync(test_key);
        }


        [Fact(DisplayName = "在List移除全部缓存")]
        public async Task ListRemoveAllAsyncTest()
        {
            var test_key = "test_list_remove_all";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "1111111" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(5, pushResult);

            var removeResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(removeResult);

            removeResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.False(removeResult);

            var countResult = _RedisService.ListCountAsync(test_key).Result;
            Assert.Equal(0, countResult);

            await _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取List的总数量")]
        public async Task ListCountAsyncTest()
        {
            var test_key = "test_list_count";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "1111111" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(5, pushResult);

            var countResult = _RedisService.ListCountAsync(test_key).Result;
            Assert.Equal(5, countResult);

            await _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "根据索引Index获取List的值")]
        public async Task ListGetByIndexAsyncTest()
        {
            var test_key = "test_get_by_index";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "4444444" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(5, pushResult);

            var firstValue = _RedisService.ListGetByIndexAsync<string>(test_key, 0).Result;
            Assert.Equal("4444444", firstValue);

            var lastValue = _RedisService.ListGetByIndexAsync<string>(test_key, -1).Result;
            Assert.Equal("1111111", lastValue);

            await _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取List全部数据")]
        public async Task ListGetAllAsyncTest()
        {
            var test_key = "test_get_all";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "4444444" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(5, pushResult);

            var getAllResult = _RedisService.ListGetAllAsync<string>(test_key).Result.ToList();
            Assert.Equal(5, getAllResult.Count());

            Assert.Equal("4444444", getAllResult[0]);
            Assert.Equal("1111111", getAllResult[4]);

            await _RedisService.ListRemoveAllAsync(test_key);
        }


        [Fact(DisplayName = "获取List区间数据操作")]
        public async Task ListGetRangeAsyncTest()
        {
            var test_key = "test_get_rang";
            await _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "4444444" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(5, pushResult);

            var getRanageResult = _RedisService.ListGetRangeAsync<string>(test_key, 0, 2).Result.ToList();
            Assert.Equal(3, getRanageResult.Count());

            var firstValue = getRanageResult.First();
            Assert.Equal("4444444", firstValue);
            var lastValue = getRanageResult.Last();
            Assert.Equal("3333333", lastValue);

            await _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "List过期时间点")]
        public  async Task ListExpireAtAsyncTest()
        {
            var test_key = "test_list_expire_at";
            await _RedisService.ListRemoveAllAsync(test_key);
            var pushResult = _RedisService.ListLeftPushAsync(test_key, "111111").Result;
            Assert.Equal(1, pushResult);

            var exipireAtResult = _RedisService.ListExpireAtAsync(test_key, DateTime.Now.AddSeconds(5));

             Task.Factory.StartNew(() =>
             {
                 Task.Delay(6).Wait();

                 var listCount = _RedisService.ListCountAsync(test_key).Result;

                 Assert.Equal(0, listCount);
             });
        }

        [Fact(DisplayName = "List过期时间段")]
        public async Task ListExpireInAsyncTest()
        {
            var test_key = "test_list_expire_in";
            await _RedisService.ListRemoveAllAsync(test_key);
            var pushResult = _RedisService.ListLeftPushAsync(test_key, "111111").Result;
            Assert.Equal(1, pushResult);

            var exipireAtResult = _RedisService.ListExpireInAsync(test_key, new TimeSpan(0, 0, 6));

             Task.Factory.StartNew(() =>
             {
                 Task.Delay(6).Wait();
                 var listCount = _RedisService.ListCountAsync(test_key).Result;
                 Assert.Equal(0, listCount);
             });
        }
    }
}
