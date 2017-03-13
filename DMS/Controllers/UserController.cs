using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            return View();
        }
	}
}