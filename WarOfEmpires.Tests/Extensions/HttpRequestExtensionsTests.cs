using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Extensions;

namespace WarOfEmpires.Tests.Extensions {
    [TestClass]
    public class HttpRequestExtensionsTests {
        [TestMethod]
        public void HttpRequestExtensions_IsAjaxRequest_Is_True_When_Header_Is_Present() {
            var request = Substitute.For<HttpRequest>();

            request.Headers.Returns(new HeaderDictionary() {
                { "X-First", "Foo" },
                { "X-Requested-With", "XMLHttpRequest" },
                { "X-Other", "Bar" }
            });

            request.IsAjaxRequest().Should().BeTrue();
        }
        
        [TestMethod]
        public void HttpRequestExtensions_IsAjaxRequest_Is_False_When_Header_Is_Not_Present() {
            var request = Substitute.For<HttpRequest>();

            request.Headers.Returns(new HeaderDictionary() {
                { "X-First", "Foo" },
                { "X-Other", "Bar" }
            });

            request.IsAjaxRequest().Should().BeFalse();
        }
    }
}
