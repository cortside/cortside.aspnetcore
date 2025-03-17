using System.Collections.Generic;
using Cortside.AspNetCore.Common.Paging;
using Xunit;

namespace Cortside.AspNetCore.Tests {
    public class PagingDtoTest {
        [Fact]
        public void ShouldCalculateTotalPages() {
            // arrange
            var items = new List<string> {
                "str1",
                "str2",
                "str3",
                "str4",
                "str5",
                "str6",
                "str7",
            };

            // act
            var pagedDto = new PagedList<string> {
                Items = items,
                PageNumber = 1,
                PageSize = 5,
                TotalItems = items.Count,
            };

            // assert
            Assert.Equal(items.Count, pagedDto.Items.Count);
            Assert.Equal(2, pagedDto.TotalPages);
            Assert.Equal(5, pagedDto.PageSize);
            Assert.Equal(7, pagedDto.TotalItems);
        }
    }
}
