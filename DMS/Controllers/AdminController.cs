using DataModel.Models.DB;
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
    public class AdminController : Controller
    {

        private DDataEntities db = new DDataEntities();
        //
        // GET: /Admin/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Role()
        {
            return View("~/Views/Admin/Role/Index.cshtml", db.AspNetRoles.ToList());
        }
        public ActionResult RoleDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Role/Details.cshtml", aspnetrole);
        }

        // GET: /Role/Create
        public ActionResult RoleCreate()
        {
            return View("~/Views/Admin/Role/Create.cshtml");
        }

        // POST: /Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate([Bind(Include = "Name")] AspNetRole aspnetrole)
        {
            if (ModelState.IsValid)
            {
             


                var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                if (!roleManager.RoleExists(aspnetrole.Name))
                    {
                        var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                        role.Name = aspnetrole.Name;
                        roleManager.Create(role);                      
                    }           


                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/Role/Index.cshtml", aspnetrole);
        }

        // GET: /Role/Edit/5
        public ActionResult RoleEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Role/Edit.cshtml",aspnetrole);
        }

        // POST: /Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleEdit([Bind(Include = "Id,Name")] AspNetRole aspnetrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspnetrole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Views/Admin/Role/Edit.cshtml",aspnetrole);
        }

        // GET: /Role/Delete/5
        public ActionResult RoleDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            if (aspnetrole == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Role/Delete.cshtml",aspnetrole);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult RoleDeleteConfirmed(string id)
        {
            AspNetRole aspnetrole = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspnetrole);
            db.SaveChanges();
            return RedirectToAction("Role");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




	}
}