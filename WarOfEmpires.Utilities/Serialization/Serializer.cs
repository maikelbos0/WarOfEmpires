using Newtonsoft.Json;

namespace WarOfEmpires.Utilities.Serialization {
    public sealed class Serializer : ISerializer {
        public string SerializeToJson(object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}