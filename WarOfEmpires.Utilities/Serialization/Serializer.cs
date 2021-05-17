using Newtonsoft.Json;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Serialization {
    [TransientServiceImplementation(typeof(ISerializer))]
    public sealed class Serializer : ISerializer {
        public string SerializeToJson(object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}