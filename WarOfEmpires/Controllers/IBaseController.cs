using System.Web.Mvc;

namespace WarOfEmpires.Controllers {
    public interface IBaseController {
        ViewEngineCollection ViewEngineCollection { get; }
        void AddResponseHeader(string name, string value);
    }
}