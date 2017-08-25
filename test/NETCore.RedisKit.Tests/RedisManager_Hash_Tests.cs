using NETCore.RedisKit.Loging;
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
    public class _RedisService_Hash_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_Hash_Tests()
        {
            IRedisProvider redisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            });

            IRedisLogger logger = new RedisLogger(new LoggerFactory(), redisProvider);

            _RedisService = new RedisService(redisProvider, logger, new DefaultJosnSerializeService());
        }

        [Fact(DisplayName = "新增/更新Hash数据")]
        public void HashSetAsyncTest()
        {
            var test_key = "test_hash_set";

            _RedisService.HashRemoveAllAsync(test_key);

            var setResult = _RedisService.HashSetAsync(test_key, 1111111, "1111111").Result;
            var valueResult = _RedisService.HashGetAsync<string>(test_key, 1111111).Result;
            Assert.True(setResult);
            Assert.Equal("1111111", valueResult);

            setResult = _RedisService.HashSetAsync(test_key, 2222222, "1111111").Result;
            valueResult = _RedisService.HashGetAsync<string>(test_key, 2222222).Result;
            Assert.True(setResult);
            Assert.Equal("1111111", valueResult);

            //如果Field已存在，更新后返回false
            setResult = _RedisService.HashSetAsync(test_key, 1111111, "3333333").Result;
            valueResult = _RedisService.HashGetAsync<string>(test_key, 1111111).Result;
            Assert.False(setResult);
            Assert.Equal("3333333", valueResult);

            var delResult = _RedisService.HashRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "新增/更新Hash数据集合")]
        public void HashSetRangeAsyncTest()
        {
            var test_key = "test_hash_set_range";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.NotEmpty(hashValue);
            Assert.NotNull(hashValue);
            Assert.Equal("第1条测试数据", hashValue);

            _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Hash数据")]
        public void HashRemoveAsyncTest()
        {
            var test_key = "test_hash_remove";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var delResult = _RedisService.HashRemoveAsync(test_key, 1).Result;
            Assert.True(delResult);

            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(9, hashCount);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.Null(hashValue);

            _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除多条Hash数据，根据Fileld数组")]
        public void HashRemoveAsyncRangeByFileldArrayTest()
        {
            var test_key = "test_hash_remove_rang_by_fileld_array";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var delFilelds = new RedisValue[] { 1, 2, 3 };
            var delResult = _RedisService.HashRemoveAsync(test_key, delFilelds).Result;

            Assert.Equal(3, delResult);

            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(7, hashCount);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.Null(hashValue);
            hashValue = _RedisService.HashGetAsync<string>(test_key, 2).Result;
            Assert.Null(hashValue);
            hashValue = _RedisService.HashGetAsync<string>(test_key, 3).Result;
            Assert.Null(hashValue);

            _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除多条Hash数据，根据Fileld集合")]
        public async Task HashRemoveAsyncRangeByFileldListTest()
        {
            var test_key = "test_hash_remove_rang_by_fileld_list";

            await _RedisService.HashRemoveAllAsync(test_key);

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var delFilelds = new List<RedisValue>() { 1, 2, 3 };
            var delResult = _RedisService.HashRemoveAsync(test_key, delFilelds).Result;

            Assert.Equal(3, delResult);

            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(7, hashCount);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.Null(hashValue);
            hashValue = _RedisService.HashGetAsync<string>(test_key, 2).Result;
            Assert.Null(hashValue);
            hashValue = _RedisService.HashGetAsync<string>(test_key, 3).Result;
            Assert.Null(hashValue);

            _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除全部Hash表数据")]
        public void HashRemoveAllAsyncTest()
        {
            var test_key = "test_hash_remove_all";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var delResult = _RedisService.HashRemoveAllAsync(test_key).Result;
            Assert.True(delResult);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.Null(hashValue);

            hashValue = _RedisService.HashGetAsync<string>(test_key, 9).Result;
            Assert.Null(hashValue);
        }

        [Fact(DisplayName = "判断Hash中是否存在指定Field")]
        public void HashExistsAsyncTest()
        {
            var test_key = "test_hash_remove_exist";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var existResult = _RedisService.HashExistsAsync(test_key, 1).Result;
            Assert.True(existResult);

            _RedisService.HashRemoveAsync(test_key, 1).Wait();
            existResult = _RedisService.HashExistsAsync(test_key, 1).Result;
            Assert.False(existResult);

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "获取Hash的总数量")]
        public async Task HashCountAsyncTest()
        {
            var test_key = "test_hash_remove_count";

            await _RedisService.HashRemoveAllAsync(test_key);

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            await _RedisService.HashRemoveAsync(test_key, 1);
            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(9, hashCount);

            await _RedisService.HashRemoveAsync(test_key, new List<RedisValue>() { 2, 3 });
            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(7, hashCount);

            await _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "根据Field获取Hash值")]
        public async Task HashGetAsyncTest()
        {
            var test_key = "test_hash_get_by_field";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.Equal("第1条测试数据", hashValue);

            hashValue = _RedisService.HashGetAsync<string>(test_key, 8).Result;
            Assert.Equal("第8条测试数据", hashValue);

            await _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "根据Field集合获取Hash值集合")]
        public void HashGetAsyncRangeTest()
        {
            var test_key = "test_hash_get_by_field_range";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var hashValues = _RedisService.HashGetAsync<string>(test_key, new List<RedisValue>() { 1, 4, 7 }).Result.ToList();
            Assert.Equal(3, hashValues.Count());
            Assert.True(hashValues.Contains("第1条测试数据"));
            Assert.True(hashValues.Contains("第4条测试数据"));
            Assert.True(hashValues.Contains("第7条测试数据"));
            Assert.False(hashValues.Contains("第8条测试数据"));

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "获取全部Hash数据")]
        public void HashGetAllAsyncTest()
        {
            var test_key = "test_hash_get_all";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var hashValues = _RedisService.HashGetAllAsync<string>(test_key).Result.ToList();

            Assert.Equal(10, hashValues.Count());
            Assert.Contains("第1条测试数据", hashValues);
            Assert.Contains("第4条测试数据", hashValues);
            Assert.Contains("第7条测试数据", hashValues);
            Assert.DoesNotContain("第11条测试数据", hashValues);

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "获取全部Hash数据，并以HashEntry数组返回")]
        public void HashGetAllAsyncBackWithHashEntryArrayTest()
        {
            var test_key = "test_hash_get_all";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            var hashEntries = _RedisService.HashGetAllAsync(test_key).Result.ToList();

            Assert.Equal(10, hashEntries.Count());

            var firstValue = hashEntries.First();
            Assert.Equal("0", firstValue.Name);
            Assert.Equal("第0条测试数据", JsonManager.Deserialize<string>(firstValue.Value));

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "Hash过期时间点")]
        public void HashExpireAtAsyncTest()
        {
            var test_key = "test_hash_expire_at";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            _RedisService.HashExpireAtAsync(test_key, DateTime.Now.AddSeconds(5));


            Task.Factory.StartNew(() =>
            {
                Task.Delay(6).Wait();

                hashCount = _RedisService.HashCountAsync(test_key).Result;
                Assert.Equal(0, hashCount);

                _RedisService.HashRemoveAllAsync(test_key).Wait();
            });
        }

        [Fact(DisplayName = "Hash过期时间段")]
        public void HashExpireInAsyncTest()
        {
            var test_key = "test_hash_expire_in";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0;i < 10;i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(10, hashCount);

            _RedisService.HashExpireInAsync(test_key, new TimeSpan(0, 0, 5));

            Task.Factory.StartNew(() =>
            {
                Task.Delay(6).Wait();

                hashCount = _RedisService.HashCountAsync(test_key).Result;
                Assert.Equal(0, hashCount);

                _RedisService.HashRemoveAllAsync(test_key).Wait();
            });
        }
    }
}
