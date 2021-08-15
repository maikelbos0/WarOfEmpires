using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Extensions;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Route(Route)]
    public sealed class HomeController : BaseController {
        public const string Route = "Home";

        public HomeController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [UserOnline]
        [HttpGet("~/", Order = -2)]
        [HttpGet("", Order = -1)]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            // Explicitly name view so it works from other actions
            return View(nameof(Index));
        }

        [HttpGet(nameof(Register))]
        public ViewResult Register() {
            return View(new RegisterUserModel());
        }

        [HttpPost(nameof(Register))]
        public ViewResult Register(RegisterUserModel model) {
            return BuildViewResultFor(new RegisterUserCommand(model.Email, model.Password))
                .OnSuccess(() => {
                    _messageService.Dispatch(new RegisterPlayerCommand(model.Email, model.DisplayName));
                    return Index();
                })
                .OnFailure(nameof(Register), model);
        }

        [HttpGet(nameof(Activate))]
        public ViewResult Activate(string activationCode, string email) {
            return BuildViewResultFor(new ActivateUserCommand(email, activationCode))
                .OnSuccess(nameof(Activated))
                .OnFailure(nameof(ActivationFailed));
        }

        [HttpGet(nameof(Activated))]
        public ViewResult Activated() {
            return View();
        }

        [HttpGet(nameof(ActivationFailed))]
        public ViewResult ActivationFailed() {
            return View();
        }

        [HttpGet(nameof(SendActivation))]
        public ViewResult SendActivation() {
            return View(new SendUserActivationModel());
        }

        [HttpPost(nameof(SendActivation))]
        public ViewResult SendActivation(SendUserActivationModel model) {
            return BuildViewResultFor(new SendUserActivationCommand(model.Email))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        [HttpGet(nameof(LogIn))]
        public ViewResult LogIn(string returnUrl = null) {
            // Explicitly name view so it works from other actions
            return View(nameof(LogIn), new LogInUserModel() {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost(nameof(LogIn))]
        public async Task<ActionResult> LogIn(LogInUserModel model) {
            var result = _messageService.Dispatch(new LogInUserCommand(model.Email, model.Password));

            if (result.Success) {
                await _authenticationService.SignIn(model.Email);

                if (!string.IsNullOrEmpty(model.ReturnUrl)) {
                    return Redirect(model.ReturnUrl);
                }
                else {
                    return RedirectToAction(nameof(PlayerController.Home), PlayerController.Route);
                }
            }
            else {
                ModelState.Merge(result);
                return View(model);
            }
        }

        [UserOnline]
        [HttpPost(nameof(LogOut))]
        [Authorize]
        public async Task<RedirectToActionResult> LogOut() {
            _messageService.Dispatch(new LogOutUserCommand(_authenticationService.Identity));
            await _authenticationService.SignOut();
            return RedirectToAction(nameof(Index));
        }

        [UserOnline]
        [HttpGet(nameof(ChangePassword))]
        [Authorize]
        public ViewResult ChangePassword() {
            return View(new ChangeUserPasswordModel());
        }

        [UserOnline]
        [HttpPost(nameof(ChangePassword))]
        [Authorize]
        public ViewResult ChangePassword(ChangeUserPasswordModel model) {
            return BuildViewResultFor(new ChangeUserPasswordCommand(_authenticationService.Identity, model.CurrentPassword, model.NewPassword))
                .OnSuccess(Index)
                .OnFailure(nameof(ChangePassword), model);
        }

        [HttpGet(nameof(ForgotPassword))]
        public ViewResult ForgotPassword() {
            return View(new ForgotUserPasswordModel());
        }

        [HttpPost(nameof(ForgotPassword))]
        public ViewResult ForgotPassword(ForgotUserPasswordModel model) {
            return BuildViewResultFor(new ForgotUserPasswordCommand(model.Email))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        [HttpGet(nameof(ResetPassword))]
        public ViewResult ResetPassword() {
            return View(new ResetUserPasswordModel());
        }

        [HttpPost(nameof(ResetPassword))]
        public ViewResult ResetPassword(string email, string token, ResetUserPasswordModel model) {
            return BuildViewResultFor(new ResetUserPasswordCommand(email, token, model.NewPassword))
                .OnSuccess(() => LogIn())
                .OnFailure(nameof(ResetPassword), model);
        }

        [UserOnline]
        [HttpGet(nameof(Deactivate))]
        [Authorize]
        public ViewResult Deactivate() {
            return View(new DeactivateUserModel());
        }

        [UserOnline]
        [HttpPost(nameof(Deactivate))]
        [Authorize]
        public ActionResult Deactivate(DeactivateUserModel model) {
            var result = _messageService.Dispatch(new DeactivateUserCommand(_authenticationService.Identity, model.Password));

            if (result.Success) {
                _authenticationService.SignOut();
                return RedirectToAction(nameof(Index));
            }
            else {
                ModelState.Merge(result);
                return View(model);
            }
        }

        [UserOnline]
        [HttpGet(nameof(ChangeEmail))]
        [Authorize]
        public ViewResult ChangeEmail() {
            return View(new ChangeUserEmailModel());
        }

        [UserOnline]
        [HttpPost(nameof(ChangeEmail))]
        [Authorize]
        public ViewResult ChangeEmail(ChangeUserEmailModel model) {
            return BuildViewResultFor(new ChangeUserEmailCommand(_authenticationService.Identity, model.Password, model.NewEmail))
                .OnSuccess(Index)
                .OnFailure(nameof(ChangeEmail), model);
        }

        [HttpGet(nameof(ConfirmEmail))]
        public ActionResult ConfirmEmail(string confirmationCode, string email) {
            var newEmail = _messageService.Dispatch(new GetUserNewEmailQuery(email));
            var result = _messageService.Dispatch(new ConfirmUserEmailChangeCommand(email, confirmationCode));

            if (result.Success) {
                if (_authenticationService.IsAuthenticated) {
                    _authenticationService.SignIn(newEmail);
                }

                return RedirectToAction(nameof(EmailChangeConfirmed));
            }
            else {
                return RedirectToAction(nameof(EmailChangeFailed));
            }
        }

        [HttpGet(nameof(EmailChangeFailed))]
        public ViewResult EmailChangeFailed() {
            return View();
        }

        [HttpGet(nameof(EmailChangeConfirmed))]
        public ViewResult EmailChangeConfirmed() {
            return View();
        }
    }
}