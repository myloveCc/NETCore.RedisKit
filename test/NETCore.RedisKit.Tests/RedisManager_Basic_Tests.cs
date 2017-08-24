using NETCore.RedisKit.Core.Internal;
using NETCore.RedisKit;
using Microsoft.Extensions.Logging;
using Xunit;
using NETCore.RedisKit.Infrastructure.Internal;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace NETCore.RedisKit.Tests
{
    public class _RedisService_Basic_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_Basic_Tests()
        {
            IRedisProvider redisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            }, true);


            IRedisKitLogger logger = new RedisKitLogger(new LoggerFactory(), redisProvider);

            _RedisService = new RedisService(redisProvider, logger);
        }

        [Fact(DisplayName = "自增(加1)")]
        public void IncrementAsyncTest()
        {
            var test_key = "test_increment";

            var addResult = _RedisService.IncrementAsync(test_key).Result;

            Assert.NotEqual(addResult, 0);
            Assert.Equal(addResult, 1);

            addResult = _RedisService.IncrementAsync(test_key).Result;
            Assert.Equal(addResult, 2);

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "加上指定值(long)")]
        public void IncrementAsyncWithLongValueTest()
        {
            var test_key = "test_increment_long";
            var addResult = _RedisService.IncrementAsync(test_key, 2).Result;

            Assert.NotEqual(addResult, 0);
            Assert.Equal(addResult, 2);

            addResult = _RedisService.IncrementAsync(test_key, 3).Result;
            Assert.Equal(addResult, 5);

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "加上指定值(double)")]
        public void IncrementAsyncWithDoubleValueTest()
        {
            var test_key = "test_increment_double";
            var addResult = _RedisService.IncrementAsync(test_key, 2.0).Result;

            Assert.NotEqual(addResult, 0);
            Assert.Equal(addResult, 2.0);

            addResult = _RedisService.IncrementAsync(test_key, 3.0).Result;
            Assert.Equal(addResult, 5.0);

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "自减(减1)")]
        public void DecrementAsyncTest()
        {
            var test_key = "test_decrement";
            var incReuslt = _RedisService.IncrementAsync(test_key, 2).Result;
            Assert.Equal(incReuslt, 2);

            var decResult = _RedisService.DecrementAsync(test_key).Result;

            Assert.NotEqual(decResult, 0);
            Assert.Equal(decResult, 1);

            decResult = _RedisService.DecrementAsync(test_key).Result;
            Assert.Equal(decResult, 0);
            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "减去指定值(long)")]
        public async Task DecrementAsyncWithLongTest()
        {
            var test_key = "test_decrement_long";
            await _RedisService.StringRemoveAsync(test_key);
            var incReuslt = _RedisService.IncrementAsync(test_key, 5).Result;
            Assert.Equal(5, incReuslt);

            var decResult = _RedisService.DecrementAsync(test_key, 2).Result;

            Assert.NotEqual(0, decResult);
            Assert.Equal(3, decResult);

            decResult = _RedisService.DecrementAsync(test_key, 3).Result;
            Assert.Equal(0, decResult);
            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }


        [Fact(DisplayName = "减去指定值(double)")]
        public void DecrementAsyncWithDoubleTest()
        {
            var test_key = "test_decrement_long";
            _RedisService.StringRemoveAsync(test_key);
            var incReuslt = _RedisService.IncrementAsync(test_key, 5.0).Result;
            Assert.Equal(5.0, incReuslt);

            var decResult = _RedisService.DecrementAsync(test_key, 2.0).Result;

            Assert.NotEqual(0, decResult);
            Assert.Equal(3.0, decResult);

            decResult = _RedisService.DecrementAsync(test_key, 3).Result;
            Assert.Equal(0, decResult);
            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "修改缓存Key名称")]
        public void KeyRenameAsyncTest()
        {
            var test_key_old = "test_key_old";
            var test_key_new = "test_key_new";

            _RedisService.StringRemoveAsync(test_key_old);
            _RedisService.StringRemoveAsync(test_key_new);

            //key is not exists
            var renameResult = _RedisService.KeyRenameAsync(test_key_old, test_key_new).Result;
            Assert.False(renameResult);

            //key name is same
            renameResult = _RedisService.KeyRenameAsync(test_key_old, test_key_old).Result;
            Assert.False(renameResult);

            //key is add to redis
            var setResult = _RedisService.StringSetAsync(test_key_old, "RenameKey").Result;
            Assert.True(setResult);

            renameResult = _RedisService.KeyRenameAsync(test_key_old, test_key_new).Result;
            Assert.True(renameResult);

            var getValue = _RedisService.StringGetAsync<string>(test_key_old).Result;
            Assert.Null(getValue);

            getValue = _RedisService.StringGetAsync<string>(test_key_new).Result;
            Assert.Equal(getValue, "RenameKey");

            var delResult = _RedisService.StringRemoveAsync(test_key_new).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "获取Key对应缓存类型")]
        public async Task KeyTypeAsyncTest()
        {
            var test_key_string = "test_key_string";
            var test_key_list = "test_key_list";
            var test_key_set = "test_key_set";
            var test_key_zset = "test_key_zset";
            var test_key_hash = "test_key_hash";

            await _RedisService.StringRemoveAsync(test_key_string);
            var addResult = _RedisService.StringSetAsync(test_key_string, "111").Result;
            Assert.True(addResult);
            var keyType = _RedisService.KeyTypeAsync(test_key_string).Result;
            Assert.Equal(keyType, RedisType.String);
            var delResult = _RedisService.StringRemoveAsync(test_key_string).Result;
            Assert.True(delResult);

            _RedisService.ListRemoveAllAsync(test_key_list);
            var listAddResult = _RedisService.ListLeftPushAsync(test_key_list, "1111").Result;
            Assert.Equal(listAddResult, 1);
            keyType = _RedisService.KeyTypeAsync(test_key_list).Result;
            Assert.Equal(keyType, RedisType.List);
            delResult = _RedisService.ListRemoveAllAsync(test_key_list).Result;
            Assert.True(delResult);

            _RedisService.SetRemoveAllAsync(test_key_set);
            var setAddResult = _RedisService.SetAddAsync(test_key_set, "1111").Result;
            Assert.True(setAddResult);
            keyType = _RedisService.KeyTypeAsync(test_key_set).Result;
            Assert.Equal(keyType, RedisType.Set);
            delResult = _RedisService.SetRemoveAllAsync(test_key_set).Result;
            Assert.True(delResult);

            _RedisService.SortedSetRemoveAllAsync(test_key_zset);
            var zsetAddResult = _RedisService.SortedSetAddAsync(test_key_zset, "1111", 1).Result;
            Assert.True(zsetAddResult);
            keyType = _RedisService.KeyTypeAsync(test_key_zset).Result;
            Assert.Equal(keyType, RedisType.SortedSet);
            delResult = _RedisService.SortedSetRemoveAllAsync(test_key_zset).Result;
            Assert.True(delResult);


            _RedisService.HashRemoveAllAsync(test_key_hash);
            var hashAddResult = _RedisService.HashSetAsync(test_key_hash, 1, "1111").Result;
            Assert.True(hashAddResult);
            keyType = _RedisService.KeyTypeAsync(test_key_hash).Result;
            Assert.Equal(keyType, RedisType.Hash);
            delResult = _RedisService.HashRemoveAllAsync(test_key_hash).Result;
            Assert.True(delResult);
        }
    }
}
