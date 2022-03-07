using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Extensions;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Route(Route)]
    public sealed class HomeController : BaseController {
        public const string Route = "Home";

        public HomeController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("~/", Order = -2)]
        [HttpGet("", Order = -1)]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            return View();
        }

        [HttpGet(nameof(Register))]
        public ViewResult Register() {
            return View(new RegisterUserModel());
        }

        [HttpPost(nameof(Register))]
        public ActionResult Register(RegisterUserModel model) {
            return BuildViewResultFor(new RegisterUserCommand(model.Email, model.Password))
                .OnSuccess(nameof(Index))
                .OnFailure(model);
        }

        [HttpGet(nameof(Activate))]
        public ActionResult Activate(string activationCode, string email) {
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
        public ActionResult SendActivation(SendUserActivationModel model) {
            return BuildViewResultFor(new SendUserActivationCommand(model.Email))
                .OnSuccess(nameof(Index))
                .ThrowOnFailure();
        }

        [HttpGet(nameof(LogIn))]
        public ViewResult LogIn(string returnUrl = null) {
            return View(new LogInUserModel() {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost(nameof(LogIn))]
        public async Task<ActionResult> LogIn(LogInUserModel model) {
            if (ModelState.IsValid) {
                var result = _messageService.Dispatch(new LogInUserCommand(model.Email, model.Password));
                ModelState.Merge(result);
            }

            if (ModelState.IsValid) {
                await _authenticationService.SignIn(model.Email);

                if (!_messageService.Dispatch(new GetPlayerIsCreatedQuery(model.Email))) {
                    return RedirectToAction(nameof(PlayerController.Create), PlayerController.Route);
                }
                else if (!string.IsNullOrEmpty(model.ReturnUrl)) {
                    return Redirect(model.ReturnUrl);
                }
                else {
                    return RedirectToAction(nameof(PlayerController.Home), PlayerController.Route);
                }
            }
            else {
                return View(model);
            }
        }

        [HttpPost(nameof(LogOut))]
        [Authorize]
        public async Task<RedirectToActionResult> LogOut() {
            _messageService.Dispatch(new LogOutUserCommand(_authenticationService.Identity));
            await _authenticationService.SignOut();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet(nameof(ChangePassword))]
        [Authorize]
        public ViewResult ChangePassword() {
            return View(new ChangeUserPasswordModel());
        }

        [HttpPost(nameof(ChangePassword))]
        [Authorize]
        public ActionResult ChangePassword(ChangeUserPasswordModel model) {
            return BuildViewResultFor(new ChangeUserPasswordCommand(_authenticationService.Identity, model.CurrentPassword, model.NewPassword))
                .OnSuccess(nameof(Index))
                .OnFailure(model);
        }

        [HttpGet(nameof(ForgotPassword))]
        public ViewResult ForgotPassword() {
            return View(new ForgotUserPasswordModel());
        }

        [HttpPost(nameof(ForgotPassword))]
        public ActionResult ForgotPassword(ForgotUserPasswordModel model) {
            return BuildViewResultFor(new ForgotUserPasswordCommand(model.Email))
                .OnSuccess(nameof(Index))
                .ThrowOnFailure();
        }

        [HttpGet(nameof(ResetPassword))]
        public ViewResult ResetPassword() {
            return View(new ResetUserPasswordModel());
        }

        [HttpPost(nameof(ResetPassword))]
        public ActionResult ResetPassword(string email, string token, ResetUserPasswordModel model) {
            return BuildViewResultFor(new ResetUserPasswordCommand(email, token, model.NewPassword))
                .OnSuccess(nameof(LogIn))
                .OnFailure(model);
        }

        [HttpGet(nameof(Deactivate))]
        [Authorize]
        public ViewResult Deactivate() {
            return View(new DeactivateUserModel());
        }

        [HttpPost(nameof(Deactivate))]
        [Authorize]
        public ActionResult Deactivate(DeactivateUserModel model) {
            if (ModelState.IsValid) {
                var result = _messageService.Dispatch(new DeactivateUserCommand(_authenticationService.Identity, model.Password));
                ModelState.Merge(result);
            }

            if (ModelState.IsValid) {
                _authenticationService.SignOut();
                return RedirectToAction(nameof(Index));
            }
            else {
                return View(model);
            }
        }

        [HttpGet(nameof(ChangeEmail))]
        [Authorize]
        public ViewResult ChangeEmail() {
            return View(new ChangeUserEmailModel());
        }

        [HttpPost(nameof(ChangeEmail))]
        [Authorize]
        public ActionResult ChangeEmail(ChangeUserEmailModel model) {
            return BuildViewResultFor(new ChangeUserEmailCommand(_authenticationService.Identity, model.Password, model.NewEmail))
                .OnSuccess(nameof(Index))
                .OnFailure(model);
        }

        [HttpGet(nameof(ConfirmEmail))]
        public ActionResult ConfirmEmail(string confirmationCode, string email) {
            var newEmail = _messageService.Dispatch(new GetUserNewEmailQuery(email));

            if (ModelState.IsValid) {
                var result = _messageService.Dispatch(new ConfirmUserEmailChangeCommand(email, confirmationCode));
                ModelState.Merge(result);
            }

            if (ModelState.IsValid) {
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

        [Route(nameof(Error))]
        [Route(nameof(Error) + "/{statusCode:int}")]
        public ActionResult Error(int statusCode = 500) {
            if (Request.IsAjaxRequest()) {
                return new StatusCodeResult(statusCode);
            }

            return View(statusCode);
        }
    }
}
