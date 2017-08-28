using NETCore.RedisKit.Loging;
using NETCore.RedisKit;
using Microsoft.Extensions.Logging;
using Xunit;
using StackExchange.Redis;
using System.Threading.Tasks;
using NETCore.RedisKit.Infrastructure;
using NETCore.RedisKit.Configuration;
using NETCore.RedisKit.Services;

namespace NETCore.RedisKit.Tests
{
    public class _RedisService_Basic_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_Basic_Tests()
        {
            IRedisLogger logger = new RedisLogger(new LoggerFactory(), new RedisKitOptions() { IsShowLog = false });

            _RedisService = new RedisService(CommonManager.Instance._RedisProvider, logger, new DefaultJosnSerializeService());
        }

        [Fact(DisplayName = "自增(加1)")]
        public void IncrementAsyncTest()
        {
            var test_key = "test_increment";

            var addResult = _RedisService.IncrementAsync(test_key).Result;

            Assert.NotEqual(0, addResult);
            Assert.Equal(1, addResult);

            addResult = _RedisService.IncrementAsync(test_key).Result;
            Assert.Equal(2, addResult);

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "加上指定值(long)")]
        public void IncrementAsyncWithLongValueTest()
        {
            var test_key = "test_increment_long";
            var addResult = _RedisService.IncrementAsync(test_key, 2).Result;

            Assert.NotEqual(0, addResult);
            Assert.Equal(2, addResult);

            addResult = _RedisService.IncrementAsync(test_key, 3).Result;
            Assert.Equal(5, addResult);

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "加上指定值(double)")]
        public void IncrementAsyncWithDoubleValueTest()
        {
            var test_key = "test_increment_double";
            var addResult = _RedisService.IncrementAsync(test_key, 2.0).Result;

            Assert.NotEqual(0, addResult);
            Assert.Equal(2.0, addResult);

            addResult = _RedisService.IncrementAsync(test_key, 3.0).Result;
            Assert.Equal(5.0, addResult);

            var delResult = _RedisService.StringRemoveAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "自减(减1)")]
        public void DecrementAsyncTest()
        {
            var test_key = "test_decrement";
            _RedisService.ItemRemove(test_key);
            var incReuslt = _RedisService.IncrementAsync(test_key, 2).Result;
            Assert.Equal(2, incReuslt);

            var decResult = _RedisService.DecrementAsync(test_key).Result;

            Assert.NotEqual(0, decResult);
            Assert.Equal(1, decResult);

            decResult = _RedisService.DecrementAsync(test_key).Result;
            Assert.Equal(0, decResult);
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
        public async Task DecrementAsyncWithDoubleTest()
        {
            var test_key = "test_decrement_long";
            await _RedisService.StringRemoveAsync(test_key);
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
            Assert.Equal("RenameKey", getValue);

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
            Assert.Equal(RedisType.String, keyType);
            var delResult = _RedisService.StringRemoveAsync(test_key_string).Result;
            Assert.True(delResult);

            await _RedisService.ListRemoveAllAsync(test_key_list);
            var listAddResult = _RedisService.ListLeftPushAsync(test_key_list, "1111").Result;
            Assert.Equal(1, listAddResult);
            keyType = _RedisService.KeyTypeAsync(test_key_list).Result;
            Assert.Equal(RedisType.List, keyType);
            delResult = _RedisService.ListRemoveAllAsync(test_key_list).Result;
            Assert.True(delResult);

            await _RedisService.SetRemoveAllAsync(test_key_set);
            var setAddResult = _RedisService.SetAddAsync(test_key_set, "1111").Result;
            Assert.True(setAddResult);
            keyType = _RedisService.KeyTypeAsync(test_key_set).Result;
            Assert.Equal(RedisType.Set, keyType);
            delResult = _RedisService.SetRemoveAllAsync(test_key_set).Result;
            Assert.True(delResult);

            await _RedisService.SortedSetRemoveAllAsync(test_key_zset);
            var zsetAddResult = _RedisService.SortedSetAddAsync(test_key_zset, "1111", 1).Result;
            Assert.True(zsetAddResult);
            keyType = _RedisService.KeyTypeAsync(test_key_zset).Result;
            Assert.Equal(RedisType.SortedSet, keyType);
            delResult = _RedisService.SortedSetRemoveAllAsync(test_key_zset).Result;
            Assert.True(delResult);


            await _RedisService.HashRemoveAllAsync(test_key_hash);
            var hashAddResult = _RedisService.HashSetAsync(test_key_hash, 1, "1111").Result;
            Assert.True(hashAddResult);
            keyType = _RedisService.KeyTypeAsync(test_key_hash).Result;
            Assert.Equal(RedisType.Hash, keyType);
            delResult = _RedisService.HashRemoveAllAsync(test_key_hash).Result;
            Assert.True(delResult);
        }
    }
}
