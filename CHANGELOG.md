# Release 8.0

* Updated powershell scripts to latest versions from coeus/shoppingcart-api
* Standardized library build files and resolved code coverage issues
* Update target framework to net8.0
* Update all dependency nuget packages
* Add/Fix build badges
* Transition to use Shouldly instead of FluentAssertions
* Add handling of PreconditionFailedResponseException to return 412 in MessageExceptionResponseFilter
* Add new EF Core interceptor for handling of audit responsibilities, moving functionality from AuditableDatabaseContext, this will allow other custom implemented database context classes to still benefit from audit stamping
* Broke out some common model builder methods from AuditableDatabaseContext to new ModelBuilderExtensions extension class
* Add Authorization parameter to swagger model
* Add definition for CustomSchemaId and CustomOperationIds to swagger definition
* Add helper extension methods for ToPagedResult and ToListResult on IList<T>
* Add IConfiguration support for values to have substitutable values anywhere in configuration with IConfiguration extension method ExpandTemplates
* Conditionally add security to swagger model if identity authority is configured
* Add support for Cortside.Authorization in AccessControlConfiguration

|Commit|Date|Author|Message|
|---|---|---|---|
| 0d3e1c5 | <span style="white-space:nowrap;">2024-09-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 2e19b1d | <span style="white-space:nowrap;">2024-09-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 4bd9e9b | <span style="white-space:nowrap;">2024-09-27</span> | <span style="white-space:nowrap;">Erik</span> |  add precondition failed
| 9b6e152 | <span style="white-space:nowrap;">2024-09-27</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #30 from cortside/feature/pre-condition-failed
| 521db7d | <span style="white-space:nowrap;">2024-09-27</span> | <span style="white-space:nowrap;">Erik</span> |  Merge remote-tracking branch 'remotes/origin/master' into feature/merge-master
| 10abbf4 | <span style="white-space:nowrap;">2024-09-27</span> | <span style="white-space:nowrap;">Erik</span> |  (origin/feature/merge-master) update cortside.common packages
| 5807943 | <span style="white-space:nowrap;">2024-10-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  merge master
| ad7c57e | <span style="white-space:nowrap;">2024-10-24</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Add new AuditableInterceptor, moving most of setting audit stap logic to there; moved 2 model builder methods to extension methods insted of by inheritance
| 3d8cf28 | <span style="white-space:nowrap;">2024-10-24</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  project cleanup
| 4a1ea34 | <span style="white-space:nowrap;">2024-10-24</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  change values from private to protected so that they are available to derived classes
| 41f6582 | <span style="white-space:nowrap;">2024-10-24</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  remove unneeded extension method overload
| 8b257c6 | <span style="white-space:nowrap;">2024-11-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  merge master
| 30fb3c2 | <span style="white-space:nowrap;">2024-11-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add custom operationsIds for swagger
| 65468d0 | <span style="white-space:nowrap;">2024-11-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| c62ea63 | <span style="white-space:nowrap;">2024-11-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| fe07c7f | <span style="white-space:nowrap;">2024-11-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add helper extensions for result classes
| 5d7880d | <span style="white-space:nowrap;">2024-12-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 1bc6dbc | <span style="white-space:nowrap;">2025-01-08</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages
| 3bb083b | <span style="white-space:nowrap;">2025-01-08</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages
| b6e5bdc | <span style="white-space:nowrap;">2025-01-15</span> | <span style="white-space:nowrap;">=</span> |  config extension for templates
| f9184c8 | <span style="white-space:nowrap;">2025-01-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #34 from cortside/feature/substitutions
| 80ce069 | <span style="white-space:nowrap;">2025-01-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  check to see if identity authority is setup before adding security to swagger
| 22a9d2d | <span style="white-space:nowrap;">2025-01-21</span> | <span style="white-space:nowrap;">=</span> |  authorization-api
| 286d8d1 | <span style="white-space:nowrap;">2025-01-22</span> | <span style="white-space:nowrap;">=</span> |  update tests
| fe40a7d | <span style="white-space:nowrap;">2025-01-22</span> | <span style="white-space:nowrap;">=</span> |  remove package ref
| 48f23c7 | <span style="white-space:nowrap;">2025-01-23</span> | <span style="white-space:nowrap;">=</span> |  codacy
| 4f32908 | <span style="white-space:nowrap;">2025-01-24</span> | <span style="white-space:nowrap;">=</span> |  deal with no icu
| d71e3da | <span style="white-space:nowrap;">2025-01-24</span> | <span style="white-space:nowrap;">=</span> |  bump version
| 5db56ac | <span style="white-space:nowrap;">2025-01-24</span> | <span style="white-space:nowrap;">=</span> |  to 8
| 4998981 | <span style="white-space:nowrap;">2025-01-24</span> | <span style="white-space:nowrap;">=</span> |  only test 8
| a5bd33a | <span style="white-space:nowrap;">2025-01-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #35 from cortside/feature/ENG-4482-authorization
| e9484c3 | <span style="white-space:nowrap;">2025-02-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to net8
| 81f0ff6 | <span style="white-space:nowrap;">2025-02-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to net8
| 0b350c7 | <span style="white-space:nowrap;">2025-02-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to net8
| 9b720da | <span style="white-space:nowrap;">2025-03-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  reformatting
| fd8c9da | <span style="white-space:nowrap;">2025-03-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Use Shouldly instead of FluentAssertions because of new licensing; reformat source;
| 186eda3 | <span style="white-space:nowrap;">2025-03-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/8.0, origin/develop, origin/HEAD, develop) update packages
****

