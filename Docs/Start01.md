
# Development: Demo application

This repository also contains a functional demo application that is consuming the framework.

A typical application will consume the `Bridge.Jolt` package from the NuGet Package Manager.
Starting this demo application is intended only for testing changes made to this repository itself.


## Prerequisites
 * Visual Studio 2019 Community / Professional / Enterprise
 * .NET 5.0 SDK
 * .NET Framework 4.7.2 SDK (will soon be deprecated)
 * Windows (only because we still use .NET Framework 4.7.2)


## Run the application

1. Clone this repository.
   ```
   git clone https://github.com/ChrML/Bridge.Jolt.git
   ```

2. Open `Bridge.Jolt/Bridge.Jolt.sln` in Visual Studio 2019.

3. Configure `WebServer.Demo` as the Startup Project.

4. Build the solution.\
   NOTE: Due to a currently unknown bug, sometimes you will get build errors that look like this the first time.
         If that happens, you should restart Visual Studio and build again.
   ```
   error CS0012: The type 'IEnumerable<>' is defined in an assembly that is not referenced. You must add a reference to assembly 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
   ```

5. Launch the application as self-hosted (F5)
