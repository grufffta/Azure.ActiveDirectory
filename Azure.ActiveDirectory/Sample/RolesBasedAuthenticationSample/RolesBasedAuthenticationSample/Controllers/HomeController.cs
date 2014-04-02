using System.Security.Permissions;
using System.Web.Mvc;
using Azure.ActiveDirectory;

namespace RolesBasedAuthenticationSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IActiveDirectoryGraphClient _graphClient = new GraphClient();

        [Authorize]
        public ActionResult UserProfile()
        {
            return View(_graphClient.CurrentUser);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public ActionResult Admin()
        {
            ViewBag.Message = "Your admin page.";

            return View();
        }

        [PrincipalPermission(SecurityAction.Demand, Role="Member")]
        public ActionResult Member()
        {
            ViewBag.Message = "Your member page.";

            return View();
        }
    }
}