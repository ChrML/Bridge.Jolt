
# Services: Adding custom services

Custom services are useful when you have application features that will be used by multiple controls in your application.

Adding this code to a custom service will decouple that code from the controls.
This ensures that both the controls and the services can be fully tested separately, and that the code can be efficiently reused.


## Creating a custom API service

A common use scenario for custom services is to provide web api that may be accessed as strongly typed.
This is useful if you have common api's used by many separate controls in your application, and you don't want every control to know the details of how these api's work.

Here is an example service for getting and changing the current user's profile:

```c#
// This is the interface that your controls will use.
public interface IUserProfileService
{
    Task<UserProfileData> GetMyProfileAsync();

    Task SetMyProfileAsync(UserProfileData data);
}


// This is the data we expect to find in a user's profile.
public class UserProfileData
{
    public string FullName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}


// This is the private implementation of the service doing the actual work.
class UserProfileService : IUserProfileService
{
    public UserProfileService(IApiClient api)
    {
        this.api = api;
    }

    public async Task<UserProfileData> GetMyProfileAsync()
    {
        return await this.api.GetAsync<UserProfileData>("DemoData/MyProfile");
    }

    public async Task SetMyProfileAsync(UserProfileData data)
    {
        await this.api.PostAsync<UserProfileData>("DemoData/MyProfile", data);
    }

    readonly IApiClient api;
}
```

Notice that our implementation (`UserProfileService`) uses the built-in `IApiClient` service to call our api. 
It is fully valid for a service to depend on another service, in-fact this is a quite common use scenario.
Although possible, services should not depend on control classes.

To tell Jolt we have a custom service named `IUserProfileService`, we need to add this code to the beginning of our application's entrypoint:

```c#
AppServices.ConfigureServices(services =>
{
    services.AddSingleton<IUserProfileService, UserProfileService>();
});
```


## Using the IUserProfileService from a control

... TODO ...


## Using a Startup- class

Sometimes you will have an application with many services, or you may have more than one page/entrypoint that need the same services configured.

In these cases it could be a good idea to move your service configuration from the entrypoint to a `Startup` class. Here is an example startup class:

```c#
class Startup: IStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<IMyCustomService, MyCustomService>()
            .AddSingleton<IOtherService, OtherService>();
    }

    public void Configure(IServices services)
    {
        // You could do additional startup tasks here after the services are ready for use.
    }
}
```

To tell Jolt to use the `Startup` class, remove the existing service configuration from your application's entrypoint, then add this instead:

```c#
AppServices.UseStartup<Startup>();
```

This will enable you to do all service configuration in one easy-to-find place. This is similar to how .NET server applications are configured.
