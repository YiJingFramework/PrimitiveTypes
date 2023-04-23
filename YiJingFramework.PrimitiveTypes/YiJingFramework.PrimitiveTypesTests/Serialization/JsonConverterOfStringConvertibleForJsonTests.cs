using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YiJingFramework.PrimitiveTypes.Serialization.Tests
{
    [TestClass()]
    public class JsonConverterOfStringConvertibleForJsonTests
    {
        [JsonConverter(typeof(JsonConverterOfStringConvertibleForJson<PositiveValue>))]
        public class PositiveValue : IStringConvertibleForJson<PositiveValue>
        {
            private int value;
            public required int Value
            {
                get { return this.value; }
                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException(nameof(value));
                    this.value = value;
                }
            }

            public static bool FromStringForJson(
                string s, [MaybeNullWhen(false)] out PositiveValue result)
            {
                result = new PositiveValue() { Value = s.Length };
                return true;
            }

            public string ToStringForJson()
            {
                var r = Enumerable.Repeat("A", this.Value);
                return string.Concat(r);
            }
        }

        private class PositiveValues
        {
            public required PositiveValue Value1 { get; init; }
            public required PositiveValue Value2 { get; init; }
        }


        [TestMethod()]
        public void SerializationTest()
        {
            {
                var value = new PositiveValue() { Value = 10 };
                var serialized = JsonSerializer.Serialize(value);
                Assert.AreEqual("\"AAAAAAAAAA\"", serialized);
                var deserialized = JsonSerializer.Deserialize<PositiveValue>(serialized);
                Assert.AreEqual(10, deserialized?.Value);
            }

            {
                var value = new PositiveValues() {
                    Value1 = new() { Value = 10 },
                    Value2 = new() { Value = 20 },
                };
                var serialized = JsonSerializer.Serialize(value);
                Assert.AreEqual(
                    """
                    {"Value1":"AAAAAAAAAA","Value2":"AAAAAAAAAAAAAAAAAAAA"}
                    """, serialized);
                var deserialized = JsonSerializer.Deserialize<PositiveValues>(serialized);
                Assert.AreEqual(10, deserialized?.Value1?.Value);
                Assert.AreEqual(20, deserialized?.Value2?.Value);
            }

            {
                var value = new PositiveValue[]{
                    new() { Value = 10 },
                    new() { Value = 20 },
                };
                var serialized = JsonSerializer.Serialize(value);
                Assert.AreEqual(
                    """
                    ["AAAAAAAAAA","AAAAAAAAAAAAAAAAAAAA"]
                    """, serialized);
                var deserialized = JsonSerializer.Deserialize<PositiveValue[]>(serialized);
                Assert.AreEqual(2, deserialized?.Length);
                Assert.AreEqual(10, deserialized?[0]?.Value);
                Assert.AreEqual(20, deserialized?[1]?.Value);
            }
        }
    }
}