namespace Cortside.AspNetCore.AccessControl {
    public class AccessControlConfiguration {
        public string AuthorizationProvider { get; set; } = AccessControlProviders.PolicyServer;
    }

    public static class AccessControlProviders {
        public static readonly string PolicyServer = nameof(PolicyServer);
        public static readonly string AuthorizationApi = nameof(AuthorizationApi);
    }
}
