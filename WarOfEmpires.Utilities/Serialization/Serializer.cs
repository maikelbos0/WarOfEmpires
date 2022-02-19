using Newtonsoft.Json;
using VDT.Core.DependencyInjection.Attributes;

namespace WarOfEmpires.Utilities.Serialization {
    [TransientServiceImplementation(typeof(ISerializer))]
    public sealed class Serializer : ISerializer {
        public string SerializeToJson(object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}