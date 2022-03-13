using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Middleware;
using WarOfEmpires.Services;

namespace WarOfEmpires.Tests.Middleware {
    [TestClass]
    public class UserOnlineMiddlewareTests {
        private readonly RequestDelegate next;
        private readonly IAuthenticationService authenticationService;
        private readonly IMessageService messageService;
        private readonly UserOnlineMiddleware middleware;

        public UserOnlineMiddlewareTests() {
            middleware = new UserOnlineMiddleware(
                next = Substitute.For<RequestDelegate>(), 
                authenticationService = Substitute.For<IAuthenticationService>(),
                messageService = Substitute.For<IMessageService>()
            );
        }

        [TestMethod]
        public async Task UserOnlineMiddleware_InvokeAsync_Dispatches_UserOnlineCommand_For_Get_Request() {
            var context = GetHttpContext("GET", false);

            authenticationService.IsAuthenticated.Returns(true);
            authenticationService.Identity.Returns("test@test.com");

            await middleware.InvokeAsync(context);

            await next.Received()(Arg.Any<HttpContext>());
            messageService.Received().Dispatch(Arg.Is<UpdateUserLastOnlineCommand>(c => c.Email == "test@test.com"));
        }

        [TestMethod]
        public async Task UserOnlineMiddleware_InvokeAsync_Dispatches_UserOnlineCommand_For_Post_Request() {
            var context = GetHttpContext("POST", false);

            authenticationService.IsAuthenticated.Returns(true);
            authenticationService.Identity.Returns("test@test.com");

            await middleware.InvokeAsync(context);

            await next.Received()(Arg.Any<HttpContext>());
            messageService.Received().Dispatch(Arg.Is<UpdateUserLastOnlineCommand>(c => c.Email == "test@test.com"));
        }

        [TestMethod]
        public async Task UserOnlineMiddleware_InvokeAsync_Dispatches_UserOnlineCommand_For_Ajax_Post_Request() {
            var context = GetHttpContext("POST", true);

            authenticationService.IsAuthenticated.Returns(true);
            authenticationService.Identity.Returns("test@test.com");

            await middleware.InvokeAsync(context);

            await next.Received()(Arg.Any<HttpContext>());
            messageService.Received().Dispatch(Arg.Is<UpdateUserLastOnlineCommand>(c => c.Email == "test@test.com"));
        }

        [TestMethod]
        public async Task UserOnlineMiddleware_InvokeAsync_Does_Not_Dispatch_UserOnlineCommand_When_Not_Authenticated() {
            var context = GetHttpContext("POST", false);

            authenticationService.IsAuthenticated.Returns(false);

            await middleware.InvokeAsync(context);

            await next.Received()(Arg.Any<HttpContext>());
            messageService.DidNotReceiveWithAnyArgs().Dispatch(default(UpdateUserLastOnlineCommand));
        }

        [TestMethod]
        public async Task UserOnlineMiddleware_InvokeAsync_Does_Not_Dispatch_UserOnlineCommand_For_Ajax_Get_Request() {
            var context = GetHttpContext("GET", true);

            authenticationService.IsAuthenticated.Returns(true);
            authenticationService.Identity.Returns("test@test.com");

            await middleware.InvokeAsync(context);

            await next.Received()(Arg.Any<HttpContext>());
            messageService.DidNotReceiveWithAnyArgs().Dispatch(default(UpdateUserLastOnlineCommand));

        }

        private static HttpContext GetHttpContext(string requestMethod, bool isAjaxRequest) {
            var context = Substitute.For<HttpContext>();
            var request = Substitute.For<HttpRequest>();

            context.Request.Returns(request);
            request.Method.Returns(requestMethod);

            if (isAjaxRequest) {
                request.Headers.Returns(new HeaderDictionary() { { "X-Requested-With", "XMLHttpRequest" } });
            }

            return context;
        }
    }
}
