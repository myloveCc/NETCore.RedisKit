using System.Collections.Generic;
using System.Linq;
using Xunit;
using StackExchange.Redis;
using NETCore.RedisKit.Core.Internal;
using NETCore.RedisKit.Infrastructure.Internal;

namespace NETCore.RedisKit.Tests
{
    public class _RedisService_SortedSet_Tests
    {

        private readonly IRedisService _RedisService;
        public _RedisService_SortedSet_Tests()
        {
            IRedisProvider redisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            });

            _RedisService = new RedisService(redisProvider);
        }


        private readonly List<SortedSetEntry> values = new List<SortedSetEntry>() { new SortedSetEntry("\"111111\"", 1), new SortedSetEntry("\"222222\"", 2), new SortedSetEntry("\"333333\"", 3), new SortedSetEntry("\"444444\"", 4) };


        [Fact(DisplayName = "可排序集合的值加上指定Score")]
        public void SortedSetIncrementScoreAsyncTest()
        {
            var test_key = "test_zset_increment";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var incrementResult = _RedisService.SortedSetIncrementScoreAsync(test_key, "222222", 3).Result;
            Assert.Equal(incrementResult, 5);

            _RedisService.SortedSetRemoveAllAsync(test_key);

        }

        [Fact(DisplayName = "可排序集合的值减去指定Score")]
        public void SortedSetDecrementScoreAsyncTest()
        {
            var test_key = "test_zset_decrement";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var decrementResult = _RedisService.SortedSetDecrementScoreAsync(test_key, "222222", 2).Result;
            Assert.Equal(decrementResult, 0);

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "可排序集合移除单个值")]
        public void SortedSetRemoveAsyncTest()
        {
            var test_key = "test_zset_remove";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var removeResult = _RedisService.SortedSetRemoveAsync(test_key, "222222").Result;
            Assert.True(removeResult);

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "可排序集合移除多个值")]
        public void SortedSetRemoveRanageAsyncTest()
        {
            var test_key = "test_zset_remove";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var removeResult = _RedisService.SortedSetRemoveRanageAsync(test_key, new List<string>() { "222222", "333333" }).Result;
            Assert.Equal(removeResult, 2);

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }


        [Fact(DisplayName = "根据索引区间删除可排序集合中的数据")]
        public void SortedSetRemoveAsyncByIndexTest()
        {
            var test_key = "test_zset_remove_by_index";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var removeResult = _RedisService.SortedSetRemoveAsync(test_key, 0, 2).Result;
            Assert.Equal(removeResult, 3);

            var existResult = _RedisService.SortedSetExistsAsync(test_key, "444444").Result;
            Assert.True(existResult);

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "根据Score区间删除可排序集合中的数据")]
        public void SortedSetRemoveAsyncByScoreTest()
        {
            var test_key = "test_zset_remove_by_index";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.None).Result;
            Assert.Equal(removeResult, 3);

            _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;

            removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.Start).Result;
            Assert.Equal(removeResult, 2);

            _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;

            removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.Stop).Result;
            Assert.Equal(removeResult, 2);

            _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;

            removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.Both).Result;
            Assert.Equal(removeResult, 1);

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "可排序集合包路径")]
        public void SortedSetTrimAsyncTest()
        {
            var test_key = "test_zset_trim";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            //保留Score前2名
            var trimResult = _RedisService.SortedSetTrimAsync(test_key, 2, Order.Descending).Result;
            Assert.Equal(trimResult, 2);
            var zsetValues = _RedisService.SortedSetGetAllAsync<string>(test_key, Order.Descending).Result;
            Assert.True(zsetValues.Contains("333333"));
            Assert.True(zsetValues.Contains("444444"));

            //保留Score后2名
            _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);
            trimResult = _RedisService.SortedSetTrimAsync(test_key, 2, Order.Ascending).Result;
            Assert.Equal(trimResult, 2);
            zsetValues = _RedisService.SortedSetGetAllAsync<string>(test_key, Order.Descending).Result;
            Assert.True(zsetValues.Contains("111111"));
            Assert.True(zsetValues.Contains("222222"));

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取集合中总数")]
        public void SortedSetCountAsyncTest()
        {
            var test_key = "test_zset_count";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var zsetCount = _RedisService.SortedSetCountAsync(test_key).Result;
            Assert.Equal(zsetCount, 4);

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "查询可排序集合中是否存在值")]
        public void SortedSetExistsAsyncTest()
        {
            var test_key = "test_zset_exist";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var existResult = _RedisService.SortedSetExistsAsync(test_key, "333333").Result;
            Assert.True(existResult);

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取集合中Score最小对应的值")]
        public void SortedSetGetMinByScoreAsyncTest()
        {
            var test_key = "test_zset_min";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var minValue = _RedisService.SortedSetGetMinByScoreAsync<string>(test_key).Result;

            Assert.NotNull(minValue);
            Assert.NotEmpty(minValue);
            Assert.Equal(minValue, "111111");

            _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取集合中Score最大对应的值")]
        public void SortedSetGetMaxByScoreAsyncTest()
        {
            var test_key = "test_zset_max";

            _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(addResult, 4);

            var maxValue = _RedisService.SortedSetGetMaxByScoreAsync<string>(test_key).Result;

            Assert.NotNull(maxValue);
            Assert.NotEmpty(maxValue);
            Assert.Equal(maxValue, "444444");

            _RedisService.SortedSetRemoveAllAsync(test_key);

        }
    };
}