# Release 6.3

## Breaking changes

* AuditableEntity corrected FK column from CreateSubjectId to CreatedSubjectId
    * Requires a migration to be created

## Changes

* Update nuget dependencies to latest stable versions
* Add common model classes for searches
* PageList calculates TotalPages
* AuditableDatabaseContext adds BlankTriggerConversion to tables for EF 8+
    * This is required by EF 8+ to know if there are triggers on EF tables
* Change from Microsoft.AspNetCore.Mvc.Versioning to supported Asp.Versioning
* Add WebApiFactory and WebApiFixture to simplify integration test fixture
    * See example of use here: https://github.com/cortside/coeus/blob/develop/shoppingcart-api/src/Acme.ShoppingCart.WebApi.IntegrationTests/IntegrationFixture.cs
* WebApiBuilder adds support for
    * WithHostBuilder
    * WithWebHostBuilder
    * WithLoggerConfiguration
    * Examples
    ```csharp
    var builder = WebApiHost.CreateBuilder(args)
        .WithWebHostBuilder(builder => {
            builder.UseSentry(o => {
                o.Dsn = "<snip>.ingest.us.sentry.io/<snip>";
                o.Environment = "dev";
                o.Debug = true;
                o.Release = "VeryGroovyService.0.0.1";
                o.AddEntityFramework();
                o.CaptureFailedRequests = true;
                o.SendDefaultPii = true;
                o.TracesSampleRate = 1.0; // Capture 100% of the application's transactions
                o.ProfilesSampleRate = 1.0; // profile 100% of the captured transactions
                o.AddIntegration(new ProfilingIntegration(
                    TimeSpan.FromMilliseconds(500) // wait 500 ms for profiling to start
                ));
            });

        })
        .WithLoggerConfiguration(cfg => {
            cfg.WriteTo.Sentry(s => {
                s.InitializeSdk = false;
                s.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                s.MinimumEventLevel = LogEventLevel.Warning;
            });
        })
        .UseStartup<Startup>();
    ```
* Add MessageExceptionResponseFilter from Cortside.Common.Messages
    * Along with ErrorsModel, ErrorModel
* Add CorrelationMiddleware, moved from Cortside.Common.Messages
* Add HttpCorrelationContext to help set CorrelationContext with information from request

