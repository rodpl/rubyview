### Requirements ###
NET 3.5 is only needed to build and test project. You don't need Visual Studio. Project is self-sufficient for build and test process. Only PATH to MSBuild.exe from NET 3.5 is needed.

### Build commands ###
To build:
  1. `msbuild Default.proj /t:BuildAll`

To run acceptance tests:
  1. `StartSelenium.bat`
  1. `StartTestSite.bat`
  1. `msbuild Default.proj /t:RunAcceptanceTests`

To run integration tests:
  * `msbuild Default.proj /t:RunIntegrationTests`

To run unit tests:
  * `msbuild Default.proj /t:RunUnitTests`

To run performance tests:
  * `msbuild Default.proj /t:RunPerformanceTests`

To run all tests at once:
  1. `StartSelenium.bat`
  1. `StartTestSite.bat`
  1. `msbuild Default.proj /t:RunAllTests`