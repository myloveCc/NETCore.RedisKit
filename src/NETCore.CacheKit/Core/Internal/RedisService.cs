using NETCore.RedisKit.Shared;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.RedisKit.Core.Internal
{
    public class RedisService : IRedisService
    {
        private readonly IRedisProvider _RedisProvider;

        public RedisService(IRedisProvider redisProvider)
        {
            _RedisProvider = redisProvider;
        }

        #region Sync

        #region Basic

        /// <summary>
        /// 自增（将值加1）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回自增之后的值</returns>
        public long Increment(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringIncrement(key, 1, flags);
            }
        }

        /// <summary>
        /// 在原有值上加操作(long)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待加值（long）</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回加后结果</returns>
        public long Increment(RedisKey key, long value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringIncrement(key, value, flags);
            }
        }

        /// <summary>
        /// 在原有值上加操作(double)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待加值（double）</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public double Increment(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringIncrement(key, value, flags);
            }
        }

        /// <summary>
        /// 自减（将值减1）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public long Decrement(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringDecrement(key, 1, flags);
            }
        }

        /// <summary>
        /// 在原有值上减操作(long)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待减值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回减后结果</returns>
        public long Decrement(RedisKey key, long value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringDecrement(key, value, flags);
            }
        }

        /// <summary>
        /// 在原有值上减操作(double)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待减值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回减后结果</returns>
        public double Decrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringDecrement(key, value, flags);
            }
        }

        /// <summary>
        /// 重命名一个Key，值不变
        /// </summary>
        /// <param name="oldKey">旧键</param>
        /// <param name="newKey">新键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public bool KeyRename(RedisKey oldKey, RedisKey newKey, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                if (oldKey.Equals(newKey) || !db.KeyExists(oldKey, flags))
                {
                    return false;
                }
                return db.KeyRename(oldKey, newKey, When.Always, flags);
            }
        }

        /// <summary>
        /// 获取Key对应Redis的类型
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public RedisType KeyType(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyType(key, flags);
            }
        }

        /// <summary>
        /// 判断Key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExists(key, flags);
            }
        }

        #endregion

        #region String

        /// <summary>
        /// String Set操作（包括新增（key不存在）/更新(如果key已存在)）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>true 成功 false 失败</returns>
        public bool StringSet<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                RedisValue value = JsonSerialize(val);
                var db = redis.GetDatabase();

                return db.StringSet(key, value, null, when, flags);
            }
        }

        /// <summary>
        /// String Set操作（包括新增/更新）,同时可以设置过期时间点
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="expiresAt">过期时间点</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>true 成功 false 失败</returns>
        public bool StringSet<T>(RedisKey key, T val, DateTime expiresAt, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                RedisValue value = JsonSerialize(val);
                var db = redis.GetDatabase();
                var timeSpan = expiresAt.Subtract(DateTime.Now);
                return db.StringSet(key, value, timeSpan, when, flags);
            }
        }

        /// <summary>
        /// String Set操作（包括新增/更新）,同时可以设置过期时间段
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="expiresIn">过期时间段</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>true 成功 false 失败</returns>
        public bool StringSet<T>(RedisKey key, T val, TimeSpan expiresIn, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                RedisValue value = JsonSerialize(val);
                var db = redis.GetDatabase();
                return db.StringSet(key, value, expiresIn, when, flags);
            }
        }

        /// <summary>
        /// String Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns>如果key存在，找到对应Value,如果不存在，返回默认值.</returns>
        public T StringGet<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = db.StringGet(key, flags);

                if (value.IsNullOrEmpty)
                {
                    return default(T);
                }
                return JsonDserialize<T>(value);
            }
        }

        /// <summary>
        /// String Get 操作（获取多条）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">键集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> StringGet<T>(IEnumerable<RedisKey> keys, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var result = new List<T>();
                if (keys != null && keys.Any())
                {
                    var db = redis.GetDatabase();
                    RedisValue[] values = db.StringGet(keys.ToArray(), flags);
                    if (values != null && values.Length > 0)
                    {
                        values.ForEach(x =>
                        {
                            if (!x.IsNullOrEmpty)
                            {
                                result.Add(JsonDserialize<T>(x));
                            }
                        });
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Sting Del 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>True if the key was removed. else false</returns>
        public bool StringRemove(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDelete(key, flags);
            }
        }

        /// <summary>
        /// Sting Del 操作（删除多条）
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long StringRemove(IEnumerable<RedisKey> keys, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var keyArray = keys.ToArray();
                return db.KeyDelete(keyArray, flags);
            }
        }

        #endregion

        #region List

        /// <summary>
        /// List Insert Left 操作，将val插入pivot位置的左边
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">待插入值</param>
        /// <param name="pivot">参考值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>返回插入左侧成功后List的长度 或 -1 表示pivot未找到.</returns>
        public long ListInsertLeft<T>(RedisKey key, T val, T pivot, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                RedisValue pivotValue = JsonSerialize(pivot);

                return db.ListInsertBefore(key, pivotValue, value, flags);
            }
        }

        /// <summary>
        /// List Insert Right 操作，，将val插入pivot位置的右边
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">待插入值</param>
        /// <param name="pivot">参考值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns> 返回插入右侧成功后List的长度 或 -1 表示pivot未找到.</returns>
        public long ListInsertRight<T>(RedisKey key, T val, T pivot, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                RedisValue pivotValue = JsonSerialize(pivot);

                return db.ListInsertAfter(key, pivotValue, value, flags);
            }
        }

        /// <summary>
        /// List Left Push  操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>Push操作之后，List的长度</returns>
        public long ListLeftPush<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.ListLeftPush(key, value, when, flags);
            }
        }

        /// <summary>
        /// List Left Push 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long ListLeftPushRanage<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                if (vals == null || !vals.Any())
                {
                    return 0;
                }
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });

                return db.ListLeftPush(key, values, flags);
            }
        }

        /// <summary>
        /// List Right Push 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long ListRightPush<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.ListRightPush(key, value, when, flags);
            }
        }

        /// <summary>
        /// List Right Push 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long ListRightPushRanage<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                if (vals == null || !vals.Any())
                {
                    return 0;
                }
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });
                return db.ListRightPush(key, values, flags);
            }
        }

        /// <summary>
        /// List Left Pop 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public T ListLeftPop<T>(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = db.ListLeftPop(key, flags);
                if (!value.IsNullOrEmpty)
                {
                    return JsonDserialize<T>(value);
                }
                return default(T);
            }
        }

        /// <summary>
        /// List Right Pop 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public T ListRightPop<T>(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = db.ListRightPop(key, flags);
                if (!value.IsNullOrEmpty)
                {
                    return JsonDserialize<T>(value);
                }
                return default(T);
            }
        }

        /// <summary>
        /// List Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>the number of removed elements</returns>
        public long ListRemove<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.ListRemove(key, value, 0, flags);
            }
        }

        /// <summary>
        /// List Remove All 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool ListRemoveAll(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDelete(key, flags);
            }
        }

        /// <summary>
        /// List Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public long ListCount(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.ListLength(key, flags);
            }
        }

        /// <summary>
        /// List Get By Index 操作 (Index 0表示左侧第一个,-1表示右侧第一个)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public T ListGetByIndex<T>(RedisKey key, long index, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = db.ListGetByIndex(key, index, flags);
                if (!value.IsNullOrEmpty)
                {
                    return JsonDserialize<T>(value);
                }
                return default(T);
            }
        }

        /// <summary>
        /// List Get All 操作(注意：从左往右取值)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> ListGetAll<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.ListRange(key, 0, -1, flags);
                var result = new List<T>();
                if (values.Any())
                {
                    values.ToList().ForEach(x =>
                    {
                        if (!x.IsNullOrEmpty)
                        {
                            result.Add(JsonDserialize<T>(x));
                        }
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// List Get Range 操作(注意：从左往右取值)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startIndex">开始索引 从0开始</param>
        /// <param name="stopIndex">结束索引 -1表示结尾</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> ListGetRange<T>(RedisKey key, long startIndex, long stopIndex, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.ListRangeAsync(key, startIndex, stopIndex, flags).Result;
                var result = new List<T>();
                if (values.Any())
                {
                    values.ToList().ForEach(x =>
                    {
                        if (!x.IsNullOrEmpty)
                        {
                            result.Add(JsonDserialize<T>(x));
                        }
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// List Expire At 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool ListExpireAt(RedisKey key, DateTime expireAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expireAt, flags);
            }
        }

        /// <summary>
        /// 设置List缓存过期
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool ListExpireIn(RedisKey key, TimeSpan expireIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expireIn, flags);
            }
        }

        #endregion

        #region Set

        /// <summary>
        /// Set Add 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>如果值不存在，则添加到集合，返回True否则返回False</returns>
        public bool SetAdd<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetAdd(key, value, flags);
            }
        }

        /// <summary>
        /// Set Add 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>添加值到集合，如果存在重复值，则不添加，返回添加的总数</returns>
        public long SetAddRanage<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return 0;
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });

                return db.SetAdd(key, values, flags);
            }
        }

        /// <summary>
        /// Set Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>如果值从Set集合中移除返回True，否则返回False</returns>
        public bool SetRemove<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetRemove(key, value, flags);
            }
        }

        /// <summary>
        /// Set Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SetRemoveRange<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return 0;
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });
                return db.SetRemove(key, values, flags);
            }
        }

        /// <summary>
        /// Set Remove All 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns> True if the key was removed.</returns>
        public bool SetRemoveAll(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDelete(key, flags);
            }
        }

        /// <summary>
        /// Set Combine 操作(可以求多个集合并集/交集/差集)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">多个集合的key<see cref="IEnumerable{T}"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public IEnumerable<T> SetCombine<T>(IEnumerable<RedisKey> keys, SetOperation operation, CommandFlags flags = CommandFlags.PreferSlave)
        {
            var result = new List<T>();
            if (keys == null || !keys.Any())
            {
                return result;
            }

            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.SetCombine(operation, keys.ToArray(), flags);

                if (values != null && values.Any())
                {
                    values.ToList().ForEach(x =>
                    {
                        if (!x.IsNullOrEmpty)
                        {
                            result.Add(JsonDserialize<T>(x));
                        }
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// Set Combine 操作(可以求2个集合并集/交集/差集)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="firstKey">第一个Set的Key</param>
        /// <param name="sencondKey">第二个Set的Key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>list with members of the resulting set.</returns>
        public IEnumerable<T> SetCombine<T>(RedisKey firstKey, RedisKey sencondKey, SetOperation operation, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.SetCombine(operation, firstKey, sencondKey, flags);

                var result = new List<T>();

                if (values != null && values.Any())
                {
                    values.ToList().ForEach(x =>
                    {
                        if (!x.IsNullOrEmpty)
                        {
                            result.Add(JsonDserialize<T>(x));
                        }
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// Set Combine And Store In StoreKey Set 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="storeKey">新集合Id</param>
        /// <param name="soureKeys">多个集合的key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SetCombineStore<T>(RedisKey storeKey, IEnumerable<RedisKey> soureKeys, SetOperation operation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (soureKeys == null || !soureKeys.Any())
            {
                return 0;
            }

            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SetCombineAndStore(operation, storeKey, soureKeys.ToArray(), flags);
            }
        }

        /// <summary>
        /// Set Combine And Store In StoreKey Set 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="storeKey">新集合Id</param>
        /// <param name="firstKey">第一个集合Key</param>
        /// <param name="secondKey">第二个集合Key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SetCombineStore<T>(RedisKey storeKey, RedisKey firstKey, RedisKey secondKey, SetOperation operation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SetCombineAndStore(operation, storeKey, firstKey, secondKey, flags);
            }
        }

        /// <summary>
        /// Set Move 操作（将元素从soure移动到destination）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sourceKey">数据源集合</param>
        /// <param name="destinationKey">待添加集合</param>
        /// <param name="val">待移动元素</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool SetMove<T>(RedisKey sourceKey, RedisKey destinationKey, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetMove(sourceKey, destinationKey, value, flags);
            }
        }
        /// <summary>
        /// Set Exists 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public bool SetExists<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetContains(key, value, flags);
            }
        }

        /// <summary>
        ///  Set Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public long SetCount(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SetLength(key, flags);
            }
        }

        /// <summary>
        /// Set Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> SetGetAll<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = db.SetMembers(key);
                var result = new List<T>();

                if (values != null && values.Any())
                {
                    values.ToList().ForEach(x =>
                    {
                        if (!x.IsNullOrEmpty)
                        {
                            result.Add(JsonDserialize<T>(x));
                        }
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// Set Expire At 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool SetExpireAt(RedisKey key, DateTime expireAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expireAt, flags);
            }
        }
        /// <summary>
        /// Set Expire In 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool SetExpireIn(RedisKey key, TimeSpan expireIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expireIn, flags);
            }
        }
        #endregion

        #region SortedSet

        /// <summary>
        /// SortedSet Add 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool SortedSetAdd<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetAdd(key, value, score, flags);
            }
        }
        /// <summary>
        /// SortedSet Add 操作（多条）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">待添加值集合<see cref="SortedSetEntry"/></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long SortedSetAdd(RedisKey key, IEnumerable<SortedSetEntry> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return 0;
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                SortedSetEntry[] values = vals.ToArray();
                return db.SortedSetAdd(key, values, flags);
            }
        }
        /// <summary>
        /// SortedSet Increment Score 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>Incremented score</returns>
        public double SortedSetIncrementScore<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetIncrement(key, value, score, flags);
            }
        }

        /// <summary>
        /// SortedSet Decrement Score 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>Decremented score</returns>
        public double SortedSetDecrementScore<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetDecrement(key, value, score, flags);
            }
        }

        /// <summary>
        /// Sorted Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool SortedSetRemove<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetRemove(key, value, flags);
            }
        }
        /// <summary>
        /// Sorted Remove 操作(删除多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SortedSetRemoveRanage<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return 0;
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });

                return db.SortedSetRemove(key, values, flags);
            }
        }
        /// <summary>
        /// Sorted Remove 操作(根据索引区间删除,索引值按Score由小到大排序)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="startIndex">开始索引，0表示第一项</param>
        /// <param name="stopIndex">结束索引，-1标识倒数第一项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SortedSetRemove(RedisKey key, long startIndex, long stopIndex, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRemoveRangeByRank(key, startIndex, stopIndex, flags);
            }
        }
        /// <summary>
        /// Sorted Remove 操作(根据Score区间删除，同时根据exclue<see cref="Exclude"/>排除删除项)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startScore">开始Score</param>
        /// <param name="stopScore">结束Score</param>
        /// <param name="exclue">排除项<see cref="Exclude"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>the number of elements removed.</returns>
        public long SortedSetRemove(RedisKey key, double startScore, double stopScore, Exclude exclue, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRemoveRangeByScore(key, startScore, stopScore, exclue, flags);
            }
        }

        /// <summary>
        /// Sorted Remove All 操作(删除全部)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>thre reuslt of all sorted set removed</returns>
        public bool SortedSetRemoveAll(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDelete(key, flags);
            }
        }

        /// <summary>
        /// Sorted Set Trim 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="size">保留条数</param>
        /// <param name="order">根据order<see cref="Order"/>来保留指定区间，如保留前100名，保留后100名</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>移除元素数量</returns>
        public long SortedSetTrim(RedisKey key, long size, Order order = Order.Descending, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();

                if (order == Order.Ascending)
                {
                    return db.SortedSetRemoveRangeByRank(key, size, -1, flags);
                }
                else
                {
                    return db.SortedSetRemoveRangeByRank(key, 0, -size - 1, flags);
                }
            }
        }

        /// <summary>
        /// Sorted Set Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SortedSetCount(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetLength(key, double.NegativeInfinity, double.PositiveInfinity, Exclude.None, flags);
            }
        }

        /// <summary>
        /// Sorted Set Exists 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public bool SortedSetExists<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetScore(key, value, flags) != null;
            }
        }

        /// <summary>
        /// SortedSet Pop Min Score Element 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public T SortedSetGetMinByScore<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.SortedSetRangeByRank(key, 0, 1, Order.Ascending, flags);

                if (values != null && values.Any())
                {
                    return JsonDserialize<T>(values.First());
                }
                return default(T);
            }
        }

        /// <summary>
        /// SortedSet Pop Max Score Element 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public T SortedSetGetMaxByScore<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.SortedSetRangeByRank(key, 0, 1, Order.Descending, flags);

                if (values != null && values.Any())
                {
                    return JsonDserialize<T>(values.First());
                }
                return default(T);
            }
        }

        /// <summary>
        /// Sorted Set Get Page List 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> SortedSetGetPageList<T>(RedisKey key, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.SortedSetRangeByRank(key, (page - 1) * pageSize, page * pageSize - 1, order, flags);
                var result = new List<T>();
                if (values != null && values.Length > 0)
                {
                    foreach (var value in values)
                    {
                        if (!value.IsNullOrEmpty)
                        {
                            var data = JsonDserialize<T>(value);
                            result.Add(data);
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Sorted Set Get Page List 操作(根据分数区间)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startScore">开始值</param>
        /// <param name="stopScore">停止值</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="exclude">排除规则<see cref="Exclude"/>,默认为None</param>
        /// <returns></returns>
        public IEnumerable<T> SortedSetGetPageList<T>(RedisKey key, double startScore, double stopScore, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, Exclude exclude = Exclude.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.SortedSetRangeByScore(key, startScore, stopScore, exclude, order, (page - 1) * pageSize, page * pageSize - 1, flags);
                var result = new List<T>();
                if (values != null && values.Length > 0)
                {
                    foreach (var value in values)
                    {
                        if (!value.IsNullOrEmpty)
                        {
                            var data = JsonDserialize<T>(value);
                            result.Add(data);
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Sorted Set Get Page List With Score 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public SortedSetEntry[] SortedSetGetPageListWithScore(RedisKey key, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRangeByRankWithScores(key, (page - 1) * pageSize, page * pageSize - 1, order, flags);
            }
        }
        /// <summary>
        /// Sorted Set Get Page List With Score 操作(根据分数区间)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startScore">开始值</param>
        /// <param name="stopScore">停止值</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="exclude">排除规则<see cref="Exclude"/>,默认为None</param>
        /// <returns></returns>
        public SortedSetEntry[] SortedSetGetPageListWithScore(RedisKey key, double startScore, double stopScore, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, Exclude exclude = Exclude.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRangeByScoreWithScores(key, startScore, stopScore, exclude, order, (page - 1) * pageSize, page * pageSize - 1, flags);
            }
        }

        /// <summary>
        /// SortedSet Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> SortedSetGetAll<T>(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.SortedSetRangeByRank(key, 0, -1, order, flags);
                var result = new List<T>();
                foreach (var value in values)
                {
                    if (!value.IsNullOrEmpty)
                    {
                        var data = JsonDserialize<T>(value);
                        result.Add(data);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Sorted Set Combine And Store 操作
        /// </summary>
        /// <param name="storeKey">存储SortedKey</param>
        /// <param name="combineKeys">待合并的Key集合<see cref="Array"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SortedSetCombineAndStore(RedisKey storeKey, RedisKey[] combineKeys, SetOperation setOperation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetCombineAndStore(setOperation, storeKey, combineKeys, null, Aggregate.Sum, flags);
            }
        }
        /// <summary>
        /// Sorted Set Combine And Store 操作
        /// </summary>
        /// <param name="storeKey">存储SortedKey</param>
        /// <param name="combineKeys">待合并的Key集合<see cref="IEnumerable{T}"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public long SortedSetCombineAndStore(RedisKey storeKey, IEnumerable<RedisKey> combineKeys, SetOperation setOperation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var keys = combineKeys.ToArray();
                return db.SortedSetCombineAndStore(setOperation, storeKey, keys, null, Aggregate.Sum, flags);
            }
        }
        /// <summary>
        ///  Sorted Set Expire At DeteTime 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool SortedSetExpireAt(RedisKey key, DateTime expiresAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expiresAt, flags);
            }
        }

        /// <summary>
        /// Sorted Set Expire In TimeSpan 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool SortedSetExpireIn(RedisKey key, TimeSpan expiresIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expiresIn, flags);
            }
        }
        #endregion

        #region Hash

        /// <summary>
        /// Hash Set 操作（新增/更新）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项Id</param>
        /// <param name="val">值</param>
        /// <param name="when">依据value的执行条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool HashSet<T>(RedisKey key, RedisValue hashField, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var value = JsonSerialize(val);
                return db.HashSet(key, hashField, value, when, flags);
            }
        }

        /// <summary>
        /// Hash Set 操作（新增/更新多条）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值集合<see cref="HashEntry"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public void HashSetRange(RedisKey key, IEnumerable<HashEntry> hashFields, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (hashFields == null || !hashFields.Any())
            {
                return;
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                db.HashSet(key, hashFields.ToArray(), flags);
            }
        }

        /// <summary>
        /// Hash Remove 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public bool HashRemove(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashDelete(key, hashField, flags);
            }
        }
        /// <summary>
        /// Hash Remove 操作(删除多条)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项集合<see cref="Array"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public long HashRemove(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashDelete(key, hashFields, flags);
            }
        }

        /// <summary>
        /// Hash Remove 操作(删除多条)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项集合<see cref="IEnumerable{T}"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public long HashRemove(RedisKey key, IEnumerable<RedisValue> hashFields, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (hashFields == null || !hashFields.Any())
            {
                return 0;
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashDelete(key, hashFields.ToArray(), flags);
            }
        }

        /// <summary>
        /// Hash Remove All 操作(删除全部)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public bool HashRemoveAll(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDelete(key, flags);
            }
        }

        /// <summary>
        /// Hash Exists 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public bool HashExists(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashExists(key, hashField, flags);
            }

        }
        /// <summary>
        /// Hash Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public long HashCount(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashLength(key, flags);
            }
        }

        /// <summary>
        /// Hash Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public T HashGet<T>(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var value = db.HashGet(key, hashField, flags);
                if (!value.IsNullOrEmpty)
                {
                    return JsonDserialize<T>(value);
                }
                return default(T);
            }
        }

        /// <summary>
        /// Hash Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashFields">hash项集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> HashGet<T>(RedisKey key, IEnumerable<RedisValue> hashFields, CommandFlags flags = CommandFlags.PreferSlave)
        {
            var result = new List<T>();
            if (hashFields != null && hashFields.Any())
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.HashGet(key, hashFields.ToArray(), flags);

                    if (values != null && values.Length > 0)
                    {
                        values.ForEach(x =>
                        {
                            if (!x.IsNullOrEmpty)
                            {
                                result.Add(JsonDserialize<T>(x));
                            }
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Hash Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public IEnumerable<T> HashGetAll<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            var result = new List<T>();
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var values = db.HashValues(key, flags);

                if (values != null && values.Length > 0)
                {
                    values.ForEach(x =>
                    {
                        if (!x.IsNullOrEmpty)
                        {
                            result.Add(JsonDserialize<T>(x));
                        }
                    });
                }
                return result;
            }
        }

        /// <summary>
        /// Hash Get All 操作 (返回HashEntry)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public HashEntry[] HashGetAll(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashGetAll(key, flags);
            }
        }

        /// <summary>
        ///  Hahs Expire At DeteTime 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool HashExpireAt(RedisKey key, DateTime expiresAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expiresAt, flags);
            }
        }

        /// <summary>
        /// Hash Expire In TimeSpan 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public bool HashExpireIn(RedisKey key, TimeSpan expiresIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpire(key, expiresIn, flags);
            }
        }
        #endregion

        #endregion

        #region Async

        #region Basic
        /// <summary>
        /// 自增（将值加1）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回自增之后的值</returns>
        public Task<long> IncrementAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringIncrementAsync(key, 1, flags);
            }
        }

        /// <summary>
        /// 在原有值上加操作(long)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待加值（long）</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回加后结果</returns>
        public Task<long> IncrementAsync(RedisKey key, long value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringIncrementAsync(key, value, flags);
            }
        }

        /// <summary>
        /// 在原有值上加操作(double)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待加值（double）</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public Task<double> IncrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringIncrementAsync(key, value, flags);
            }
        }

        /// <summary>
        /// 自减（将值减1）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public Task<long> DecrementAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringDecrementAsync(key, 1, flags);
            }

        }
        /// <summary>
        /// 在原有值上减操作(long)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待减值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回减后结果</returns>
        public Task<long> DecrementAsync(RedisKey key, long value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringDecrementAsync(key, value, flags);
            }
        }

        /// <summary>
        /// 在原有值上减操作(double)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">待减值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns>返回减后结果</returns>
        public Task<double> DecrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringDecrementAsync(key, value, flags);
            }
        }

        /// <summary>
        /// 重命名一个Key，值不变
        /// </summary>
        /// <param name="oldKey">旧键</param>
        /// <param name="newKey">新键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public Task<bool> KeyRenameAsync(RedisKey oldKey, RedisKey newKey, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                if (oldKey.Equals(newKey) || !db.KeyExists(oldKey, flags))
                {
                    return Task<bool>.FromResult(false);
                }
                return db.KeyRenameAsync(oldKey, newKey, When.Always, flags);
            }
        }

        /// <summary>
        /// 获取Key对应Redis的类型
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public Task<RedisType> KeyTypeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyTypeAsync(key, flags);
            }
        }

        /// <summary>
        /// 判断Key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/></param>
        /// <returns></returns>
        public Task<bool> KeyExistsAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExistsAsync(key, flags);
            }
        }


        #endregion

        #region String

        /// <summary>
        /// String Set操作（包括新增（key不存在）/更新(如果key已存在)）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> StringSetAsync<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            RedisValue value = JsonSerialize(val);
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();

                return db.StringSetAsync(key, value, null, when, flags);
            }
        }

        /// <summary>
        /// String Set操作（包括新增/更新）,同时可以设置过期时间段
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="expiresAt">过期时间段</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> StringSetAsync<T>(RedisKey key, T val, DateTime expiresAt, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            RedisValue value = JsonSerialize(val);
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var timeSpan = expiresAt.Subtract(DateTime.Now);
                return db.StringSetAsync(key, value, timeSpan, when, flags);
            }
        }

        /// <summary>
        /// String Set操作（包括新增/更新）,同时可以设置过期时间点
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="expiresIn">过期时间点</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>true 成功 false 失败</returns>
        public Task<bool> StringSetAsync<T>(RedisKey key, T val, TimeSpan expiresIn, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            RedisValue value = JsonSerialize(val);
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.StringSetAsync(key, value, expiresIn, when, flags);
            }
        }

        /// <summary>
        /// String Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns>如果key存在，找到对应Value,如果不存在，返回默认值.</returns>
        public Task<T> StringGetAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    RedisValue value = db.StringGet(key, flags);

                    if (value.IsNullOrEmpty)
                    {
                        return default(T);
                    }
                    return JsonDserialize<T>(value);
                }
            });

        }

        /// <summary>
        /// String Get 操作（获取多条）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">键集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> StringGetAsync<T>(IEnumerable<RedisKey> keys, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                var result = new List<T>();
                if (keys != null && keys.Any())
                {
                    using (var redis = _RedisProvider.Redis)
                    {
                        var db = redis.GetDatabase();
                        RedisValue[] values = db.StringGet(keys.ToArray(), flags);
                        if (values != null && values.Length > 0)
                        {
                            values.ForEach(x =>
                            {
                                if (!x.IsNullOrEmpty)
                                {
                                    result.Add(JsonDserialize<T>(x));
                                }
                            });
                        }
                    }
                }
                return result;
            });
        }

        /// <summary>
        /// Sting Del 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>True if the key was removed. else false</returns>
        public Task<bool> StringRemoveAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDeleteAsync(key, flags);
            }
        }

        /// <summary>
        /// Sting Del 操作（删除多条）
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> StringRemoveAsync(IEnumerable<RedisKey> keys, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var keyArray = keys.ToArray();
                return db.KeyDeleteAsync(keyArray, flags);
            }
        }

        #endregion

        #region List

        /// <summary>
        /// List Insert Left 操作，将val插入pivot位置的左边
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">待插入值</param>
        /// <param name="pivot">参考值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>返回插入左侧成功后List的长度 或 -1 表示pivot未找到.</returns>
        public Task<long> ListInsertLeftAsync<T>(RedisKey key, T val, T pivot, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                RedisValue pivotValue = JsonSerialize(pivot);

                return db.ListInsertBeforeAsync(key, pivotValue, value, flags);
            }
        }

        /// <summary>
        /// List Insert Right 操作，，将val插入pivot位置的右边
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">待插入值</param>
        /// <param name="pivot">参考值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns> 返回插入右侧成功后List的长度 或 -1 表示pivot未找到.</returns>
        public Task<long> ListInsertRightAsync<T>(RedisKey key, T val, T pivot, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                RedisValue pivotValue = JsonSerialize(pivot);

                return db.ListInsertAfterAsync(key, pivotValue, value, flags);
            }
        }

        /// <summary>
        /// List Left Push  操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>Push操作之后，List的长度</returns>
        public Task<long> ListLeftPushAsync<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.ListLeftPushAsync(key, value, when, flags);
            }
        }

        /// <summary>
        /// List Left Push 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> ListLeftPushRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return Task.FromResult<long>(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });

                return db.ListLeftPushAsync(key, values, flags);
            }
        }

        /// <summary>
        /// List Right Push 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="when">操作前置条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> ListRightPushAsync<T>(RedisKey key, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.ListRightPushAsync(key, value, when, flags);
            }
        }

        /// <summary>
        /// List Right Push 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> ListRightPushRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return Task.FromResult<long>(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });
                return db.ListRightPushAsync(key, values, flags);
            }
        }

        /// <summary>
        /// List Left Pop 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<T> ListLeftPopAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    RedisValue value = db.ListLeftPopAsync(key, flags).Result;
                    if (!value.IsNullOrEmpty)
                    {
                        return JsonDserialize<T>(value);
                    }
                    return default(T);
                }
            });
        }

        /// <summary>
        /// List Right Pop 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<T> ListRightPopAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    RedisValue value = db.ListRightPopAsync(key, flags).Result;
                    if (!value.IsNullOrEmpty)
                    {
                        return JsonDserialize<T>(value);
                    }
                    return default(T);
                }
            });
        }

        /// <summary>
        /// List Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>the number of removed elements</returns>
        public Task<long> ListRemoveAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);

                return db.ListRemoveAsync(key, value, 0, flags);
            }
        }

        /// <summary>
        /// List Remove All 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> ListRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDeleteAsync(key, flags);
            }
        }

        /// <summary>
        /// List Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<long> ListCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.ListLengthAsync(key, flags);
            }
        }

        /// <summary>
        /// List Get By Index 操作 (Index 0表示左侧第一个,-1表示右侧第一个)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<T> ListGetByIndexAsync<T>(RedisKey key, long index, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    RedisValue value = db.ListGetByIndexAsync(key, index, flags).Result;
                    if (!value.IsNullOrEmpty)
                    {
                        return JsonDserialize<T>(value);
                    }
                    return default(T);
                }
            });

        }

        /// <summary>
        /// List Get All 操作(注意：从左往右取值)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> ListGetAllAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.ListRangeAsync(key, 0, -1, flags).Result;
                    var result = new List<T>();
                    if (values.Any())
                    {
                        values.ToList().ForEach(x =>
                        {
                            if (!x.IsNullOrEmpty)
                            {
                                result.Add(JsonDserialize<T>(x));
                            }
                        });
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// List Get Range 操作(注意：从左往右取值)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startIndex">开始索引 从0开始</param>
        /// <param name="stopIndex">结束索引 -1表示结尾</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> ListGetRangeAsync<T>(RedisKey key, long startIndex, long stopIndex, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.ListRangeAsync(key, startIndex, stopIndex, flags).Result;
                    var result = new List<T>();
                    if (values.Any())
                    {
                        values.ToList().ForEach(x =>
                        {
                            if (!x.IsNullOrEmpty)
                            {
                                result.Add(JsonDserialize<T>(x));
                            }
                        });
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// List Expire At 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> ListExpireAtAsync(RedisKey key, DateTime expireAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expireAt, flags);
            }
        }
        /// <summary>
        /// 设置List缓存过期
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> ListExpireInAsync(RedisKey key, TimeSpan expireIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expireIn, flags);
            }
        }
        #endregion

        #region Set

        /// <summary>
        /// Set Add 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>如果值不存在，则添加到集合，返回True否则返回False</returns>
        public Task<bool> SetAddAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetAddAsync(key, value, flags);
            }
        }

        /// <summary>
        /// Set Add 操作(添加多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>添加值到集合，如果存在重复值，则不添加，返回添加的总数</returns>
        public Task<long> SetAddRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return Task.FromResult<long>(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });

                return db.SetAddAsync(key, values, flags);
            }
        }

        /// <summary>
        /// Set Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>如果值从Set集合中移除返回True，否则返回False</returns>
        public Task<bool> SetRemoveAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetRemoveAsync(key, value, flags);
            }
        }

        /// <summary>
        /// Set Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SetRemoveRangeAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return Task.FromResult<long>(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });

                return db.SetRemoveAsync(key, values, flags);
            }
        }

        /// <summary>
        /// Set Remove All 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns> True if the key was removed.</returns>
        public Task<bool> SetRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDeleteAsync(key, flags);
            }
        }

        /// <summary>
        /// Set Combine 操作(可以求多个集合并集/交集/差集)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">多个集合的key<see cref="IEnumerable{T}"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> SetCombineAsync<T>(IEnumerable<RedisKey> keys, SetOperation operation, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                var result = new List<T>();
                if (keys == null || !keys.Any())
                {
                    return result;
                }

                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.SetCombineAsync(operation, keys.ToArray(), flags).Result;

                    if (values != null && values.Any())
                    {
                        values.ToList().ForEach(x =>
                        {
                            if (!x.IsNullOrEmpty)
                            {
                                result.Add(JsonDserialize<T>(x));
                            }
                        });
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// Set Combine 操作(可以求2个集合并集/交集/差集)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="firstKey">第一个Set的Key</param>
        /// <param name="sencondKey">第二个Set的Key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>list with members of the resulting set.</returns>
        public Task<IEnumerable<T>> SetCombineAsync<T>(RedisKey firstKey, RedisKey sencondKey, SetOperation operation, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.SetCombineAsync(operation, firstKey, sencondKey, flags).Result;

                    var result = new List<T>();

                    if (values != null && values.Any())
                    {
                        values.ToList().ForEach(x =>
                        {
                            if (!x.IsNullOrEmpty)
                            {
                                result.Add(JsonDserialize<T>(x));
                            }
                        });
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// Set Combine And Store In StoreKey Set 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="storeKey">新集合Id</param>
        /// <param name="soureKeys">多个集合的key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SetCombineStoreAsync<T>(RedisKey storeKey, IEnumerable<RedisKey> soureKeys, SetOperation operation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (soureKeys == null || !soureKeys.Any())
            {
                return Task.FromResult<long>(0);
            }

            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SetCombineAndStoreAsync(operation, storeKey, soureKeys.ToArray(), flags);
            }

        }
        /// <summary>
        /// Set Combine And Store In StoreKey Set 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="storeKey">新集合Id</param>
        /// <param name="firstKey">第一个集合Key</param>
        /// <param name="secondKey">第二个集合Key</param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SetCombineStoreAsync<T>(RedisKey storeKey, RedisKey firstKey, RedisKey secondKey, SetOperation operation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SetCombineAndStoreAsync(operation, storeKey, firstKey, secondKey, flags);
            }
        }

        /// <summary>
        /// Set Move 操作（将元素从soure移动到destination）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sourceKey">数据源集合</param>
        /// <param name="destinationKey">待添加集合</param>
        /// <param name="val">待移动元素</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> SetMoveAsync<T>(RedisKey sourceKey, RedisKey destinationKey, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetMoveAsync(sourceKey, destinationKey, value, flags);
            }
        }

        /// <summary>
        /// Set Exists 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<bool> SetExistsAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SetContainsAsync(key, value, flags);
            }
        }

        /// <summary>
        ///  Set Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<long> SetCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SetLengthAsync(key, flags);
            }
        }

        /// <summary>
        /// Set Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> SetGetAllAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    RedisValue[] values = db.SetMembersAsync(key).Result;
                    var result = new List<T>();

                    if (values != null && values.Any())
                    {
                        values.ToList().ForEach(x =>
                                                {
                                                    if (!x.IsNullOrEmpty)
                                                    {
                                                        result.Add(JsonDserialize<T>(x));
                                                    }
                                                });
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// Set Expire At 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> SetExpireAtAsync(RedisKey key, DateTime expireAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expireAt, flags);
            }

        }
        /// <summary>
        /// Set Expire In 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expireIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> SetExpireInAsync(RedisKey key, TimeSpan expireIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expireIn, flags);
            }
        }

        #endregion

        #region SortedSet

        /// <summary>
        /// SortedSet Add 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> SortedSetAddAsync<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetAddAsync(key, value, score, flags);
            }
        }

        /// <summary>
        /// SortedSet Add 操作（多条）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">待添加值集合<see cref="SortedSetEntry"/></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<long> SortedSetAddAsync(RedisKey key, IEnumerable<SortedSetEntry> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return Task.FromResult<long>(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                SortedSetEntry[] values = vals.ToArray();
                return db.SortedSetAddAsync(key, values, flags);
            }
        }

        /// <summary>
        /// SortedSet Increment Score 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>Incremented score</returns>
        public Task<double> SortedSetIncrementScoreAsync<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetIncrementAsync(key, value, score, flags);
            }
        }

        /// <summary>
        /// SortedSet Decrement Score 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="score">优先级</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>Decremented score</returns>
        public Task<double> SortedSetDecrementScoreAsync<T>(RedisKey key, T val, double score, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetDecrementAsync(key, value, score, flags);
            }
        }

        /// <summary>
        /// Sorted Remove 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> SortedSetRemoveAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue value = JsonSerialize(val);
                return db.SortedSetRemoveAsync(key, value, flags);
            }
        }

        /// <summary>
        /// Sorted Remove 操作(删除多条)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="vals">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SortedSetRemoveRanageAsync<T>(RedisKey key, IEnumerable<T> vals, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (vals == null || !vals.Any())
            {
                return Task.FromResult<long>(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                RedisValue[] values = new RedisValue[vals.Count()];
                var i = 0;
                vals.ForEach(x =>
                {
                    values[i] = JsonSerialize(x);
                    i++;
                });

                return db.SortedSetRemoveAsync(key, values, flags);
            }
        }

        /// <summary>
        /// Sorted Remove 操作(根据索引区间删除,索引值按Score由小到大排序)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="startIndex">开始索引，0表示第一项</param>
        /// <param name="stopIndex">结束索引，-1标识倒数第一项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SortedSetRemoveAsync(RedisKey key, long startIndex, long stopIndex, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRemoveRangeByRankAsync(key, startIndex, stopIndex, flags);
            }
        }
        /// <summary>
        /// Sorted Remove 操作(根据Score区间删除，同时根据exclue<see cref="Exclude"/>排除删除项)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startScore">开始Score</param>
        /// <param name="stopScore">结束Score</param>
        /// <param name="exclue">排除项<see cref="Exclude"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>the number of elements removed.</returns>
        public Task<long> SortedSetRemoveAsync(RedisKey key, double startScore, double stopScore, Exclude exclue, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRemoveRangeByScoreAsync(key, startScore, stopScore, exclue, flags);
            }
        }

        /// <summary>
        /// Sorted Remove All 操作(删除全部)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>thre reuslt of all sorted set removed</returns>
        public Task<bool> SortedSetRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDeleteAsync(key, flags);
            }
        }

        /// <summary>
        /// Sorted Set Trim 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="size">保留条数</param>
        /// <param name="order">根据order<see cref="Order"/>来保留指定区间，如保留前100名，保留后100名</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns>移除元素数量</returns>
        public Task<long> SortedSetTrimAsync(RedisKey key, long size, Order order = Order.Descending, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();

                if (order == Order.Ascending)
                {
                    return db.SortedSetRemoveRangeByRankAsync(key, size, -1, flags);
                }
                else
                {
                    return db.SortedSetRemoveRangeByRankAsync(key, 0, -size - 1, flags);
                }
            }
        }

        /// <summary>
        /// Sorted Set Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SortedSetCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();

                return db.SortedSetLengthAsync(key, double.NegativeInfinity, double.PositiveInfinity, Exclude.None, flags);
            }
        }

        /// <summary>
        /// Sorted Set Exists 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<bool> SortedSetExistsAsync<T>(RedisKey key, T val, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    RedisValue value = JsonSerialize(val);
                    return db.SortedSetScore(key, value, flags) != null;
                }
            });
        }

        /// <summary>
        /// SortedSet Pop Min Score Element 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<T> SortedSetGetMinByScoreAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.SortedSetRangeByRank(key, 0, 1, Order.Ascending, flags);

                    if (values != null && values.Any())
                    {
                        return JsonDserialize<T>(values.First());
                    }
                    return default(T);
                }
            });
        }

        /// <summary>
        /// SortedSet Pop Max Score Element 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<T> SortedSetGetMaxByScoreAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.SortedSetRangeByRank(key, 0, 1, Order.Descending, flags);

                    if (values != null && values.Any())
                    {
                        return JsonDserialize<T>(values.First());
                    }
                    return default(T);
                }
            });
        }

        /// <summary>
        /// Sorted Set Get Page List 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> SortedSetGetPageListAsync<T>(RedisKey key, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.SortedSetRangeByRank(key, (page - 1) * pageSize, page * pageSize - 1, order, flags);
                    var result = new List<T>();
                    if (values != null && values.Length > 0)
                    {
                        foreach (var value in values)
                        {
                            if (!value.IsNullOrEmpty)
                            {
                                var data = JsonDserialize<T>(value);
                                result.Add(data);
                            }
                        }
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// Sorted Set Get Page List 操作(根据分数区间)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startScore">开始值</param>
        /// <param name="stopScore">停止值</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="exclude">排除规则<see cref="Exclude"/>,默认为None</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> SortedSetGetPageListAsync<T>(RedisKey key, double startScore, double stopScore, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, Exclude exclude = Exclude.None)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.SortedSetRangeByScore(key, startScore, stopScore, exclude, order, (page - 1) * pageSize, page * pageSize - 1, flags);
                    var result = new List<T>();
                    if (values != null && values.Length > 0)
                    {
                        foreach (var value in values)
                        {
                            if (!value.IsNullOrEmpty)
                            {
                                var data = JsonDserialize<T>(value);
                                result.Add(data);
                            }
                        }
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// Sorted Set Get Page List With Score 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<SortedSetEntry[]> SortedSetGetPageListWithScoreAsync(RedisKey key, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRangeByRankWithScoresAsync(key, (page - 1) * pageSize, page * pageSize - 1, order, flags);
            }
        }

        /// <summary>
        /// Sorted Set Get Page List With Score 操作(根据分数区间)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="startScore">开始值</param>
        /// <param name="stopScore">停止值</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <param name="exclude">排除规则<see cref="Exclude"/>,默认为None</param>
        /// <returns></returns>
        public Task<SortedSetEntry[]> SortedSetGetPageListWithScoreAsync(RedisKey key, double startScore, double stopScore, int page, int pageSize, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave, Exclude exclude = Exclude.None)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetRangeByScoreWithScoresAsync(key, startScore, stopScore, exclude, order, (page - 1) * pageSize, page * pageSize - 1, flags);
            }
        }

        /// <summary>
        /// SortedSet Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="order">排序规则<see cref="Order"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> SortedSetGetAllAsync<T>(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.SortedSetRangeByRank(key, 0, -1, order, flags);
                    var result = new List<T>();
                    foreach (var value in values)
                    {
                        if (!value.IsNullOrEmpty)
                        {
                            var data = JsonDserialize<T>(value);
                            result.Add(data);
                        }
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// Sorted Set Combine And Store 操作
        /// </summary>
        /// <param name="storeKey">存储SortedKey</param>
        /// <param name="combineKeys">待合并的Key集合<see cref="Array"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SortedSetCombineAndStoreAsync(RedisKey storeKey, RedisKey[] combineKeys, SetOperation setOperation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.SortedSetCombineAndStoreAsync(setOperation, storeKey, combineKeys, null, Aggregate.Sum, flags);
            }
        }

        /// <summary>
        /// Sorted Set Combine And Store 操作
        /// </summary>
        /// <param name="storeKey">存储SortedKey</param>
        /// <param name="combineKeys">待合并的Key集合<see cref="IEnumerable{T}"/></param>
        /// <param name="operation">合并类型<see cref="SetOperation"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<long> SortedSetCombineAndStoreAsync(RedisKey storeKey, IEnumerable<RedisKey> combineKeys, SetOperation setOperation, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var keys = combineKeys.ToArray();
                return db.SortedSetCombineAndStoreAsync(setOperation, storeKey, keys, null, Aggregate.Sum, flags);
            }
        }


        /// <summary>
        ///  Sorted Set Expire At DeteTime 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> SortedSetExpireAtAsync(RedisKey key, DateTime expiresAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expiresAt, flags);
            }
        }

        /// <summary>
        /// Sorted Set Expire In TimeSpan 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> SortedSetExpireInAsync(RedisKey key, TimeSpan expiresIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expiresIn, flags);
            }
        }
        #endregion

        #region Hash

        /// <summary>
        /// Hash Set 操作（新增/更新）
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项Id</param>
        /// <param name="val">值</param>
        /// <param name="when">依据value的执行条件<see cref="When"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> HashSetAsync<T>(RedisKey key, RedisValue hashField, T val, When when = When.Always, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                var value = JsonSerialize(val);
                return db.HashSetAsync(key, hashField, value, when, flags);
            }
        }

        /// <summary>
        /// Hash Set 操作（新增/更新多条）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值集合<see cref="HashEntry"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task HashSetRangeAsync(RedisKey key, IEnumerable<HashEntry> hashFields, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (hashFields == null || !hashFields.Any())
            {
                return Task.FromResult(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashSetAsync(key, hashFields.ToArray(), flags);
            }
        }

        /// <summary>
        /// Hash Remove 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<bool> HashRemoveAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashDeleteAsync(key, hashField, flags);
            }
        }

        /// <summary>
        /// Hash Remove 操作(删除多条)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项集合<see cref="Array"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<long> HashRemoveAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashDeleteAsync(key, hashFields, flags);
            }
        }

        /// <summary>
        /// Hash Remove 操作(删除多条)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项集合<see cref="IEnumerable{T}"/></param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<long> HashRemoveAsync(RedisKey key, IEnumerable<RedisValue> hashFields, CommandFlags flags = CommandFlags.DemandMaster)
        {
            if (hashFields == null || !hashFields.Any())
            {
                return Task.FromResult<long>(0);
            }
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashDeleteAsync(key, hashFields.ToArray(), flags);
            }
        }

        /// <summary>
        /// Hash Remove All 操作(删除全部)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<bool> HashRemoveAllAsync(RedisKey key, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyDeleteAsync(key, flags);
            }
        }

        /// <summary>
        /// Hash Exists 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<bool> HashExistsAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashExistsAsync(key, hashField, flags);
            }
        }

        /// <summary>
        /// Hash Count 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<long> HashCountAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashLengthAsync(key, flags);
            }
        }

        /// <summary>
        /// Hash Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashField">hash项</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<T> HashGetAsync<T>(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var value = db.HashGet(key, hashField, flags);
                    if (!string.IsNullOrEmpty(value))
                    {
                        return JsonDserialize<T>(value);
                    }
                    return default(T);
                }
            });
        }


        /// <summary>
        /// Hash Get 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashFields">hash项集合</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> HashGetAsync<T>(RedisKey key, IEnumerable<RedisValue> hashFields, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                var result = new List<T>();

                if (hashFields != null && hashFields.Any())
                {
                    using (var redis = _RedisProvider.Redis)
                    {
                        var db = redis.GetDatabase();
                        var values = db.HashGet(key, hashFields.ToArray(), flags);

                        if (values != null && values.Length > 0)
                        {
                            values.ForEach(x =>
                            {
                                if (!x.IsNullOrEmpty)
                                {
                                    result.Add(JsonDserialize<T>(x));
                                }
                            });
                        }
                    }
                }
                return result;
            });
        }

        /// <summary>
        /// Hash Get All 操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> HashGetAllAsync<T>(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            return Task<IEnumerable<T>>.Factory.StartNew(() =>
            {
                var result = new List<T>();

                using (var redis = _RedisProvider.Redis)
                {
                    var db = redis.GetDatabase();
                    var values = db.HashValues(key, flags);

                    if (values != null && values.Length > 0)
                    {
                        values.ForEach(x =>
                                                                                    {
                                                                                        if (!x.IsNullOrEmpty)
                                                                                        {
                                                                                            result.Add(JsonDserialize<T>(x));
                                                                                        }
                                                                                    });
                    }
                    return result;
                }
            });
        }

        /// <summary>
        /// Hash Get All 操作 (返回HashEntry)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为PreferSlave</param>
        /// <returns></returns>
        public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.PreferSlave)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.HashGetAllAsync(key, flags);
            }
        }

        /// <summary>
        ///  Hahs Expire At DeteTime 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresAt">DateTime失效点：到达该时间点，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> HashExpireAtAsync(RedisKey key, DateTime expiresAt, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expiresAt, flags);
            }
        }

        /// <summary>
        /// Hash Expire In TimeSpan 操作
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiresIn">TimeSpan失效点：经过该时间段，立即失效</param>
        /// <param name="flags">操作标识<see cref="CommandFlags"/>,默认为DemandMaster</param>
        /// <returns></returns>
        public Task<bool> HashExpireInAsync(RedisKey key, TimeSpan expiresIn, CommandFlags flags = CommandFlags.DemandMaster)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                return db.KeyExpireAsync(key, expiresIn, flags);
            }
        }
        #endregion

        #region Transition

        /// <summary>
        /// Redis 事务操作
        /// </summary>
        /// <param name="action">执行命令</param>
        /// <param name="asyncObjec">同步对象</param>
        /// <returns></returns>
        public Task<bool> TransactionAsync(Action<ITransaction> action, object asyncObjec = null)
        {
            using (var redis = _RedisProvider.Redis)
            {
                var db = redis.GetDatabase();
                //创建事务
                var transition = db.CreateTransaction(asyncObjec);
                //执行Action
                action(transition);
                //执行事务
                return transition.ExecuteAsync();
            }
        }
        #endregion

        #endregion

        #region private
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="value">序列化对象</param>
        /// <returns>序列化之后的json字符串</returns>
        private string JsonSerialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="value">json字符串</param>
        /// <returns>反序列化之后的对象</returns>
        private T JsonDserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        #endregion

    }
}
