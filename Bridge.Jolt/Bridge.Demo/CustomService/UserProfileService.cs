using Jolt.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jolt.Demo.CustomService
{
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

        public async Task SetMyProfileDataAsync(UserProfileData data)
        {
            await this.api.PostAsync<UserProfileData>("DemoData/MyProfile", data);
        }

        readonly IApiClient api;
    }
}
