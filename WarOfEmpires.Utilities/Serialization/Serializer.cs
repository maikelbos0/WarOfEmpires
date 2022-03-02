using System.Text.Json;

namespace WarOfEmpires.Utilities.Serialization {
    public sealed class Serializer : ISerializer {
        public string SerializeToJson(object obj) => JsonSerializer.Serialize(obj);
    }
}