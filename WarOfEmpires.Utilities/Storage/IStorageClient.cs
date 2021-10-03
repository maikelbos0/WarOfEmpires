using System.IO;

namespace WarOfEmpires.Utilities.Storage {
    public interface IStorageClient {
        void Store(string fileName, Stream content);
        void Delete(string fileName);
    }
}