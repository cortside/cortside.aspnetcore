namespace Cortside.AspNetCore.AccessControl {
    public class AccessControlConfiguration {
        public string AuthorizationProvider { get; set; } = AccessControlProviders.PolicyServer;
    }

    public static class AccessControlProviders {
        public const string PolicyServer = nameof(PolicyServer);
        public const string AuthorizationApi = nameof(AuthorizationApi);
    }
}
