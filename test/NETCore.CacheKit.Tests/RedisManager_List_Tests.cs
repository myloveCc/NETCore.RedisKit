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
        public void ListInsertLeftAsyncTest()
        {
            var test_key = "test_list_insert_left";

            _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListLeftPushAsync(test_key, "1111111").Result;
            Assert.Equal(pushResult, 1);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "2222222").Result;
            Assert.Equal(pushResult, 2);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "3333333").Result;
            Assert.Equal(pushResult, 3);

            var insertBeforeResult = _RedisService.ListInsertLeftAsync(test_key, "44444444", "1111111").Result;
            Assert.Equal(insertBeforeResult, 4);

            var firstValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal(firstValue, "1111111");

            var secondValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal(secondValue, "44444444");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }


        [Fact(DisplayName = "在List值右侧插入")]
        public void ListInsertRightAsyncTest()
        {
            var test_key = "test_list_insert_right";

            _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListLeftPushAsync(test_key, "1111111").Result;
            Assert.Equal(pushResult, 1);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "2222222").Result;
            Assert.Equal(pushResult, 2);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "3333333").Result;
            Assert.Equal(pushResult, 3);

            var insertAfterResult = _RedisService.ListInsertRightAsync(test_key, "44444444", "1111111").Result;
            Assert.Equal(insertAfterResult, 4);

            var firstValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal(firstValue, "44444444");

            var secondValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.Equal(secondValue, "1111111");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List左侧添加值")]
        public void ListLeftPushAsyncTest()
        {
            var test_key = "test_list_push_left";

            _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListLeftPushAsync(test_key, "1111111").Result;
            Assert.Equal(pushResult, 1);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "2222222").Result;
            Assert.Equal(pushResult, 2);

            pushResult = _RedisService.ListLeftPushAsync(test_key, "3333333").Result;
            Assert.Equal(pushResult, 3);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal(rightValue, "1111111");

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal(leftValue, "3333333");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List左侧添加多个值集合")]
        public void ListLeftPushAsyncRangeTest()
        {
            var test_key = "test_list_push_left_range";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 3);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal(rightValue, "1111111");

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal(leftValue, "3333333");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }


        [Fact(DisplayName = "在List右侧添加值")]
        public void ListRightPushAsyncTest()
        {
            var test_key = "test_list_push_right";

            _RedisService.ListRemoveAllAsync(test_key);

            var pushResult = _RedisService.ListRightPushAsync(test_key, "1111111").Result;
            Assert.Equal(pushResult, 1);

            pushResult = _RedisService.ListRightPushAsync(test_key, "2222222").Result;
            Assert.Equal(pushResult, 2);

            pushResult = _RedisService.ListRightPushAsync(test_key, "3333333").Result;
            Assert.Equal(pushResult, 3);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal(rightValue, "3333333");

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal(leftValue, "1111111");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }


        [Fact(DisplayName = "在List右侧添加多个值集合")]
        public void ListRightPushAsyncRangeTest()
        {
            var test_key = "test_list_push_right_range";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListRightPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 3);

            var rightValue = _RedisService.ListRightPopAsync<string>(test_key).Result;
            Assert.NotEmpty(rightValue);
            Assert.Equal(rightValue, "3333333");

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;
            Assert.NotEmpty(leftValue);
            Assert.Equal(leftValue, "1111111");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List左侧取出一个值")]
        public void ListLeftPopAsyncTest()
        {
            var test_key = "test_list_pop_left";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 3);

            var leftValue = _RedisService.ListLeftPopAsync<string>(test_key).Result;

            Assert.Equal(leftValue, "3333333");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List右侧取出一个值")]
        public void ListRightPopAsyncTest()
        {
            var test_key = "test_list_pop_right";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 3);

            var leftValue = _RedisService.ListRightPopAsync<string>(test_key).Result;

            Assert.Equal(leftValue, "1111111");

            var delResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "在List移除指定值（可能存在多个）")]
        public void ListRemoveAsyncTest()
        {
            var test_key = "test_list_remove";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "1111111" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 5);

            var removeResult = _RedisService.ListRemoveAsync(test_key, "1111111").Result;
            Assert.Equal(removeResult, 3);

            var countResult = _RedisService.ListCountAsync(test_key).Result;
            Assert.Equal(countResult, 2);

            _RedisService.ListRemoveAllAsync(test_key);
        }


        [Fact(DisplayName = "在List移除全部缓存")]
        public void ListRemoveAllAsyncTest()
        {
            var test_key = "test_list_remove_all";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "1111111" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 5);

            var removeResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.True(removeResult);

            removeResult = _RedisService.ListRemoveAllAsync(test_key).Result;
            Assert.False(removeResult);

            var countResult = _RedisService.ListCountAsync(test_key).Result;
            Assert.Equal(countResult, 0);

            _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取List的总数量")]
        public void ListCountAsyncTest()
        {
            var test_key = "test_list_count";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "1111111" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 5);

            var countResult = _RedisService.ListCountAsync(test_key).Result;
            Assert.Equal(countResult, 5);

            _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "根据索引Index获取List的值")]
        public void ListGetByIndexAsyncTest()
        {
            var test_key = "test_get_by_index";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "4444444" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 5);

            var firstValue = _RedisService.ListGetByIndexAsync<string>(test_key, 0).Result;
            Assert.Equal(firstValue, "4444444");

            var lastValue = _RedisService.ListGetByIndexAsync<string>(test_key, -1).Result;
            Assert.Equal(lastValue, "1111111");

            _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取List全部数据")]
        public void ListGetAllAsyncTest()
        {
            var test_key = "test_get_all";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "4444444" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 5);

            var getAllResult = _RedisService.ListGetAllAsync<string>(test_key).Result.ToList();
            Assert.Equal(getAllResult.Count(), 5);

            Assert.Equal(getAllResult[0], "4444444");
            Assert.Equal(getAllResult[4], "1111111");

            _RedisService.ListRemoveAllAsync(test_key);
        }


        [Fact(DisplayName = "获取List区间数据操作")]
        public void ListGetRangeAsyncTest()
        {
            var test_key = "test_get_rang";
            _RedisService.ListRemoveAllAsync(test_key);

            var values = new List<string>() { "1111111", "2222222", "3333333", "1111111", "4444444" };
            var pushResult = _RedisService.ListLeftPushRanageAsync(test_key, values).Result;
            Assert.Equal(pushResult, 5);

            var getRanageResult = _RedisService.ListGetRangeAsync<string>(test_key, 0, 2).Result.ToList();
            Assert.Equal(getRanageResult.Count(), 3);

            var firstValue = getRanageResult.First();
            Assert.Equal(firstValue, "4444444");
            var lastValue = getRanageResult.Last();
            Assert.Equal(lastValue, "3333333");

            _RedisService.ListRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "List过期时间点")]
        public void ListExpireAtAsyncTest()
        {
            var test_key = "test_list_expire_at";
            _RedisService.ListRemoveAllAsync(test_key);
            var pushResult = _RedisService.ListLeftPushAsync(test_key, "111111").Result;
            Assert.Equal(pushResult, 1);

            var exipireAtResult = _RedisService.ListExpireAtAsync(test_key, DateTime.Now.AddSeconds(5));

            Task.Factory.StartNew(() =>
            {
                Task.Delay(6).Wait();

                var listCount = _RedisService.ListCountAsync(test_key).Result;

                Assert.Equal(listCount, 0);
            });
        }

        [Fact(DisplayName = "List过期时间段")]
        public void ListExpireInAsyncTest()
        {
            var test_key = "test_list_expire_in";
            _RedisService.ListRemoveAllAsync(test_key);
            var pushResult = _RedisService.ListLeftPushAsync(test_key, "111111").Result;
            Assert.Equal(pushResult, 1);

            var exipireAtResult = _RedisService.ListExpireInAsync(test_key, new TimeSpan(0, 0, 6));

            Task.Factory.StartNew(() =>
            {
                Task.Delay(6).Wait();
                var listCount = _RedisService.ListCountAsync(test_key).Result;
                Assert.Equal(listCount, 0);
            });
        }
    }
}
