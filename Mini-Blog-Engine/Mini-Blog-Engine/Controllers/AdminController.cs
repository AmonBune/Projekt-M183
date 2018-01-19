using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult Dashboard()
        {
            
            var current_user = (string)Session["role"];
            var user_roles = MvcApplication.UserRoles;
            var current_user_role = (string)user_roles[current_user];

            if (current_user_role == "Administrator")
            {

                // access granted

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }        
    }
}