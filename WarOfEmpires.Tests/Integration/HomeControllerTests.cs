using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using WarOfEmpires.Controllers;
using WarOfEmpires.Database;
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
        private readonly FakeAppSettings _appSettings = new FakeAppSettings();

        public HomeControllerTests() {
            var services = new ServiceCollection();

            services.AddServices(typeof(HomeController).Assembly);
            services.Replace(ServiceDescriptor.Scoped<IAuthenticationService>(serviceProvider => _authenticationService));
            services.Replace(ServiceDescriptor.Scoped<IWarContext>(serviceProvider => _context));
            services.Replace(ServiceDescriptor.Scoped<IMailClient>(serviceProvider => _mailClient));
            services.Replace(ServiceDescriptor.Scoped<IAppSettings>(serviceProvider => _appSettings));
            services.AddScoped<HomeController>();

            serviceProvider = services.BuildServiceProvider();
        }

        private HomeController GetController() {
            return serviceProvider.GetRequiredService<HomeController>();
        }

        [TestMethod]
        public void HomeController_Registration_Activation_To_LogIn_Succeeds() {
            // Registration
            var registrationResult = GetController().Register(new RegisterUserModel() {
                Email = "test@test.com",
                Password = "test",
                ConfirmPassword = "test",
                DisplayName = "Test display"
            });

            registrationResult.ViewName.Should().Be("Index");
            _mailClient.SentMessages.Should().NotBeEmpty();

            // Activation
            var activationCode = Regex.Match(_mailClient.SentMessages.Last().Body, "\\d+").Value;
            var activationResult = GetController().Activate(activationCode, "test@test.com");

            activationResult.ViewName.Should().Be("Activated");

            // Log in
            var logInResult = GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectResult>();
            _authenticationService.Identity.Should().Be("test@test.com");
        }

        [TestMethod]
        public void HomeController_RequestPasswordReset_ResetPassword_To_LogIn_Succeeds() {
            // Set up
            var user = new User("test@test.com", "old");
            user.Activate();
            _context.Users.Add(user);

            // Request password reset
            var forgotPasswordResult = GetController().ForgotPassword(new ForgotUserPasswordModel() {
                Email = "test@test.com"
            });

            forgotPasswordResult.ViewName.Should().Be("Index");
            _mailClient.SentMessages.Should().NotBeEmpty();

            // Reset password
            var token = Regex.Match(_mailClient.SentMessages.Last().Body, "(?<=\\=)\\w+(?=\")").Value;
            var resetResult = GetController().ResetPassword("test@test.com", token, new ResetUserPasswordModel() {
                NewPassword = "test",
                ConfirmNewPassword = "test"
            });

            resetResult.ViewName.Should().Be("PasswordReset");

            // Log in
            var logInResult = GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectResult>();
            _authenticationService.Identity.Should().Be("test@test.com");
        }

        [TestMethod]
        public void HomeController_ChangePassword_LogOut_To_LogIn_Succeeds() {
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
            changePasswordResult.ViewName.Should().Be("Index");

            // Log out
            var logOutResult = GetController().LogOut();

            logOutResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().BeNull();

            // Log in
            var logInResult = GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "hunter2"
            });

            logInResult.Should().BeOfType<RedirectResult>();
            _authenticationService.Identity.Should().Be("test@test.com");
        }

        [TestMethod]
        public void HomeController_FailedLogIn_To_LogIn_To_Deactivate_Succeeds() {
            // Set up
            var user = new User("test@test.com", "test");
            user.Activate();
            _context.Users.Add(user);

            // Failed log in
            var failedLogInResult = GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "hunter2"
            });

            failedLogInResult.Should().BeOfType<ViewResult>();
            _authenticationService.Identity.Should().BeNull();

            // Log in
            var logInResult = GetController().LogIn(new LogInUserModel() {
                Email = "test@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectResult>();
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
        public void HomeController_ChangeEmail_LogOut_ConfirmEmail_To_LogIn_Succeeds() {
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
            changeEmailResult.ViewName.Should().Be("Index");

            // Log out
            var logOutResult = GetController().LogOut();

            logOutResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().BeNull();

            // Confirm email change
            var confirmationCode = Regex.Match(_mailClient.SentMessages.Last().Body, "\\d+").Value;
            var confirmationResult = GetController().ConfirmEmail(confirmationCode, "test@test.com");

            confirmationResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().BeNull();

            // Log in with new email address
            var logInResult = GetController().LogIn(new LogInUserModel() {
                Email = "new@test.com",
                Password = "test"
            });

            logInResult.Should().BeOfType<RedirectResult>();
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
            changeEmailResult.ViewName.Should().Be("Index");

            // Confirm email change
            var confirmationCode = Regex.Match(_mailClient.SentMessages.Last().Body, "\\d+").Value;
            var confirmationResult = GetController().ConfirmEmail(confirmationCode, "test@test.com");

            confirmationResult.Should().BeOfType<RedirectToActionResult>();
            _authenticationService.Identity.Should().Be("new@test.com");
        }
    }
}