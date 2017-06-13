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
    public class _RedisService_Hash_Tests
    {
        private readonly IRedisService _RedisService;
        public _RedisService_Hash_Tests()
        {
            IRedisProvider redisProvider = new RedisProvider(new RedisKitOptions()
            {
                EndPoints = "127.0.0.1:6379"
            });

            _RedisService = new RedisService(redisProvider);
        }

        [Fact(DisplayName = "新增/更新Hash数据")]
        public void HashSetAsyncTest()
        {
            var test_key = "test_hash_set";

            _RedisService.HashRemoveAllAsync(test_key);

            var setResult = _RedisService.HashSetAsync(test_key, 1111111, "1111111").Result;
            var valueResult = _RedisService.HashGetAsync<string>(test_key, 1111111).Result;
            Assert.True(setResult);
            Assert.Equal(valueResult, "1111111");

            setResult = _RedisService.HashSetAsync(test_key, 2222222, "1111111").Result;
            valueResult = _RedisService.HashGetAsync<string>(test_key, 2222222).Result;
            Assert.True(setResult);
            Assert.Equal(valueResult, "1111111");

            //如果Field已存在，更新后返回false
            setResult = _RedisService.HashSetAsync(test_key, 1111111, "3333333").Result;
            valueResult = _RedisService.HashGetAsync<string>(test_key, 1111111).Result;
            Assert.False(setResult);
            Assert.Equal(valueResult, "3333333");

            var delResult = _RedisService.HashRemoveAllAsync(test_key).Result;
            Assert.True(delResult);
        }

        [Fact(DisplayName = "新增/更新Hash数据集合")]
        public void HashSetRangeAsyncTest()
        {
            var test_key = "test_hash_set_range";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.NotEmpty(hashValue);
            Assert.NotNull(hashValue);
            Assert.Equal(hashValue, "第1条测试数据");

            _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除Hash数据")]
        public void HashRemoveAsyncTest()
        {
            var test_key = "test_hash_remove";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var delResult = _RedisService.HashRemoveAsync(test_key, 1).Result;
            Assert.True(delResult);

            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 9);

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

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var delFilelds = new RedisValue[] { 1, 2, 3 };
            var delResult = _RedisService.HashRemoveAsync(test_key, delFilelds).Result;

            Assert.Equal(delResult, 3);

            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 7);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.Null(hashValue);
            hashValue = _RedisService.HashGetAsync<string>(test_key, 2).Result;
            Assert.Null(hashValue);
            hashValue = _RedisService.HashGetAsync<string>(test_key, 3).Result;
            Assert.Null(hashValue);

            _RedisService.HashRemoveAllAsync(test_key);
        }

        [Fact(DisplayName = "移除多条Hash数据，根据Fileld集合")]
        public void HashRemoveAsyncRangeByFileldListTest()
        {
            var test_key = "test_hash_remove_rang_by_fileld_list";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var delFilelds = new List<RedisValue>() { 1, 2, 3 };
            var delResult = _RedisService.HashRemoveAsync(test_key, delFilelds).Result;

            Assert.Equal(delResult, 3);

            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 7);

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

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

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

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var existResult = _RedisService.HashExistsAsync(test_key, 1).Result;
            Assert.True(existResult);

            _RedisService.HashRemoveAsync(test_key, 1).Wait();
            existResult = _RedisService.HashExistsAsync(test_key, 1).Result;
            Assert.False(existResult);

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "获取Hash的总数量")]
        public void HashCountAsyncTest()
        {
            var test_key = "test_hash_remove_count";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            _RedisService.HashRemoveAsync(test_key, 1);
            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 9);

            _RedisService.HashRemoveAsync(test_key, new List<RedisValue>() { 2, 3 });
            hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 7);

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "根据Field获取Hash值")]
        public void HashGetAsyncTest()
        {
            var test_key = "test_hash_get_by_field";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var hashValue = _RedisService.HashGetAsync<string>(test_key, 1).Result;
            Assert.Equal(hashValue, "第1条测试数据");

            hashValue = _RedisService.HashGetAsync<string>(test_key, 8).Result;
            Assert.Equal(hashValue, "第8条测试数据");

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "根据Field集合获取Hash值集合")]
        public void HashGetAsyncRangeTest()
        {
            var test_key = "test_hash_get_by_field_range";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var hashValues = _RedisService.HashGetAsync<string>(test_key, new List<RedisValue>() { 1, 4, 7 }).Result.ToList();
            Assert.Equal(hashValues.Count(), 3);
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

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var hashValues = _RedisService.HashGetAllAsync<string>(test_key).Result.ToList();

            Assert.Equal(hashValues.Count(), 10);
            Assert.True(hashValues.Contains("第1条测试数据"));
            Assert.True(hashValues.Contains("第4条测试数据"));
            Assert.True(hashValues.Contains("第7条测试数据"));
            Assert.False(hashValues.Contains("第11条测试数据"));

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "获取全部Hash数据，并以HashEntry数组返回")]
        public void HashGetAllAsyncBackWithHashEntryArrayTest()
        {
            var test_key = "test_hash_get_all";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            var hashEntries = _RedisService.HashGetAllAsync(test_key).Result.ToList();

            Assert.Equal(hashEntries.Count(), 10);

            var firstValue = hashEntries.First();
            Assert.NotNull(firstValue);
            Assert.Equal(firstValue.Name, "0");
            Assert.Equal(JsonManager.Deserialize<string>(firstValue.Value), "第0条测试数据");

            _RedisService.HashRemoveAllAsync(test_key).Wait();
        }

        [Fact(DisplayName = "Hash过期时间点")]
        public void HashExpireAtAsyncTest()
        {
            var test_key = "test_hash_expire_at";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            _RedisService.HashExpireAtAsync(test_key, DateTime.Now.AddSeconds(5));


            Task.Factory.StartNew(() => {
                Task.Delay(6).Wait();

                hashCount = _RedisService.HashCountAsync(test_key).Result;
                Assert.Equal(hashCount, 0);

                _RedisService.HashRemoveAllAsync(test_key).Wait();
            });
        }

        [Fact(DisplayName = "Hash过期时间段")]
        public void HashExpireInAsyncTest()
        {
            var test_key = "test_hash_expire_in";

            _RedisService.HashRemoveAllAsync(test_key).Wait();

            var values = new List<HashEntry>();

            for (int i = 0; i < 10; i++)
            {
                var hashEntry = new HashEntry(i, JsonManager.Serialize($"第{i}条测试数据"));
                values.Add(hashEntry);
            }

            _RedisService.HashSetRangeAsync(test_key, values).Wait();

            var hashCount = _RedisService.HashCountAsync(test_key).Result;
            Assert.Equal(hashCount, 10);

            _RedisService.HashExpireInAsync(test_key, new TimeSpan(0, 0, 5));

            Task.Factory.StartNew(() => {
                Task.Delay(6).Wait();

                hashCount = _RedisService.HashCountAsync(test_key).Result;
                Assert.Equal(hashCount, 0);

                _RedisService.HashRemoveAllAsync(test_key).Wait();
            });
        }
    }
}
