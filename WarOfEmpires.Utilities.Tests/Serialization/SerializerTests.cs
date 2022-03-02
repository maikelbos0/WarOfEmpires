using WarOfEmpires.Utilities.Serialization;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarOfEmpires.Utilities.Tests.Serialization {
    [TestClass]
    public sealed class SerializerTests {
        public sealed class TestObject {
            public int Id { get; private set; }
            public string Value { get; private set; }

            public TestObject(int id, string value) {
                Id = id;
                Value = value;
            }
        }

        [TestMethod]
        public void Serializer_SerializeJson_Succeeds() {
            var serializer = new Serializer();
            var testObject = new TestObject(1, "test");

            serializer.SerializeToJson(testObject).Should().Be("{\"Id\":1,\"Value\":\"test\"}");
        }

        [TestMethod]
        public void Serializer_SerializeJson_Succeeds_For_Special_Characters() {
            var serializer = new Serializer();
            var testObject = new TestObject(1, "\"'\\<([{test}])>%&\r\n");

            serializer.SerializeToJson(testObject).Should().Be("{\"Id\":1,\"Value\":\"\\u0022\\u0027\\\\\\u003C([{test}])\\u003E%\\u0026\\r\\n\"}");
        }
    }
}