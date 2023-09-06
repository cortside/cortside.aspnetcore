using System;

namespace Cortside.AspNetCore {
    /// <summary>
    /// Specifies how to treat the time value when converting between string and <see cref="DateTime"/>.
    /// </summary>
    public enum DateTimeHandling {
        /// <summary>
        /// Treat as local time. If the <see cref="DateTime"/> object represents a Coordinated Universal Time (UTC), it is converted to the local time.
        /// </summary>
        Local = 0,

        /// <summary>
        /// Treat as a UTC. If the <see cref="DateTime"/> object represents a local time, it is converted to a UTC.
        /// </summary>
        Utc = 1
    }
}
