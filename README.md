# Cortside.AspNetCore

## Cortside.AspNetCore

```csharp
// warm all the serivces up, can chain these together if needed
services.AddStartupTask<WarmupServicesStartupTask>();
```

```csharp
// setup and register boostrapper and it's installers
services.AddBootStrapper<DefaultApplicationBootStrapper>(Configuration, o => {
    o.AddInstaller(new IdentityServerInstaller());
    o.AddInstaller(new NewtonsoftInstaller());
    o.AddInstaller(new SubjectPrincipalInstaller());
    o.AddInstaller(new SwaggerInstaller());
    o.AddInstaller(new ModelMapperInstaller());
});
```

```csharp
// add response compression using gzip and brotli compression
services.AddDefaultResponseCompression(CompressionLevel.Optimal);
```

## Cortside.AspNetCore.ApplicationInsights

```csharp
services.AddApplicationInsights(serviceName, instrumentationKey);
```

## Cortside.AspNetCore.Common

This contains model classes that are common, i.e:
* PageResult/PagedList - model thow has information about a total resultset as well as the items for a single page of results 
* ListResult - model that has results wrapped in common shape, matching PagedResult

## Cortside.AspNetCore.AccessControl

```csharp
// Add the access control using IdentityServer and PolicyServer
services.AddAccessControl(Configuration);
```

## Cortside.AspNetCore.Swagger

```csharp
// Add swagger with versioning and OpenID Connect configuration using Newtonsoft
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var versions = new List<OpenApiInfo> {
    new OpenApiInfo {
        Version = "v1",
        Title = "Acme.ShoppingCart API",
        Description = "Acme.ShoppingCart API",
    },
    new OpenApiInfo {
        Version = "v2",
        Title = "Acme.ShoppingCart API",
        Description = "Acme.ShoppingCart API",
    }
};
services.AddSwagger(Configuration, xmlFile, versions);
```

```csharp
app.UseSwagger("Acme.ShoppingCart Api", provider);
```

## Cortside.AspNetCore.Auditable

Contains base class AuditableEntity for domain entities as well as Subject domain entity needed by it.

```csharp
// add SubjectPrincipal for auditing
services.AddSubjectPrincipal();
```

```csharp
// intentionally set after UseAuthentication
app.UseMiddleware<SubjectMiddleware>();
```

## Cortside.AspNetCore.EntityFramework

Contains the following:

* AuditableDatabaseContext
* UnitOfWorkContext
* IUnitOfWork
* QueryableExtensions for getting paged results