|Commit|Date|Author|Message|
|---|---|---|---|
| b8a4781 | <span style="white-space:nowrap;">2024-01-10</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 66e3f9f | <span style="white-space:nowrap;">2024-01-10</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| c7748cd | <span style="white-space:nowrap;">2024-01-10</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  skip commits that only contain version changes (should avoid building develop after merge from master)
| 3df3b35 | <span style="white-space:nowrap;">2024-01-10</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 7032750 | <span style="white-space:nowrap;">2024-01-23</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 3e22fee | <span style="white-space:nowrap;">2024-03-27</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add support for WebApplicationFactory to be able to add configuration to the running application
| 70f0e4c | <span style="white-space:nowrap;">2024-03-27</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add support for WebApplicationFactory to be able to add configuration to the running application
| 7aacc43 | <span style="white-space:nowrap;">2024-03-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add support for WebApplicationFactory to be able to add configuration to the running application
| b3ffeb7 | <span style="white-space:nowrap;">2024-03-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  move integration testing classes to package that can be shared
| 2128807 | <span style="white-space:nowrap;">2024-03-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  move integration testing classes to package that can be shared
| 6dfd460 | <span style="white-space:nowrap;">2024-03-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  move integration testing classes to package that can be shared
| 75214e4 | <span style="white-space:nowrap;">2024-04-19</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  fix fk name from Create to Created
| 0bf7ba8 | <span style="white-space:nowrap;">2024-06-03</span> | <span style="white-space:nowrap;">rosin</span> |  Configuration action injection idea
| 70e32f1 | <span style="white-space:nowrap;">2024-06-03</span> | <span style="white-space:nowrap;">rosin</span> |  Rename methods to accord with .net official names
| 16f84a7 | <span style="white-space:nowrap;">2024-06-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #22 from somniamble/fix/expose-host-builder
| 0958a62 | <span style="white-space:nowrap;">2024-06-04</span> | <span style="white-space:nowrap;">rosin</span> |  Allow configuring Serilog using callable action
| c26defb | <span style="white-space:nowrap;">2024-06-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #23 from somniamble/feature/configure-serilog
| 9988b4d | <span style="white-space:nowrap;">2024-06-14</span> | <span style="white-space:nowrap;">Erik</span> |  mitigation for triggers in ef7+
| dc8f6df | <span style="white-space:nowrap;">2024-07-17</span> | <span style="white-space:nowrap;">Erik</span> |  fix for net6
| 944f52c | <span style="white-space:nowrap;">2024-07-17</span> | <span style="white-space:nowrap;">Erik</span> |  simplify a tad
| 4b8accd | <span style="white-space:nowrap;">2024-07-22</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  Migrate asp.net code from common
| 970a459 | <span style="white-space:nowrap;">2024-07-22</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  update package from common.messages, fix tests
| b730e8a | <span style="white-space:nowrap;">2024-07-22</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  remove this unused code
| 1ebfef1 | <span style="white-space:nowrap;">2024-07-22</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  fix codeql warnings
| e182d53 | <span style="white-space:nowrap;">2024-07-22</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  minor cleanup
| 34f8e1b | <span style="white-space:nowrap;">2024-07-22</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  codacy fixes
| bc81ec4 | <span style="white-space:nowrap;">2024-07-23</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #25 from cortside/migrate-aspnetcore
| 01b43f7 | <span style="white-space:nowrap;">2024-07-25</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  Add TotalPages to PagedList<> DTO
| d4d4de3 | <span style="white-space:nowrap;">2024-07-25</span> | <span style="white-space:nowrap;">Braden Edmunds</span> |  Add test
| 2419896 | <span style="white-space:nowrap;">2024-07-25</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #26 from cortside/feature/total-pages
| c53c52e | <span style="white-space:nowrap;">2024-07-26</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #24 from cortside/feature/hastriggersconvention
| 443bdf5 | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Erik</span> |  search & paginated request stuff
| a5bde22 | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Erik</span> |  one more
| 0c72877 | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Erik</span> |  sync
| ded0a41 | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Erik</span> |  move
| b0e2458 | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/feature/search-models, feature/search-models) rename parameter to match shoppingcart-api
| ff187aa | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Erik</span> |  limit range for model validation
| 557873f | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Erik</span> |  Merge branch 'feature/search-models' of github.com:cortside/cortside.aspnetcore into feature/search-models
| abdfef1 | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #27 from cortside/feature/search-models
| e6deac8 | <span style="white-space:nowrap;">2024-07-31</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  move TimedHostedService here
| b436ce5 | <span style="white-space:nowrap;">2024-08-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  move error models from Cortside.Common.Messages
| ea94fac | <span style="white-space:nowrap;">2024-08-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; move correlationcontext and hosting classes back to common and reference from there
| 5563c1d | <span style="white-space:nowrap;">2024-08-07</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages
| f56ab3c | <span style="white-space:nowrap;">2024-09-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update scripts; update nuget packages
| b4612c2 | <span style="white-space:nowrap;">2024-09-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/6.3, origin/develop, origin/HEAD, develop) merge from master
****

