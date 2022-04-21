# Cortside.AspNetCore

## Cortside.AspNetCore

```csharp
// warm all the serivces up, can chain these together if needed
services.AddStartupTask<WarmupServicesStartupTask>();
```

```csharp
services.AddBootStrapper<DefaultApplicationBootStrapper>(Configuration, o => {
    o.AddInstaller(new IdentityServerInstaller());
    o.AddInstaller(new NewtonsoftInstaller());
    o.AddInstaller(new SubjectPrincipalInstaller());
    o.AddInstaller(new SwaggerInstaller());
    o.AddInstaller(new ModelMapperInstaller());
});
```

## Cortside.AspNetCore.ApplicationInsights

```csharp
services.AddApplicationInsights(serviceName, instrumentationKey);
```

## Cortside.AspNetCore.Common

This contains model classes that are common, i.e:
* PageResult/PagedList - model thow has information about a total resultset as well as the items for a single page of results 
* ListResult - model that has results wrapped in common shape, matching PagedResult
