using System.Web.Mvc;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [RoutePrefix("Home")]
    public sealed class HomeController : BaseController {
        public HomeController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
        }

        [Route("~/")]
        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [Route("Register")]
        [HttpGet]
        public ActionResult Register() {
            return View(new RegisterUserModel());
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult Register(RegisterUserModel model) {
            return ValidatedCommandResult(model, new RegisterUserCommand(model.Email, model.Password), () => {
                _messageService.Dispatch(new RegisterPlayerCommand(model.Email, model.DisplayName));
                return View("Registered");
            });
        }

        [Route("Activate")]
        [HttpGet]
        public ActionResult Activate(string activationCode, string email) {
            var result = _messageService.Dispatch(new ActivateUserCommand(email, activationCode));

            if (result.Success) {
                return View("Activated");
            }
            else {
                return View("ActivationFailed");
            }
        }

        [Route("SendActivation")]
        [HttpGet]
        public ActionResult SendActivation() {
            return View(new SendUserActivationModel());
        }

        [Route("SendActivation")]
        [HttpPost]
        public ActionResult SendActivation(SendUserActivationModel model) {
            return ValidatedCommandResult(model, new SendUserActivationCommand(model.Email), "ActivationSent");
        }

        [Route("LogIn")]
        [HttpGet]
        public ActionResult LogIn() {
            return View(new LogInUserModel());
        }

        [Route("LogIn")]
        [HttpPost]
        public ActionResult LogIn(LogInUserModel model) {
            return ValidatedCommandResult(model,
                new LogInUserCommand(model.Email, model.Password),
                () => {
                    _authenticationService.SignIn(model.Email);
                    return RedirectToAction("Index");
                });
        }

        [Route("LogOut")]
        [HttpPost]
        [Authorize]
        public ActionResult LogOut() {
            _messageService.Dispatch(new LogOutUserCommand(_authenticationService.Identity));
            _authenticationService.SignOut();
            return RedirectToAction("Index");
        }

        [Route("ChangePassword")]
        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword() {
            return View(new ChangeUserPasswordModel());
        }

        [Route("ChangePassword")]
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangeUserPasswordModel model) {
            return ValidatedCommandResult(model, new ChangeUserPasswordCommand(_authenticationService.Identity, model.CurrentPassword, model.NewPassword), "PasswordChanged");
        }

        [Route("ForgotPassword")]
        [HttpGet]
        public ActionResult ForgotPassword() {
            return View(new ForgotUserPasswordModel());
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotUserPasswordModel model) {
            return ValidatedCommandResult(model, new ForgotUserPasswordCommand(model.Email), "PasswordResetSent");
        }

        [Route("ResetPassword")]
        [HttpGet]
        public ActionResult ResetPassword() {
            return View(new ResetUserPasswordModel());
        }

        [Route("ResetPassword")]
        [HttpPost]
        public ActionResult ResetPassword(string email, string token, ResetUserPasswordModel model) {
            return ValidatedCommandResult(model, new ResetUserPasswordCommand(email, token, model.NewPassword), "PasswordReset");
        }

        [Route("Deactivate")]
        [HttpGet]
        [Authorize]
        public ActionResult Deactivate() {
            return View(new DeactivateUserModel());
        }

        [Route("Deactivate")]
        [HttpPost]
        [Authorize]
        public ActionResult Deactivate(DeactivateUserModel model) {
            return ValidatedCommandResult(model,
                new DeactivateUserCommand(_authenticationService.Identity, model.Password),
                () => {
                    _authenticationService.SignOut();
                    return RedirectToAction("Index");
                });
        }

        [Route("ChangeEmail")]
        [HttpGet]
        [Authorize]
        public ActionResult ChangeEmail() {
            return View(new ChangeUserEmailModel());
        }

        [Route("ChangeEmail")]
        [HttpPost]
        [Authorize]
        public ActionResult ChangeEmail(ChangeUserEmailModel model) {
            return ValidatedCommandResult(model, new ChangeUserEmailCommand(_authenticationService.Identity, model.Password, model.NewEmail), "EmailChangeRequested");
        }

        [Route("ConfirmEmail")]
        [HttpGet]
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

        [Route("EmailChangeConfirmed")]
        [HttpGet]
        [Authorize]
        public ActionResult EmailChangeConfirmed() {
            return View();
        }
    }
}