using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Dashboard()
        {

            var current_user = (string)Session["username"];
            var user_roles = MvcApplication.UserRoles;
            var current_user_role = (string)user_roles[current_user];

            if (current_user_role == "User")
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