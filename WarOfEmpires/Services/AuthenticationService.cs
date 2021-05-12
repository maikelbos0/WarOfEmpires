using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Services {
    [ScopedServiceImplementation(typeof(IAuthenticationService))]
    public sealed class AuthenticationService : IAuthenticationService {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated {
            get {
                return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public string Identity {
            get {
                return httpContextAccessor.HttpContext.User.Identity.Name;
            }
        }

        public async Task SignIn(string name) {
            var claims = new[] { new Claim(ClaimTypes.Name, name) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        public async Task SignOut() {
            await httpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}