using System.Collections.Generic;

namespace Cortside.AspNetCore.Common.Models {
    /// <summary>
    /// List of results
    /// </summary>
    /// <typeparam name="T">model</typeparam>
    public class ListResult<T> {
        public ListResult() {
        }

        public ListResult(IList<T> results) {
            Results = results;
        }

        public IList<T> Results { get; set; }
    }
}
