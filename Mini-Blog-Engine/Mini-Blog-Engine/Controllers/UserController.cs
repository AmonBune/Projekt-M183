using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Dashboard()
        {

            var current_user = (string)Session["role"];
            var user_roles = MvcApplication.UserRoles;
            var current_user_role = (string)user_roles[current_user];

            if (current_user_role == "User")
            {
                // access granted

                SqlConnection connection = new SqlConnection();
                if (System.Security.Principal.WindowsIdentity.GetCurrent().Name == "GAMER-LAPTOP\\Gamer-Beast")
                {
                    connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Gamer-Beast\\Documents\\git\\Projekt-M183\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
                }
                else
                {
                    connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Jan\\Documents\\Schule\\M183\\Test\\Projekt-M183\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
                }

                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                cmd.CommandText = "SELECT * FROM [dbo].[Post]";
                cmd.Connection = connection;

                connection.Open();

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    List<object> table = new List<object>(); // LIST FOR EVERYTHING
                    while (reader.Read())
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                table.Add(reader.GetString(i));
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    table.Add(reader.GetInt32(i));
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                    
                    ViewBag.table = table;

                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ViewBag.Message = "Wrong Credentials";
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}