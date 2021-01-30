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
                .OnFailure("Register", model);
        }

        [Route("Activate")]
        [HttpGet]
        public ViewResult Activate(string activationCode, string email) {
            return BuildViewResultFor(new ActivateUserCommand(email, activationCode))
                .OnSuccess("Activated")
                .OnFailure("ActivationFailed");
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
                .ThrowOnFailure();
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
                .OnFailure("ChangePassword", model);
        }

        [Route("ForgotPassword")]
        [HttpGet]
        public ViewResult ForgotPassword() {
            return View(new ForgotUserPasswordModel());
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public ViewResult ForgotPassword(ForgotUserPasswordModel model) {
            return BuildViewResultFor(new ForgotUserPasswordCommand(model.Email))
            .OnSuccess(Index)
            .ThrowOnFailure();
        }

        [Route("ResetPassword")]
        [HttpGet]
        public ViewResult ResetPassword() {
            return View(new ResetUserPasswordModel());
        }

        [Route("ResetPassword")]
        [HttpPost]
        public ViewResult ResetPassword(string email, string token, ResetUserPasswordModel model) {
            return  BuildViewResultFor(new ResetUserPasswordCommand(email, token, model.NewPassword))
                .OnSuccess("PasswordReset")
                .OnFailure("ResetPassword", model);
        }

        [UserOnline]
        [Route("Deactivate")]
        [HttpGet]
        [Authorize]
        public ViewResult Deactivate() {
            return View(new DeactivateUserModel());
        }

        [UserOnline]
        [Route("Deactivate")]
        [HttpPost]
        [Authorize]
        public ActionResult Deactivate(DeactivateUserModel model) {
            var result = _messageService.Dispatch(new DeactivateUserCommand(_authenticationService.Identity, model.Password));

            if (result.Success) {
                _authenticationService.SignOut();
                return RedirectToAction("Index");
            }
            else {
                return View(model);
            }
        }

        [UserOnline]
        [Route("ChangeEmail")]
        [HttpGet]
        [Authorize]
        public ViewResult ChangeEmail() {
            return View(new ChangeUserEmailModel());
        }

        [UserOnline]
        [Route("ChangeEmail")]
        [HttpPost]
        [Authorize]
        public ViewResult ChangeEmail(ChangeUserEmailModel model) {
            return BuildViewResultFor(new ChangeUserEmailCommand(_authenticationService.Identity, model.Password, model.NewEmail))
                .OnSuccess(Index)
                .OnFailure("ChangeEmail", model);
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
        public ViewResult EmailChangeConfirmed() {
            return View();
        }
    }
}