# Release 6.2

* Update nuget dependencies to latest stable versions
* Update serilog libraries to latest for better net6.0/net8.0 multitargeting 

|Commit|Date|Author|Message|
|---|---|---|---|
| 68592f0 | <span style="white-space:nowrap;">2023-11-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 9dfd443 | <span style="white-space:nowrap;">2023-11-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'release/6.1' into develop
| 5f3e760 | <span style="white-space:nowrap;">2023-11-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| c194d0c | <span style="white-space:nowrap;">2023-11-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| aef12ea | <span style="white-space:nowrap;">2023-11-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| d1b799e | <span style="white-space:nowrap;">2023-11-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| c4e8aaf | <span style="white-space:nowrap;">2023-12-22</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget package with updating to latest of serilog
| 549a772 | <span style="white-space:nowrap;">2023-12-22</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget package with updating to latest of serilog
| 31dc1ba | <span style="white-space:nowrap;">2024-01-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/update-serilog, update-serilog) update nuget package vesions and latest powershell scripts
| 2105387 | <span style="white-space:nowrap;">2024-01-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/6.2, origin/develop, origin/HEAD, develop) Merge pull request #19 from cortside/update-serilog
****

# Release 6.1

* Added delegates for DbContextOptionsBuilder and SqlServerDbContextOptionsBuilder to AddDbContext so that things could be customized
	```csharp
	// add database context with interfaces
	services.AddDatabaseContext<IDatabaseContext, DatabaseContext>(Configuration, o => {
		o.EnableSensitiveDataLogging(true);  // the default is actually false
	});
	```
* Added support through InternalDateTimeHandling to control serializer settings and handling around datetimes.  The default is to handle as UTC, meaning that values in will be deserialized to a DateTimeKind.UTC.  This should be kept most of the time as services should run in and process datetimes as UTC.  For services that must run in a specific timezone, InternalDateTimeHandling.Local can be used.  Apis should accept any value ISO8601 datetime and convert to UTC or local as specified.  Api responses should be IS0-8601 in UTC (Z).
	```csharp
	// add controllers and all of the api defaults
	services.AddApiDefaults(InternalDateTimeHandling.Utc, options => {
		options.Filters.Add<MyResponseFilter>();
	});
	```
* AddApiControllers/AddApiDefaults sets SuppressAsyncSuffixInActionNames to false to disable the name munging
* Add customer model binder UtcDateTimeModelBinder to handle deserializing value based on InternalDateTimeHandling
* WarmupServicesStartupTask now instantiates all controllers as well as anything registered as a singleton.  This was done to further bring forward DI registration issues to service starting rather than at request time.

