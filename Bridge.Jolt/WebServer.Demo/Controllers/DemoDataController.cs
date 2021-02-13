using Microsoft.AspNetCore.Mvc;

namespace WebServer.Demo.Controllers
{
    /// <summary>
    /// Provides data for use by our demo application.
    /// </summary>
    public class DemoDataController : Controller
    {
        /// <summary>
        /// Endpoint for getting fictional user data.
        /// </summary>
        /// <returns></returns>
        public IActionResult CurrentUser()
        {
            return this.Ok(new
            {
                UserName = "Christian Lundheim",
                NickName = "CL"
            });
        }
    }
}
