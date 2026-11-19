using System;
using System.Collections.Generic;

namespace Cortside.AspNetCore.Common.Models {
    public class PagedResult<T> {
        public PagedResult() {
            Results = new List<T>();
        }

        public PagedResult(int totalItems, int pageNumber, int pageSize, IList<T> results) : this() {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Results = results;
        }

        /// <summary>
        /// Total number of items found for the given resource
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Current paged result page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Current paged result page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of paged result pages for given resource
        /// </summary>
        public int TotalPages =>
            PageSize <= 0
                ? 0
                // Integer division with ceiling rounding
                // See https://stackoverflow.com/a/4175152/539997
                : (TotalItems + PageSize - 1) / PageSize;

        /// <summary>
        /// Current paged result page of results
        /// </summary>
        public IList<T> Results { get; set; }
    }
}
