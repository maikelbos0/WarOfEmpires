using Microsoft.AspNetCore.Http;

namespace WarOfEmpires.Extensions {
    public static class HttpRequestExtensions {
        public static bool IsAjaxRequest(this HttpRequest request) {
            return request?.Headers["X-Requested-With"].Equals("XMLHttpRequest") ?? false;
        }
    }
}
