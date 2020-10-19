using System.Web.Mvc;
using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Extensions;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [RoutePrefix("Home")]
    public sealed class HomeController : BaseController {
        public HomeController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [UserOnline]
        [Route("~/")]
        [Route]
        [Route("Index")]
        [HttpGet]
        public ViewResult Index() {
            // Explicitly name view so it works from other actions
            return View("Index");
        }

        [Route("Register")]
        [HttpGet]
        public ViewResult Register() {
            return View(new RegisterUserModel());
        }

        [Route("Register")]
        [HttpPost]
        public ViewResult Register(RegisterUserModel model) {
            return BuildViewResultFor(new RegisterUserCommand(model.Email, model.Password))
                .OnSuccess(() => {
                    _messageService.Dispatch(new RegisterPlayerCommand(model.Email, model.DisplayName));
                    return Index();
                })
                .OnFailure("Register", model)
                .Execute();
        }

        [Route("Activate")]
        [HttpGet]
        public ViewResult Activate(string activationCode, string email) {
            return BuildViewResultFor(new ActivateUserCommand(email, activationCode))
                .OnSuccess("Activated")
                .OnFailure("ActivationFailed")
                .Execute();
        }

        [Route("SendActivation")]
        [HttpGet]
        public ViewResult SendActivation() {
            return View(new SendUserActivationModel());
        }

        [Route("SendActivation")]
        [HttpPost]
        public ViewResult SendActivation(SendUserActivationModel model) {
            return BuildViewResultFor(new SendUserActivationCommand(model.Email))
                .OnSuccess(Index)
                .ThrowOnFailure()
                .Execute();
        }

        [Route("LogIn")]
        [HttpGet]
        public ActionResult LogIn() {
            if (Request.IsAjaxRequest()) {
                Response.AddHeader("X-Unauthenticated", "true");
                return new EmptyResult();
            }
            else {
                return View(new LogInUserModel() {
                    ReturnUrl = Request.QueryString["ReturnUrl"]
                });
            }
        }

        [Route("LogIn")]
        [HttpPost]
        public ActionResult LogIn(LogInUserModel model) {
            var result = _messageService.Dispatch(new LogInUserCommand(model.Email, model.Password));

            if (result.Success) {
                _authenticationService.SignIn(model.Email);
                return Redirect(model.ReturnUrl ?? "Index");
            }
            else {
                ModelState.Merge(result);
                return View(model);
            }
        }

        [UserOnline]
        [Route("LogOut")]
        [HttpPost]
        [Authorize]
        public RedirectToRouteResult LogOut() {
            _messageService.Dispatch(new LogOutUserCommand(_authenticationService.Identity));
            _authenticationService.SignOut();
            return RedirectToAction("Index");
        }

        [UserOnline]
        [Route("ChangePassword")]
        [HttpGet]
        [Authorize]
        public ViewResult ChangePassword() {
            return View(new ChangeUserPasswordModel());
        }

        [UserOnline]
        [Route("ChangePassword")]
        [HttpPost]
        [Authorize]
        public ViewResult ChangePassword(ChangeUserPasswordModel model) {
            return BuildViewResultFor(new ChangeUserPasswordCommand(_authenticationService.Identity, model.CurrentPassword, model.NewPassword))
                .OnSuccess(Index)
                .OnFailure("ChangePassword", model)
                .Execute();
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

        [UserOnline]
        [Route("Deactivate")]
        [HttpGet]
        [Authorize]
        public ActionResult Deactivate() {
            return View(new DeactivateUserModel());
        }

        [UserOnline]
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

        [UserOnline]
        [Route("ChangeEmail")]
        [HttpGet]
        [Authorize]
        public ActionResult ChangeEmail() {
            return View(new ChangeUserEmailModel());
        }

        [UserOnline]
        [Route("ChangeEmail")]
        [HttpPost]
        [Authorize]
        public ActionResult ChangeEmail(ChangeUserEmailModel model) {
            return ValidatedCommandResult(model, new ChangeUserEmailCommand(_authenticationService.Identity, model.Password, model.NewEmail), "EmailChangeRequested");
        }

        [UserOnline]
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

        [UserOnline]
        [Route("EmailChangeConfirmed")]
        [HttpGet]
        [Authorize]
        public ActionResult EmailChangeConfirmed() {
            return View();
        }
    }
}