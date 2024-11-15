using System.Collections.Generic;

namespace Cortside.AspNetCore.Common.Models {
    /// <summary>
    /// Extension methods for generic IList 
    /// </summary>
    public static class IListExtensions {
        /// <summary>
        /// Convert IList&lt;T&gt; to ListResult&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ListResult<T> ToListResult<T>(this IList<T> list) {
            var result = new ListResult<T>(list);
            return result;
        }

        /// <summary>
        /// Convert IList&lt;T&gt; to PagedResult&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="totalItems"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedResult<T> ToPagedResult<T>(this IList<T> list, int totalItems, int pageNumber, int pageSize) {
            var result = new PagedResult<T>() {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Results = list
            };
            return result;
        }
    }
}
