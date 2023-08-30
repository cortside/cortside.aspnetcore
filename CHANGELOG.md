# Release 6.0

* Update version number to match framework version (6.x)
* Update nuget dependencies to latest stable versions
* Cleanup of deprecated dependencies

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
