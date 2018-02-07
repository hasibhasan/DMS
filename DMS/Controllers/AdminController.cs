using DataModel.Models.DB;
using DataModel.Models.EntityManager;
using DataModel.Models.ViewModel;
using DMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    { 
              
        
        //
        // GET: /Admin/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            AdminManager adm = new AdminManager();
            var BenDetails = adm.GetBenDetails();
            return View(BenDetails);
        }       

        public ActionResult Admins()
        {
           /*
            AdminManager adm = new AdminManager();
            var users= adm.GetAllUsers();  
            return View("~/Views/Admin/UserRole/Index.cshtml",users);*/
            return null;
        }




	}
}