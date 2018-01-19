using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class HomeController : Controller
    {
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

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];
     
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Jan\\Documents\\Schule\\M183\\Test\\Projekt-M183\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT [Id], [Username], [Password], [Firstname], [Familyname], [Mobilephonenumber], [Role], [Status] FROM [dbo].[User] WHERE [Username] = '" + username + "' AND [Password] = '" + password + "'";
            cmd.Connection = connection;

            connection.Open();

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Session["username"] = username;
                string role = "";
                while (reader.Read())
                {
                    role = reader.GetString(6);
                }
                if (role == "admin")
                {
                    Session["role"] = "admin";
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (role == "user")
                {
                    Session["role"] = "user";
                    return RedirectToAction("Dashboard", "User");
                }
            }


            else {
                ViewBag.Message = "Wrong Credentials";
            }           

            return View();
        }
    }
}