|Commit|Date|Author|Message|
|---|---|---|---|
| 502bb33 | <span style="white-space:nowrap;">2023-08-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  generate changelog
| 74e3a08 | <span style="white-space:nowrap;">2023-08-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| 0be753d | <span style="white-space:nowrap;">2023-08-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update release notes
| f5b8a31 | <span style="white-space:nowrap;">2023-08-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (release/6.0) update release notes
| f8f83ee | <span style="white-space:nowrap;">2023-08-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/release/6.0, origin/master) Merge pull request #15 from cortside/release/6.0
| b26ef65 | <span style="white-space:nowrap;">2023-09-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 34f3e54 | <span style="white-space:nowrap;">2023-09-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-22] wip for timezone/date handling
| 86e3f11 | <span style="white-space:nowrap;">2023-09-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-22] wip for timezone/date handling
| 3de1046 | <span style="white-space:nowrap;">2023-09-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-22] timezone/date handling allowing for dates to be parsed as utc or local based on handling setting
| 294036c | <span style="white-space:nowrap;">2023-09-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-22] timezone/date handling allowing for dates to be parsed as utc or local based on handling setting
| 8f023ec | <span style="white-space:nowrap;">2023-09-07</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-17] timezone/date handling allowing for dates to be parsed as utc or local based on handling setting
| 7d6e586 | <span style="white-space:nowrap;">2023-09-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ISSUE-17] cleanup
| ec759b2 | <span style="white-space:nowrap;">2023-09-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/ISSUE-22) [ISSUE-17] cleanup
| a26178e | <span style="white-space:nowrap;">2023-09-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #16 from cortside/ISSUE-22
| 663f4b7 | <span style="white-space:nowrap;">2023-09-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (ISSUE-22) [ISSUE-17] cleanup
| 77d47ea | <span style="white-space:nowrap;">2023-09-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'ISSUE-22' into develop
| be05251 | <span style="white-space:nowrap;">2023-10-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add resolution of controllers to warmup task
| 6343a42 | <span style="white-space:nowrap;">2023-10-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  use SuppressAsyncSuffixInActionNames to disable the name munging
| 9c3da4d | <span style="white-space:nowrap;">2023-10-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add success test for warmup task
| b5c1b39 | <span style="white-space:nowrap;">2023-10-25</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  use Action delegates for options instead of passing in pieces parts
| e976c86 | <span style="white-space:nowrap;">2023-10-25</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  use Action delegates for options instead of passing in pieces parts
| 0af23fc | <span style="white-space:nowrap;">2023-11-07</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  make client registration a singleton per reshsharp recommendations
| 4d55f62 | <span style="white-space:nowrap;">2023-11-07</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> develop, origin/develop, origin/HEAD) move AddRestApiClient extention methods to Cortside.RestApiClient libary and remove duplicate of TokenRequest for one in OpenIdConnectAuthenticator
****

# Release 6.0

* Update version number to match framework version (6.x)
* Update nuget dependencies to latest stable versions
* Cleanup of deprecated dependencies
* Change log properties to RequestClientId, RequestUserPrincipalName, RequestSubjectId from ClientId, UserPrincipalName, SubjectId to make them more clear and less likely to conflict with other logging properties

|Commit|Date|Author|Message|
|---|---|---|---|
| 88b7b7f | <span style="white-space:nowrap;">2023-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| c0c273e | <span style="white-space:nowrap;">2023-06-20</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 55a03e2 | <span style="white-space:nowrap;">2023-07-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  wip logging notes
| 52fb52e | <span style="white-space:nowrap;">2023-07-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| cab7c5b | <span style="white-space:nowrap;">2023-07-14</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  prefix subject middleware with Request for logged values
| 678c40a | <span style="white-space:nowrap;">2023-07-17</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version to 6.x to be in line with dotnet and net6 version numbers
| 314b133 | <span style="white-space:nowrap;">2023-08-30</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/6.0, origin/develop, origin/HEAD, develop) update to latest nuget packages
****

# Release 1.3

