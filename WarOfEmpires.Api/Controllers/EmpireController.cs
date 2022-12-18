using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Api.Services;
using WarOfEmpires.Queries.Empires;

namespace WarOfEmpires.Api.Controllers;

[ApiController]
[Route(nameof(Routing.Empire))]
public class EmpireController : BaseController {
    public EmpireController(IMessageService messageService, IIdentityService identityService) : base(messageService, identityService) {
    }

    [HttpGet(nameof(Routing.Empire.ResourceHeader))]
    public IActionResult ResourceHeader()
        => Ok(messageService.Dispatch(new GetResourceHeaderQuery(identityService.Identity)));
}
