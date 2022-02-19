using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using VDT.Core.DependencyInjection.Attributes;

namespace WarOfEmpires.Services {
    [ScopedServiceImplementation(typeof(IAuthenticationService))]
    public sealed class AuthenticationService : IAuthenticationService {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated {
            get {
                return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public string Identity {
            get {
                return _httpContextAccessor.HttpContext.User.Identity.Name;
            }
        }

        public async Task SignIn(string name) {
            var claims = new[] { new Claim(ClaimTypes.Name, name) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        public async Task SignOut() {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}