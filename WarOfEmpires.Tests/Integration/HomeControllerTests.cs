using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WarOfEmpires.Controllers;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Services;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.DependencyInjection;
using WarOfEmpires.Utilities.Mail;

namespace WarOfEmpires.Tests.Integration {
    [TestClass]
    public sealed class HomeControllerTests {
        private readonly IServiceProvider serviceProvider;
        private readonly FakeAuthenticationService _authenticationService = new FakeAuthenticationService();
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly FakeMailClient _mailClient = new FakeMailClient();

        public HomeControllerTests() {
            var services = new ServiceCollection();

            services.AddServices(typeof(HomeController).Assembly);
            services.Replace(ServiceDescriptor.Scoped<IAuthenticationService>(serviceProvider => _authenticationService));
            services.Replace(ServiceDescriptor.Transient<IWarContext>(serviceProvider => _context));
            services.Replace(ServiceDescriptor.Scoped<IMailClient>(serviceProvider => _mailClient));
            services.AddSingleton<AppSettings>();
            services.AddTransient<HomeController>();

            serviceProvider = services.BuildServiceProvider();
        }

        private HomeController GetController() {
            var controller = serviceProvider.GetRequiredService<HomeController>();

            controller.Url = Substitute.For<IUrlHelper>();
            controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("Test");

            return controller;
        }

        [TestMethod]
        public async Task HomeController_Registration_Activation_To_LogIn_Succeeds() {
            // Registration
            var registrationResult = GetController().Register(new RegisterUserModel() {
                Email = "test@test.com",
                Password = "test",
                ConfirmPassword = "test",
                DisplayName = "Test display"
            });

            registrationResult.Should().BeOfType<JsonResult>();
            _mailClient.SentMessages.Should().NotBeEmpty();

            // Activation
            var activationCode = Regex.Match(_mailClient.SentMessages.Last().Body, "\\d+").Value;
            var activationResult = GetController().Activate(activationCode, "test@test.com");

            activationResult.Should().BeOfType<RedirectResult>();

            // Log in
            var logInResult = await GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().Be("test@test.com");
        }

        [TestMethod]
        public async Task HomeController_RequestPasswordReset_ResetPassword_To_LogIn_Succeeds() {
            // Set up
            var user = new User("test@test.com", "old");
            user.Activate();
            _context.Users.Add(user);

            // Request password reset
            var forgotPasswordResult = GetController().ForgotPassword(new ForgotUserPasswordModel() {
                Email = "test@test.com"
            });

            forgotPasswordResult.Should().BeOfType<RedirectResult>();
            _mailClient.SentMessages.Should().NotBeEmpty();

            // Reset password
            var token = Regex.Match(_mailClient.SentMessages.Last().Body, "(?<=\\=)\\w+(?=\")").Value;
            var resetResult = GetController().ResetPassword("test@test.com", token, new ResetUserPasswordModel() {
                NewPassword = "test",
                ConfirmNewPassword = "test"
            });

            resetResult.Should().BeOfType<RedirectResult>();

            // Log in
            var logInResult = await GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().Be("test@test.com");
        }

        [TestMethod]
        public async Task HomeController_ChangePassword_LogOut_To_LogIn_Succeeds() {
            // Set up
            var user = new User("test@test.com", "test");
            user.Activate();
            _context.Users.Add(user);
            _authenticationService.Identity = "test@test.com";

            // Change password
            var changePasswordResult = GetController().ChangePassword(new ChangeUserPasswordModel() {
                CurrentPassword = "test",
                NewPassword = "hunter2",
                ConfirmNewPassword = "hunter2"
            });
            changePasswordResult.Should().BeOfType<RedirectResult>();

            // Log out
            var logOutResult = await GetController().LogOut();

            logOutResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().BeNull();

            // Log in
            var logInResult = await GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "hunter2"
            });

            logInResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().Be("test@test.com");
        }

        [TestMethod]
        public async Task HomeController_FailedLogIn_To_LogIn_To_Deactivate_Succeeds() {
            // Set up
            var user = new User("test@test.com", "test");
            user.Activate();
            _context.Users.Add(user);
            
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.User)).SetValue(player, user);
            _context.Players.Add(player);

            // Failed log in
            var failedLogInResult = await GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "hunter2"
            });

            failedLogInResult.Should().BeOfType<ViewResult>();
            _authenticationService.Identity.Should().BeNull();

            // Log in
            var logInResult = await GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().Be("test@test.com");

            // Deactivate
            var deactivateResult = GetController().Deactivate(new DeactivateUserModel() {
                Password = "test"
            });

            deactivateResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().BeNull();
            _context.Users.Single().Status.Should().Be(UserStatus.Inactive);
        }

        [TestMethod]
        public async Task HomeController_ChangeEmail_LogOut_ConfirmEmail_To_LogIn_Succeeds() {
            // Set up
            var user = new User("test@test.com", "test");
            user.Activate();
            _context.Users.Add(user);
            _authenticationService.Identity = "test@test.com";

            // Change email
            var changeEmailResult = GetController().ChangeEmail(new ChangeUserEmailModel() {
                Password = "test",
                NewEmail = "new@test.com",
                ConfirmNewEmail = "new@test.com"
            });
            changeEmailResult.Should().BeOfType<RedirectResult>();

            // Log out
            var logOutResult = await GetController().LogOut();

            logOutResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().BeNull();

            // Confirm email change
            var confirmationCode = Regex.Match(_mailClient.SentMessages.Last().Body, "\\d+").Value;
            var confirmationResult = GetController().ConfirmEmail(confirmationCode, "test@test.com");

            confirmationResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().BeNull();

            // Log in with new email address
            var logInResult = await GetController().LogIn(new LogInUserModel() {
                Email = "new@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().Be("new@test.com");
        }

        [TestMethod]
        public void HomeController_ChangeEmail_To_ConfirmEmail_Succeeds() {
            // Set up
            var user = new User("test@test.com", "test");
            user.Activate();
            _context.Users.Add(user);
            _authenticationService.Identity = "test@test.com";

            // Change email
            var changeEmailResult = GetController().ChangeEmail(new ChangeUserEmailModel() {
                Password = "test",
                NewEmail = "new@test.com",
                ConfirmNewEmail = "new@test.com"
            });
            changeEmailResult.Should().BeOfType<RedirectResult>();

            // Confirm email change
            var confirmationCode = Regex.Match(_mailClient.SentMessages.Last().Body, "\\d+").Value;
            var confirmationResult = GetController().ConfirmEmail(confirmationCode, "test@test.com");

            confirmationResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().Be("new@test.com");
        }
    }
}