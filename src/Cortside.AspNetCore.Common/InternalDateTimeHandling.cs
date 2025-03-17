using System;

namespace Cortside.AspNetCore.Common {
    /// <summary>
    /// Specifies how to parse value when converting between string and <see cref="DateTime"/>.
    /// </summary>
    public enum InternalDateTimeHandling {
        /// <summary>
        /// Parse value with resulting date having DateTimeKind of Utc, adjust from any qualified offset to UTC if needed.
        /// </summary>
        Utc = 0,

        /// <summary>
        /// Parse value with resulting date having DateTimeKind of Local, adjust from any qualified offset to Local if needed.
        /// </summary>
        /// <remarks>
        /// This should only be used when dealing with a legacy system that deals in a specific singular timezone and where persisted values in database are likely persisted as local timezone.
        /// </remarks>
        Local = 1
    }
}
