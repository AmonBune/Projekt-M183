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
        /*
         * 1) Warum haben Sie sich für gerade für den Hash Algorithmus (Usernamen & Passwort) entschieden?
         * Antwort: WIr hatten keine Zeit, einen Hash Algorithmus einzubauen, aber wenn wir einen eingebaut hätten, hätten wir SHA-3 benutzt.
         * Weil es eine einfache Implementation davon gibt, und SHA-3 sehr sicher ist https://stackoverflow.com/questions/14061299/simple-implementation-of-sha-3-keccak-hashing-to-the-wrong-output-in-c
         * 
         * 2) In der User-Login-Tabelle ist noch ein Feld für die IP-Adresse Reserviert. Welche Attacke lässt sich dadurch verhindern?
         * Antwort: Session ID theft & Eavesdropping
         * 
         * 3) Erklären Sie, wie diese Attacke genau funktioniert und inwiefern die Gegenmassnahmen die Attacke vereitelt?
         * Antwort: Wenn ein Hacker eine Session ID eines legitimen Users kopiert und 1:1 verwendet, kann durch die IP-Adresse erkannt werden, dass ein Hacker versucht, unerlaubt auf den Account zuzugreifen.
         * 
        */

        public ActionResult Index()
        {
            List<Models.BlogPost> posts = new List<Models.BlogPost>();
            SqlDataReader reader = createConnection("SELECT * FROM [Post] WHERE [DeletedOn] IS NULL");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Models.BlogPost post = new Models.BlogPost();
                    post.Id = reader.GetInt32(0);
                    post.Title = reader.GetString(2);
                    post.Description = reader.GetString(3);
                    post.Content = reader.GetString(4);
                    posts.Add(post);
                }
            }
            return View(posts);
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

            string query = "SELECT [Id], [Username], [Password], [Firstname], [Familyname], [Mobilephonenumber], [Role], [Status] FROM [dbo].[User] WHERE [Username] = '" + username + "' AND [Password] = '" + password + "'";
            SqlDataReader reader = createConnection(query);

            if (reader.HasRows)
            {
                Session["username"] = username;
                string role = "";
                while (reader.Read())
                {
                    Session["userid"] = reader.GetInt32(0);
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
            else
            {
                ViewBag.Message = "Wrong Credentials";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        private SqlDataReader createConnection(string sql)
        {
            SqlConnection connection = new SqlConnection();
            if (System.Security.Principal.WindowsIdentity.GetCurrent().Name == "GAMER-LAPTOP\\Gamer-Beast")
            {
                connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Gamer-Beast\\Documents\\git\\Projekt-M183\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
            }
            else if (System.Security.Principal.WindowsIdentity.GetCurrent().Name == "GAMER-PC\\Amon Bune")
            {
                connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\Local Space\\git\\Projekt-M183\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
            }
            else
            {
                connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Jan\\Documents\\Schule\\M183\\Test\\Projekt-M183\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
            }
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.Connection = connection;
            sqlcommand.CommandText = sql;
            connection.Open();
            return sqlcommand.ExecuteReader();
        }
    }
}