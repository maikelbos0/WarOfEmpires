using Newtonsoft.Json;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Serialization {
    [ScopedServiceImplementation(typeof(ISerializer))]
    public sealed class Serializer : ISerializer {
        public string SerializeToJson(object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}