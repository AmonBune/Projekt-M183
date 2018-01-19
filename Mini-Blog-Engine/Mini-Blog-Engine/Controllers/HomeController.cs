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
            List<Models.BlogPost> posts = new List<Models.BlogPost>();
            SqlDataReader reader = createConnection("SELECT * FROM [Post] WHERE [DeletedOn] IS NULL");
            if (reader.HasRows) {
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


            else {
                ViewBag.Message = "Wrong Credentials";
            }           

            return View();
        }

        private SqlDataReader createConnection(string sql) {
            SqlConnection connection = new SqlConnection();
            if (System.Security.Principal.WindowsIdentity.GetCurrent().Name == "GAMER-LAPTOP\\Gamer-Beast")
            {
                connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Gamer-Beast\\Documents\\git\\Projekt-M183\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
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

        public ActionResult Logout() {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult ViewPost(int? id)
        {
            Models.BlogPost post = new Models.BlogPost();
            string sql = "SELECT * FROM [Post] WHERE [Id] = '" + id + "'";
            string query2 = "SELECT * FROM [Comment] WHERE [PostId] = " + id + "";
            SqlDataReader reader = createConnection(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    post.Id = reader.GetInt32(0);
                    post.Title = reader.GetString(2);
                    post.Description = reader.GetString(3);
                    post.Content = reader.GetString(4);
                }
            }
            SqlDataReader reader2 = createConnection(query2);
            List<Models.Comment> comments = new List<Models.Comment>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Models.Comment comment = new Models.Comment();
                    comment.Text = reader2.GetString(2);
                    comments.Add(comment);
                }
            }
            post.Comments = comments;
            return View("Post", post);
        }
    }
}