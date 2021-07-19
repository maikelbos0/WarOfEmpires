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
    [Route("")]
    [Route("Home")]
    public sealed class HomeController : BaseController {
        public HomeController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [UserOnline]
        [HttpGet]
        [HttpGet("Index")]
        public ViewResult Index() {
            // Explicitly name view so it works from other actions
            return View("Index");
        }

        [HttpGet("Register")]
        public ViewResult Register() {
            return View(new RegisterUserModel());
        }

        [HttpPost("Register")]
        public ViewResult Register(RegisterUserModel model) {
            return BuildViewResultFor(new RegisterUserCommand(model.Email, model.Password))
                .OnSuccess(() => {
                    _messageService.Dispatch(new RegisterPlayerCommand(model.Email, model.DisplayName));
                    return Index();
                })
                .OnFailure("Register", model);
        }

        [HttpGet("Activate")]
        public ViewResult Activate(string activationCode, string email) {
            return BuildViewResultFor(new ActivateUserCommand(email, activationCode))
                .OnSuccess("Activated")
                .OnFailure("ActivationFailed");
        }

        [HttpGet("SendActivation")]
        public ViewResult SendActivation() {
            return View(new SendUserActivationModel());
        }

        [HttpPost("SendActivation")]
        public ViewResult SendActivation(SendUserActivationModel model) {
            return BuildViewResultFor(new SendUserActivationCommand(model.Email))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        [HttpGet("LogIn")]
        public ActionResult LogIn(string returnUrl = null) {
            return View(new LogInUserModel() {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult> LogIn(LogInUserModel model) {
            var result = _messageService.Dispatch(new LogInUserCommand(model.Email, model.Password));

            if (result.Success) {
                await _authenticationService.SignIn(model.Email);
                return Redirect(model.ReturnUrl ?? "Index");
            }
            else {
                ModelState.Merge(result);
                return View(model);
            }
        }

        [UserOnline]
        [HttpPost("LogOut")]
        [Authorize]
        public async Task<RedirectToActionResult> LogOut() {
            _messageService.Dispatch(new LogOutUserCommand(_authenticationService.Identity));
            await _authenticationService.SignOut();
            return RedirectToAction("Index");
        }

        [UserOnline]
        [HttpGet("ChangePassword")]
        [Authorize]
        public ViewResult ChangePassword() {
            return View(new ChangeUserPasswordModel());
        }

        [UserOnline]
        [HttpPost("ChangePassword")]
        [Authorize]
        public ViewResult ChangePassword(ChangeUserPasswordModel model) {
            return BuildViewResultFor(new ChangeUserPasswordCommand(_authenticationService.Identity, model.CurrentPassword, model.NewPassword))
                .OnSuccess(Index)
                .OnFailure("ChangePassword", model);
        }

        [HttpGet("ForgotPassword")]
        public ViewResult ForgotPassword() {
            return View(new ForgotUserPasswordModel());
        }

        [HttpPost("ForgotPassword")]
        public ViewResult ForgotPassword(ForgotUserPasswordModel model) {
            return BuildViewResultFor(new ForgotUserPasswordCommand(model.Email))
                .OnSuccess(Index)
                .ThrowOnFailure();
        }

        [HttpGet("ResetPassword")]
        public ViewResult ResetPassword() {
            return View(new ResetUserPasswordModel());
        }

        [HttpPost("ResetPassword")]
        public ViewResult ResetPassword(string email, string token, ResetUserPasswordModel model) {
            return BuildViewResultFor(new ResetUserPasswordCommand(email, token, model.NewPassword))
                .OnSuccess("PasswordReset")
                .OnFailure("ResetPassword", model);
        }

        [UserOnline]
        [HttpGet("Deactivate")]
        [Authorize]
        public ViewResult Deactivate() {
            return View(new DeactivateUserModel());
        }

        [UserOnline]
        [HttpPost("Deactivate")]
        [Authorize]
        public ActionResult Deactivate(DeactivateUserModel model) {
            var result = _messageService.Dispatch(new DeactivateUserCommand(_authenticationService.Identity, model.Password));

            if (result.Success) {
                _authenticationService.SignOut();
                return RedirectToAction("Index");
            }
            else {
                ModelState.Merge(result);
                return View(model);
            }
        }

        [UserOnline]
        [HttpGet("ChangeEmail")]
        [Authorize]
        public ViewResult ChangeEmail() {
            return View(new ChangeUserEmailModel());
        }

        [UserOnline]
        [HttpPost("ChangeEmail")]
        [Authorize]
        public ViewResult ChangeEmail(ChangeUserEmailModel model) {
            return BuildViewResultFor(new ChangeUserEmailCommand(_authenticationService.Identity, model.Password, model.NewEmail))
                .OnSuccess(Index)
                .OnFailure("ChangeEmail", model);
        }

        [HttpGet("ConfirmEmail")]
        public ActionResult ConfirmEmail(string confirmationCode, string email) {
            var newEmail = _messageService.Dispatch(new GetUserNewEmailQuery(email));
            var result = _messageService.Dispatch(new ConfirmUserEmailChangeCommand(email, confirmationCode));

            if (result.Success) {
                if (_authenticationService.IsAuthenticated) {
                    _authenticationService.SignIn(newEmail);
                }

                return RedirectToAction("EmailChangeConfirmed");
            }
            else {
                return View("EmailChangeFailed");
            }
        }

        [HttpGet("EmailChangeConfirmed")]
        [Authorize]
        public ViewResult EmailChangeConfirmed() {
            return View();
        }
    }
}