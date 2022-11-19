using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace WarOfEmpires.Api.Services;

public sealed class IdentityService : IIdentityService {
    private readonly IHttpContextAccessor httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor) {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string Identity => (httpContextAccessor.HttpContext ?? throw new InvalidOperationException("Missing http context")).User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
}
