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
                    var tableContent1 = "";
                    while (reader.Read())
                    {
                        List<object> 
                        tableContent1 = reader.GetString(0);
                        tableContent2 = reader.GetString(1);
                        tableContent3 = reader.GetString(2);
                        tableContent4 = reader.GetString(3);
                        tableContent5 = reader.GetString(4);
                        tableContent6 = reader.GetString(5);
                        tableContent7 = reader.GetString(6);
                        tableContent8 = reader.GetString(7);
                    }

                    ViewBag.TableContent0 = tableContent0;
                    ViewBag.TableContent1 = tableContent1;
                    ViewBag.TableContent1 = tableContent1;
                    ViewBag.TableContent1 = tableContent1;
                    ViewBag.TableContent1 = tableContent1;
                    ViewBag.TableContent1 = tableContent1;
                    ViewBag.TableContent1 = tableContent1;
                    ViewBag.TableContent1 = tableContent1;

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