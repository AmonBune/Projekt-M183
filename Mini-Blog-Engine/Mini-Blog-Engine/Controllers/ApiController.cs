using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        public IHttpActionResult Index()
        {
            return Ok();
        }

        public IHttpActionResult Index(string apitoken)
        {
            if (apitoken == null)
            {
                return NotFound();
            }
            return Ok(apitoken);
        }

        public IHttpActionResult Get()
        {
            return new TextResult("hello", Request);
        }

        public ActionResult About()
        {
            return null;
        }
    }
}