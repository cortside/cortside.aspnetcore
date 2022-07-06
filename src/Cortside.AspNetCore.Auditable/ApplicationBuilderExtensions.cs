using Cortside.AspNetCore.Auditable.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Cortside.AspNetCore {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseSubjectPrincipal(this IApplicationBuilder app) {
            app.UseMiddleware<SubjectMiddleware>();

            return app;
        }
    }
}
