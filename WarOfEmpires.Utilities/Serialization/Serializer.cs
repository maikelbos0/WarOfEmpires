using WarOfEmpires.Utilities.Container;
using Newtonsoft.Json;

namespace WarOfEmpires.Utilities.Serialization {
    [InterfaceInjectable]
    public sealed class Serializer : ISerializer {
        public string SerializeToJson(object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}