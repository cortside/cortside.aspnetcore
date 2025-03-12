using System;

namespace Cortside.AspNetCore.Tests.Controllers {
    public class PostData {
        //[JsonConverter(typeof(CustomDateTimeConverter), new object[] { "MM-dd-yyyy" })]
        public DateTime DateFrom { get; set; }

        //[JsonConverter(typeof(CustomDateTimeConverter), new object[] { "MM-dd-yyyy" })]
        public DateTime? DateTo { get; set; }
    }
}
