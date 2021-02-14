using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jolt.Demo.CustomService
{
    public interface IUserProfileService
    {
        Task<UserProfileData> GetMyProfileAsync();

        Task SetMyProfileDataAsync(UserProfileData data);
    }
}
