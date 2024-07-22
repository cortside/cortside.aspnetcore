using System;
using System.Collections.Generic;
using System.Net.Http;
using Cortside.AspNetCore.Filters;
using Cortside.Common.Messages.MessageExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Cortside.Common.Messages.Tests.Filters {
    public class MessageExceptionResponseFilterTest : IDisposable {
        private readonly MessageExceptionResponseFilter filter = null;
        private readonly LoggerFactory loggerFactory = new LoggerFactory();
        public MessageExceptionResponseFilterTest() {
            filter = new MessageExceptionResponseFilter(new Logger<MessageExceptionResponseFilter>(loggerFactory));
        }

        [Theory]
        [MemberData(nameof(GetPassThroughScenarios))]
        public void ShouldPassThroughCertainScenarios(ActionExecutedContext context) {
            // act
            filter.OnActionExecuted(context);

            // assert
            Assert.False(context.ExceptionHandled);
            Assert.Null(context.Result);
        }

        [Theory]
        [MemberData(nameof(GetCommonMessageExceptionScenarios))]
        public void ShouldWriteResponseForCommonMessageExceptions(MessageException message, Func<IActionResult, bool> comparison) {
            // arrange
            ActionExecutedContext context = GetActionExecutedContext();
            context.Exception = message;

            // act
            filter.OnActionExecuted(context);

            // assert
            Assert.True(context.ExceptionHandled);
            Assert.NotNull(context.Result);
            Assert.True(comparison(context.Result));
        }

        [Fact]
        public void ShouldGenerateErrorModel() {
            // arrange
            var messages = new MessageList() {
                new MissingRequiredFieldError("property1"),
                new InvalidTypeFormatError("property2", "abc")
            };

            // act
            var ex = new ValidationListException(messages);
            var model = filter.GetErrorsModel(ex);

            // assert
            Assert.NotNull(model);
            Assert.NotEmpty(model.Errors);
            Assert.Equal(2, model.Errors.Count);

            // check each one
            Assert.Equal("MissingRequiredFieldError", model.Errors[0].Type);
            Assert.Equal("property1", model.Errors[0].Property);
            Assert.Equal("property1 is required.", model.Errors[0].Message);

            // second
            Assert.Equal("InvalidTypeFormatError", model.Errors[1].Type);
            Assert.Equal("property2", model.Errors[1].Property);
            Assert.Equal("abc is not a valid value for property2.", model.Errors[1].Message);
        }

        public static IEnumerable<object[]> GetCommonMessageExceptionScenarios() {
            yield return new object[] { new NotFoundResponseException(), (Func<IActionResult, bool>)((result) => result is NotFoundObjectResult) };
            yield return new object[] { new UnprocessableEntityResponseException(), (Func<IActionResult, bool>)((result) => ((ObjectResult)result).StatusCode == StatusCodes.Status422UnprocessableEntity) };
            yield return new object[] { new ForbiddenAccessResponseException(), (Func<IActionResult, bool>)((result) => ((ObjectResult)result).StatusCode == StatusCodes.Status403Forbidden) };
        }

        public static IEnumerable<object[]> GetPassThroughScenarios() {
            var successContext = GetActionExecutedContext();
            yield return new object[] { successContext };

            var exceptionContext = GetActionExecutedContext();
            exceptionContext.Exception = new Exception();
            yield return new object[] { exceptionContext };
        }

        private static ActionExecutedContext GetActionExecutedContext() {
            var controllerMock = new Mock<Controller>();
            var filters = new List<IFilterMetadata>();
            var actionContext = new ActionContext() {
                HttpContext = new Mock<HttpContext>().Object,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor() {
                    ActionName = "index",
                    ControllerName = "home"
                }
            };
            return new ActionExecutedContext(actionContext, filters, controllerMock.Object);
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            loggerFactory.Dispose();
        }
    }
}
