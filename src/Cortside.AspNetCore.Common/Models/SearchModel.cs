using System.ComponentModel.DataAnnotations;

namespace Cortside.AspNetCore.Common.Models {
    public class SearchModel {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 30;

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>
        /// The sort.
        /// </value>
        public string Sort { get; set; } = "-CreatedDate";
    }

}