|Commit|Date|Author|Message|
|---|---|---|---|
| 1883d1b | <span style="white-space:nowrap;">2023-01-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version
| fd1a71c | <span style="white-space:nowrap;">2023-01-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| e7289d3 | <span style="white-space:nowrap;">2023-02-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 7948ac9 | <span style="white-space:nowrap;">2023-02-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add dto classes to go with auditable entity classes
| 3e9c1a3 | <span style="white-space:nowrap;">2023-02-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'master' into develop
| 77901a3 | <span style="white-space:nowrap;">2023-03-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [SVC-1237] use new LoggerConfiguration extension method that has more setup for bowdlerizer
| a9994b1 | <span style="white-space:nowrap;">2023-03-01</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [SVC-1237] use previous build image
| 68ad7cb | <span style="white-space:nowrap;">2023-03-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/SVC-1237, SVC-1237) [SVC-1237] update to use merged serilog.bowdlerizer
| 28965b6 | <span style="white-space:nowrap;">2023-03-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #12 from cortside/SVC-1237
| 5697de8 | <span style="white-space:nowrap;">2023-04-10</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add entity comments for data dictionary
| 10477b1 | <span style="white-space:nowrap;">2023-04-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ARC-120] add new logging middleware and app insights telementy initializer for logging request ip address
| 468a7a2 | <span style="white-space:nowrap;">2023-05-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ARC-136] add ef core interceptor to add query hints
| 9a32184 | <span style="white-space:nowrap;">2023-05-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/ARC-136, ARC-136) [ARC-136] add ef core interceptor to add query hints
| c3118a3 | <span style="white-space:nowrap;">2023-05-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #13 from cortside/ARC-136
| 984ea18 | <span style="white-space:nowrap;">2023-05-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ARC-136] add service colleciton extention method to register database context
| 0fdf15f | <span style="white-space:nowrap;">2023-05-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ARC-136] add service colleciton extention method to register database context
| 66a3286 | <span style="white-space:nowrap;">2023-05-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ARC-136] add service colleciton extention method to register database context
| fe28a85 | <span style="white-space:nowrap;">2023-05-24</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add additional extention methods to make registrations easier
| e65bd6a | <span style="white-space:nowrap;">2023-05-24</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add additional extention methods to make registrations easier
| 58db718 | <span style="white-space:nowrap;">2023-05-24</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add additional extention methods to make registrations easier
| f845963 | <span style="white-space:nowrap;">2023-05-31</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add extention method for EncryptionService
| b4a4cad | <span style="white-space:nowrap;">2023-06-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  allow for specifying MessageExceptionResponseFilter
| 8962e99 | <span style="white-space:nowrap;">2023-06-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add virtual method that can be overridden to add more pre-save logic
| 76e0ee7 | <span style="white-space:nowrap;">2023-06-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  allow for list of filters when creating api controllers
| 0e3f2cf | <span style="white-space:nowrap;">2023-06-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  allow for list of filters when creating api controllers
| bb4936d | <span style="white-space:nowrap;">2023-06-07</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  allow for output formatters
| 5870e95 | <span style="white-space:nowrap;">2023-06-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Create codeql.yml
| 36e6bbd | <span style="white-space:nowrap;">2023-06-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add github actions for lint analysis
| b126e18 | <span style="white-space:nowrap;">2023-06-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  address lint warnings
| 80b9158 | <span style="white-space:nowrap;">2023-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update powershell scripts
| da5f5e7 | <span style="white-space:nowrap;">2023-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/1.3, origin/develop, origin/HEAD, develop) update to latest cortside libraries
****

