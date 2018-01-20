﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index(int id)
        {
            Models.BlogPost post = new Models.BlogPost();
            string sql = "SELECT * FROM [Post] WHERE [Id] = '" + id + "'";
            string sql = "SELECT * FROM [Post] WHERE [Id] = '" + id + "' AND [DeletedOn] IS NULL";
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
            SqlDataReader commentReader = createConnection(query2);
            List<Models.Comment> comments = new List<Models.Comment>();
            if (commentReader.HasRows)
            {
                while (commentReader.Read())
                {
                    Models.Comment comment = new Models.Comment();
                    comment.Text = commentReader.GetString(3);
                    comments.Add(comment);
                }
            }
            post.Comments = comments;
            return View("Index", post);
        }

        [HttpPost]
        public ActionResult PostComment(string content, int postid)
        {
            int id = (int)Session["userid"];
<<<<<<< HEAD
            string query = "INSERT INTO [Comment] (PostId, UserId, Commet, CreatedOn) VALUES (" + postid + ", " + id + ", '" + content + "', @insertdate)";
            SqlCommand command = insertData(query);
            command.Parameters.AddWithValue("insertdate", DateTime.Now);
            command.ExecuteNonQuery();
            return RedirectToAction("Index", new { id = postid });
=======
            if (content.Length <= 200)
            {
                string query = "INSERT INTO [Comment] (PostId, UserId, Commet, CreatedOn) VALUES (@postid, @userid, @content, @insertdate)";
                SqlCommand command = insertData(query);
                command.Parameters.AddWithValue("insertdate", DateTime.Now);
                command.Parameters.AddWithValue("postid", postid);
                command.Parameters.AddWithValue("content", RemoveSpecialCharacters(content));
                command.Parameters.AddWithValue("userid", id);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index", new {id=postid});
>>>>>>> 6240af45e099f1db66f1677838ff7704231292ff
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

        private SqlCommand insertData(string sql)
        {
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
            return sqlcommand;
        }

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}