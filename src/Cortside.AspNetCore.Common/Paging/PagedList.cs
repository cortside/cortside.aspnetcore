using System;
using System.Collections.Generic;

namespace Cortside.AspNetCore.Common.Paging {
    public class PagedList<T> {
        public PagedList() {
            Items = new List<T>();
        }

        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages {
            get { return (int)Math.Ceiling(TotalItems / (decimal)PageSize); }
        }

        public IList<T> Items { get; set; }

        public PagedList<TOutput> Convert<TOutput>(Func<T, TOutput> converter) {
            var result = new PagedList<TOutput> {
                TotalItems = TotalItems,
                PageNumber = PageNumber,
                PageSize = PageSize,
                Items = new List<TOutput>()
            };

            foreach (var item in Items) {
                result.Items.Add(converter(item));
            }

            return result;
        }
    }
}
