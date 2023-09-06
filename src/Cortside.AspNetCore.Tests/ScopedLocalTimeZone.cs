using System;
using System.Reflection;

namespace Cortside.AspNetCore.Tests {
    public class ScopedLocalTimeZone : IDisposable {
        private readonly TimeZoneInfo _actualLocalTimeZoneInfo;

        private static void SetLocalTimeZone(TimeZoneInfo timeZoneInfo) {
            var info = typeof(TimeZoneInfo).GetField("s_cachedData", BindingFlags.NonPublic | BindingFlags.Static);
            object cachedData = info.GetValue(null);

            var field = cachedData.GetType().GetField("_localTimeZone", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Instance);
            field.SetValue(cachedData, timeZoneInfo);
        }

        public ScopedLocalTimeZone(TimeZoneInfo timeZoneInfo) {
            _actualLocalTimeZoneInfo = TimeZoneInfo.Local;
            SetLocalTimeZone(timeZoneInfo);
        }

        public void Dispose() {
            TimeZoneInfo.ClearCachedData();
            SetLocalTimeZone(_actualLocalTimeZoneInfo);
        }
    }
}
