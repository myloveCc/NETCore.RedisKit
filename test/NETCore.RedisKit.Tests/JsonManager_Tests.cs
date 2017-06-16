using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NETCore.RedisKit.Tests
{
    public class JsonManager_Tests
    {
        [Fact]
        public void Serialize_Obj_Null_Test()
        {
            var result = JsonManager.Serialize<Test>(null);

            Assert.Empty(result);
        }

        [Fact]
        public void Serialize_Obj_NotNull_Test()
        {
            var result = JsonManager.Serialize<Test>(new Test());

            Assert.NotEmpty(result);
        }

        [Fact]
        public void Serialize_Int_Test()
        {
            var result = JsonManager.Serialize(1);

            Assert.Equal("1", result);
        }

        [Fact]
        public void Dserialize_Value_Empty_Test()
        {
            var result = JsonManager.Deserialize<Test>("");

            Assert.Null(result);

            result = JsonManager.Deserialize<Test>(string.Empty);

            Assert.Null(result);
        }

        [Fact]
        public void Dserialize_Value_Error_Test()
        {
            Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => JsonManager.Deserialize<Test>("1111"));
        }
    }

    class Test
    {
        public string Title { get; set; }
    }
}
