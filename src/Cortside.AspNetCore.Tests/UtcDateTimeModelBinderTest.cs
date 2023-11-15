using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Cortside.AspNetCore.ModelBinding;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Cortside.AspNetCore.Tests {
    public class UtcDateTimeModelBinderTest {
        public static readonly object[][] Data = {
            new object[] { "2/10/2000 12:00:00 AM", new DateTime(2000, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
            new object[] { "2023-02-10T00:00:00-11:00", new DateTime(2023, 2, 10, 11, 0, 0, DateTimeKind.Utc) }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public async Task BindModelAsync_returns_success_with_with_expected_value(string modelValue, DateTime expectedResult) {
            // Arrange
            var modelBinder = new UtcDateTimeModelBinder(InternalDateTimeHandling.Utc);
            var bindingContext = BuildBindingContext(modelValue);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            bindingContext.Result.IsModelSet.Should().Be(true);
            var model = bindingContext.Result.Model as DateTime?;
            model.Value.Should().Be(expectedResult);
            model.Value.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public async Task BindModelAsync_invalid() {
            // Arrange
            var modelBinder = new UtcDateTimeModelBinder(InternalDateTimeHandling.Utc);
            var bindingContext = BuildBindingContext("invalid");

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            bindingContext.Result.IsModelSet.Should().Be(false);
            bindingContext.ModelState[bindingContext.ModelName].Errors.Count.Should().Be(1);
        }

        [Fact]
        public async Task BindModelAsync_does_not_bind_if_model_value_is_not_yes_or_no() {
            // Arrange
            var modelBinder = new DateTimeModelBinder(DateTimeStyles.AdjustToUniversal, new NullLoggerFactory());
            var bindingContext = BuildBindingContext("invalid");

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            bindingContext.Result.IsModelSet.Should().Be(false);
            bindingContext.ModelState[bindingContext.ModelName].Errors.Count.Should().Be(1);
        }

        private ModelBindingContext BuildBindingContext(string modelValue) {
            const string ModelName = "test";
            var bindingContext = new DefaultModelBindingContext { ModelName = ModelName };

            var bindingSource = new BindingSource("", "", false, false);
            var queryCollection = new QueryCollection(new Dictionary<string, StringValues>
            {
                { ModelName, new StringValues(modelValue) },
                { "Foo", new StringValues("1964/12/02 12:00:00") }
            });
            bindingContext.ValueProvider = new QueryStringValueProvider(bindingSource, queryCollection, null);
            bindingContext.ModelState = new ModelStateDictionary();
            bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(DateTime));

            return bindingContext;
        }
    }
}
