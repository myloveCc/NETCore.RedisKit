using System.Collections.Generic;
using System.Linq;
using Xunit;
using StackExchange.Redis;
using NETCore.RedisKit.Core.Internal;
using NETCore.RedisKit.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using NETCore.RedisKit.Core;
using System.Threading.Tasks;

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
            }, true);


            IRedisKitLogger logger = new RedisKitLogger(new LoggerFactory(), redisProvider);

            _RedisService = new RedisService(redisProvider, logger);
        }


        private readonly List<SortedSetEntry> values = new List<SortedSetEntry>() { new SortedSetEntry("\"111111\"", 1), new SortedSetEntry("\"222222\"", 2), new SortedSetEntry("\"333333\"", 3), new SortedSetEntry("\"444444\"", 4) };


        [Fact(DisplayName = "可排序集合的值加上指定Score")]
        public async Task SortedSetIncrementScoreAsyncTest()
        {
            var test_key = "test_zset_increment";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var incrementResult = _RedisService.SortedSetIncrementScoreAsync(test_key, "222222", 3).Result;
            Assert.Equal(5, incrementResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);

        }

        [Fact(DisplayName = "可排序集合的值减去指定Score")]
        public async Task SortedSetDecrementScoreAsyncTest()
        {
            var test_key = "test_zset_decrement";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var decrementResult = _RedisService.SortedSetDecrementScoreAsync(test_key, "222222", 2).Result;
            Assert.Equal(0, decrementResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "可排序集合移除单个值")]
        public async Task SortedSetRemoveAsyncTest()
        {
            var test_key = "test_zset_remove";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var removeResult = _RedisService.SortedSetRemoveAsync(test_key, "222222").Result;
            Assert.True(removeResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "可排序集合移除多个值")]
        public async Task SortedSetRemoveRanageAsyncTest()
        {
            var test_key = "test_zset_remove";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var removeResult = _RedisService.SortedSetRemoveRanageAsync(test_key, new List<string>() { "222222", "333333" }).Result;
            Assert.Equal(2, removeResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }


        [Fact(DisplayName = "根据索引区间删除可排序集合中的数据")]
        public async Task SortedSetRemoveAsyncByIndexTest()
        {
            var test_key = "test_zset_remove_by_index";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var removeResult = _RedisService.SortedSetRemoveAsync(test_key, 0, 2).Result;
            Assert.Equal(3, removeResult);

            var existResult = _RedisService.SortedSetExistsAsync(test_key, "444444").Result;
            Assert.True(existResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "根据Score区间删除可排序集合中的数据")]
        public async Task SortedSetRemoveAsyncByScoreTest()
        {
            var test_key = "test_zset_remove_by_index";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.None).Result;
            Assert.Equal(3, removeResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;

            removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.Start).Result;
            Assert.Equal(2, removeResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;

            removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.Stop).Result;
            Assert.Equal(2, removeResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;

            removeResult = _RedisService.SortedSetRemoveAsync(test_key, 1, 3, Exclude.Both).Result;
            Assert.Equal(1, removeResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "可排序集合包路径")]
        public async Task SortedSetTrimAsyncTest()
        {
            var test_key = "test_zset_trim";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            //保留Score前2名
            var trimResult = _RedisService.SortedSetTrimAsync(test_key, 2, Order.Descending).Result;
            Assert.Equal(2, trimResult);
            var zsetValues = _RedisService.SortedSetGetAllAsync<string>(test_key, Order.Descending).Result;
            Assert.Contains("333333", zsetValues);
            Assert.Contains("444444", zsetValues);

            //保留Score后2名
            await _RedisService.SortedSetRemoveAllAsync(test_key);
            addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);
            trimResult = _RedisService.SortedSetTrimAsync(test_key, 2, Order.Ascending).Result;
            Assert.Equal(2, trimResult);
            zsetValues = _RedisService.SortedSetGetAllAsync<string>(test_key, Order.Descending).Result;
            Assert.Contains("111111", zsetValues);
            Assert.Contains("222222", zsetValues);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取集合中总数")]
        public async Task SortedSetCountAsyncTest()
        {
            var test_key = "test_zset_count";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var zsetCount = _RedisService.SortedSetCountAsync(test_key).Result;
            Assert.Equal(4, zsetCount);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "查询可排序集合中是否存在值")]
        public async Task SortedSetExistsAsyncTest()
        {
            var test_key = "test_zset_exist";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var existResult = _RedisService.SortedSetExistsAsync(test_key, "333333").Result;
            Assert.True(existResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取集合中Score最小对应的值")]
        public async Task SortedSetGetMinByScoreAsyncTest()
        {
            var test_key = "test_zset_min";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var minValue = _RedisService.SortedSetGetMinByScoreAsync<string>(test_key).Result;

            Assert.NotNull(minValue);
            Assert.NotEmpty(minValue);
            Assert.Equal("111111", minValue);

            await _RedisService.SortedSetRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "获取集合中Score最大对应的值")]
        public async Task SortedSetGetMaxByScoreAsyncTest()
        {
            var test_key = "test_zset_max";

            await _RedisService.SortedSetRemoveAllAsync(test_key);

            var addResult = _RedisService.SortedSetAddAsync(test_key, values).Result;
            Assert.Equal(4, addResult);

            var maxValue = _RedisService.SortedSetGetMaxByScoreAsync<string>(test_key).Result;

            Assert.NotNull(maxValue);
            Assert.NotEmpty(maxValue);
            Assert.Equal("444444", maxValue);

            await _RedisService.SortedSetRemoveAllAsync(test_key);

        }
    };
}
