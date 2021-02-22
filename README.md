
# Introduction

Jolt is a front-end framework with the goal of making front-end development as effortless as possible.
The aim is to use utilize only the modern coding patterns that are known to reduce bloat, heavy syntax and complexity.


# Key features

 *  C# programming language running browser native as Javascript.

 *  Strongly typed use of HTTP API's.

 *  Uses modern dependency injection patterns.
 
 *  Integrates with browser history and navigation.
 
 *  Heavily customizable by replacing the built-in services.
 

# C# language

Projects using the Jolt framework are written in C#, which is transpiled to Javascript to run natively in any browser.
This is under the belief that C# is a more productive coding language than Javascript if the proper UX- framework is provided.
That is if the application is front-end heavy, like Single-Page-Applications.

Currently [Bridge.NET](https://github.com/bridgedotnet/Bridge) is used for Javascript transpilation, however unfortunately it does not seem maintained for the last 2 years.
A longterm plan for this framework could be to adapt it to the Blazor WebAssembly. This should be possible while keeping the consumer code mostly compatible.

Eventually we could use a different C# to Javascript transpiler than Bridge.NET.
[H5](https://github.com/theolivenbaum/h5) seems to make an effort by modernizing Bridge.NET to .NET 5, however it could use more support and contributors.


# Getting started

 * [Controls 1: The basics](https://github.com/ChrML/Bridge.Jolt/blob/main/Docs/Controls01_Basics.md)
 * [Controls 2: Lifecycles](https://github.com/ChrML/Bridge.Jolt/blob/main/Docs/Controls02_Lifecycle.md)
 * [Services 1: Basic usage](https://github.com/ChrML/Bridge.Jolt/blob/main/Docs/Services01_Basics.md)
 * [Services 2: Creating custom services](https://github.com/ChrML/Bridge.Jolt/blob/main/Docs/Services02_Custom.md)
 * [Navigation 1: Basic usage](https://github.com/ChrML/Bridge.Jolt/blob/main/Docs/Navigation01_Basics.md)
 

# Development
 * [Starting the demo application](https://github.com/ChrML/Bridge.Jolt/blob/main/Docs/Start01.md)


# Tasks left to accomplish

 * [ ]  Test coverage using xUnit.
 * [ ]  Add more standard controls.
 * [ ]  Add forms based on type-descriptors.
 * [ ]  Adapt to a different compiler such as Blazor WebAssembly.