# Release 1.2
|Commit|Date|Author|Message|
|---|---|---|---|
| e9eaacd | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20220713] updated nuget packages
| c25f868 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| 238d8cd | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| 02e7244 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| b8e9b67 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #9 from cortside/feature/BOT-20220713
| aa0de6c | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20220713] updated nuget packages
| 8d2b1eb | <span style="white-space:nowrap;">2022-07-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add exception handler to complete task so that correlationId still happens in response headers
| 72cc454 | <span style="white-space:nowrap;">2022-07-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'develop' of github.com:cortside/Cortside.AspNetCore into develop
| 56c4c77 | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  remove registration of unneeded filter
| a10950e | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add sonar to build
| bb1a562 | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add sonar to build
| 4a8d8ff | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add sonar to build
| fc4a83b | <span style="white-space:nowrap;">2022-08-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  have AddApiDefaults and AddApiControllers return IMvcBuilder so that other builder related things are possible
| af4b3b6 | <span style="white-space:nowrap;">2022-09-26</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  can't ignore obsolete actions or it will show as breaking change with swagger diff when it's not really yet
| 965b637 | <span style="white-space:nowrap;">2022-10-10</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add validation of configuration for AddAccessControl along with tests
| ef59cdc | <span style="white-space:nowrap;">2022-12-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget api key
| 34dfb33 | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| 1251660 | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| c809243 | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| 831631e | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| 49cba9c | <span style="white-space:nowrap;">2022-12-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add prebuild
| 14dc3da | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20230102] updated nuget packages
| 162677d | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update helper scripts; update nuget packages
| ecb640d | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version in prep for next release
| 57bc332 | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/feature/BOT-20230102, feature/BOT-20230102) update to vs2022 for .net 6 builds
| 20b10db | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #10 from cortside/feature/BOT-20230102
| bc6beeb | <span style="white-space:nowrap;">2023-01-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to latest nuget packages
| a7936d2 | <span style="white-space:nowrap;">2023-01-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> release/1.2, origin/develop, origin/HEAD, develop) initial changelog
****
# Release 1.0
|Commit|Date|Author|Message|
|---|---|---|---|
| 8301d90 | <span style="white-space:nowrap;">2022-04-18</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Initial commit
| 977fd62 | <span style="white-space:nowrap;">2022-04-18</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  initial commit as extracted from coeus
| d085fe6 | <span style="white-space:nowrap;">2022-04-18</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to vs2022
| 3ffe38b | <span style="white-space:nowrap;">2022-04-18</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  set version to 1.0
| 5e27160 | <span style="white-space:nowrap;">2022-04-19</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  fix namespaces
| 2166381 | <span style="white-space:nowrap;">2022-04-19</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add project for common models
| a807cb2 | <span style="white-space:nowrap;">2022-04-20</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add update to subject
| 86ea70f | <span style="white-space:nowrap;">2022-04-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  adding more extension methods to make configuration easier
| 832c2a9 | <span style="white-space:nowrap;">2022-04-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  adding more extension methods to make configuration easier
| b2dec0f | <span style="white-space:nowrap;">2022-04-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add more extensions for common Startup configuration
| 10ce228 | <span style="white-space:nowrap;">2022-04-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add more extensions for common Startup configuration
| 075ff00 | <span style="white-space:nowrap;">2022-04-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add more extensions for common Startup configuration
| 74e2869 | <span style="white-space:nowrap;">2022-04-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add more extensions for common Startup configuration
| 404a285 | <span style="white-space:nowrap;">2022-04-22</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add more extensions for common Startup configuration
| dd34476 | <span style="white-space:nowrap;">2022-04-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add additional information for access control configuration
| 05dcd4e | <span style="white-space:nowrap;">2022-05-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Update README.md
| abc56fa | <span style="white-space:nowrap;">2022-05-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Update README.md
| 670952f | <span style="white-space:nowrap;">2022-05-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Update README.md
| 904f2b0 | <span style="white-space:nowrap;">2022-05-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Update README.md
| e435ec1 | <span style="white-space:nowrap;">2022-05-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add new webapi with builder to hold common/default setup
| 76df09d | <span style="white-space:nowrap;">2022-05-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  refactor builder
| 40046ef | <span style="white-space:nowrap;">2022-05-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  refactor/cleanup of builder
| d9ee6a1 | <span style="white-space:nowrap;">2022-05-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add logging of kestrel start with listing addresses
| 449a37c | <span style="white-space:nowrap;">2022-05-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add default configuration to kestrel
| 80ed61d | <span style="white-space:nowrap;">2022-05-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add default configuration to kestrel
| 9466ccc | <span style="white-space:nowrap;">2022-05-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add some additional logging
| 4787e52 | <span style="white-space:nowrap;">2022-05-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  working to get kestrel configuration of urls working
| a04590f | <span style="white-space:nowrap;">2022-05-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add workaround for url configuration bug
| 54a789e | <span style="white-space:nowrap;">2022-05-06</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  make auditable entity subject properties virtual for projects using lazy proxies
| 63f3de1 | <span style="white-space:nowrap;">2022-06-07</span> | <span style="white-space:nowrap;">Juan Gomez</span> |  skip additional rows count
| 4fa23cb | <span style="white-space:nowrap;">2022-06-09</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #1 from jgabrielgv/develop
| 2eb293d | <span style="white-space:nowrap;">2022-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [NoOpTransaction] don't have BeginNoTracking dispose of the context when done
| 5c3fbc9 | <span style="white-space:nowrap;">2022-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [nooptransaction] add build and sonar badges to README.md
| abd84a6 | <span style="white-space:nowrap;">2022-06-15</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [NoOpTransaction] don't have BeginNoTracking dispose of the context when done, no execution on commit/rollback
| dbe61cb | <span style="white-space:nowrap;">2022-06-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [nooptransaction] add tx type that will change the context's change tracker back from as not tracking at end of tx/scope
| 35c1ee4 | <span style="white-space:nowrap;">2022-06-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #2 from cortside/nooptransaction
| 9cdc6f6 | <span style="white-space:nowrap;">2022-06-16</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #3 from cortside/develop
| 3098997 | <span style="white-space:nowrap;">2022-06-21</span> | <span style="white-space:nowrap;">Juan Gomez</span> |  [PP-2319] use custom subject
| 32e61e2 | <span style="white-space:nowrap;">2022-06-21</span> | <span style="white-space:nowrap;">Juan Gomez</span> |  using instead of full namespace
| 717c40c | <span style="white-space:nowrap;">2022-06-22</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [PP-2319] update to latest package versions
| 4ff9179 | <span style="white-space:nowrap;">2022-06-22</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [PP-2319] update to latest package versions
| c9acd3a | <span style="white-space:nowrap;">2022-06-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #4 from cortside/usecustomsubject
| 722f910 | <span style="white-space:nowrap;">2022-06-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #5 from cortside/develop
| 288486b | <span style="white-space:nowrap;">2022-07-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ValidationListException] optimizations to configuring services and webapplication
| 56d8e43 | <span style="white-space:nowrap;">2022-07-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ValidationListException] add extension methods for configuring webapi builder
| e2566ab | <span style="white-space:nowrap;">2022-07-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ValidationListException] add extention method to add controllers with defaults
| dade935 | <span style="white-space:nowrap;">2022-07-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ValidationListException] update to latest nuget packages
| a8087f4 | <span style="white-space:nowrap;">2022-07-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ValidationListException] add extention method to add controllers with defaults
| ae5787f | <span style="white-space:nowrap;">2022-07-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ValidationListException] add extention method to add controllers with defaults
| 6a74c73 | <span style="white-space:nowrap;">2022-07-05</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [ValidationListException] add extention methods to simplify setup
| 10e4462 | <span style="white-space:nowrap;">2022-07-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #6 from cortside/ValidationListException
| 4e020e1 | <span style="white-space:nowrap;">2022-07-11</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update to latest nuget packages
| ec2c429 | <span style="white-space:nowrap;">2022-07-12</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/master) Merge pull request #7 from cortside/develop
| e9eaacd | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20220713] updated nuget packages
| c25f868 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| 238d8cd | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| 02e7244 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  handle git flow named branches
| b8e9b67 | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #9 from cortside/feature/BOT-20220713
| aa0de6c | <span style="white-space:nowrap;">2022-07-13</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20220713] updated nuget packages
| 8d2b1eb | <span style="white-space:nowrap;">2022-07-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add exception handler to complete task so that correlationId still happens in response headers
| 72cc454 | <span style="white-space:nowrap;">2022-07-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge branch 'develop' of github.com:cortside/Cortside.AspNetCore into develop
| 56c4c77 | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  remove registration of unneeded filter
| a10950e | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add sonar to build
| bb1a562 | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add sonar to build
| 4a8d8ff | <span style="white-space:nowrap;">2022-08-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add sonar to build
| fc4a83b | <span style="white-space:nowrap;">2022-08-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  have AddApiDefaults and AddApiControllers return IMvcBuilder so that other builder related things are possible
| af4b3b6 | <span style="white-space:nowrap;">2022-09-26</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  can't ignore obsolete actions or it will show as breaking change with swagger diff when it's not really yet
| 965b637 | <span style="white-space:nowrap;">2022-10-10</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add validation of configuration for AddAccessControl along with tests
| ef59cdc | <span style="white-space:nowrap;">2022-12-21</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget api key
| 34dfb33 | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| 1251660 | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| c809243 | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| 831631e | <span style="white-space:nowrap;">2022-12-28</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update nuget packages; use connectionstring for applicationInsights configuration
| 49cba9c | <span style="white-space:nowrap;">2022-12-29</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  add prebuild
| 14dc3da | <span style="white-space:nowrap;">2023-01-02</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  [feature/BOT-20230102] updated nuget packages
| 162677d | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update helper scripts; update nuget packages
| ecb640d | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  update version in prep for next release
| 57bc332 | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (origin/feature/BOT-20230102, feature/BOT-20230102) update to vs2022 for .net 6 builds
| 20b10db | <span style="white-space:nowrap;">2023-01-03</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  Merge pull request #10 from cortside/feature/BOT-20230102
| bc6beeb | <span style="white-space:nowrap;">2023-01-04</span> | <span style="white-space:nowrap;">Cort Schaefer</span> |  (HEAD -> develop, origin/develop, origin/HEAD) update to latest nuget packages
****
