using Jolt.Controls;
using Jolt.Demo.CustomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jolt.Demo.Controls
{
    class UserProfileDemo: HtmlControl
    {

        public UserProfileDemo(IUserProfileService userProfiles)
        {
            this.userProfiles = userProfiles;
            this.Dom.Append(new Label("Test"));
        }




        readonly IUserProfileService userProfiles;

    }
}
