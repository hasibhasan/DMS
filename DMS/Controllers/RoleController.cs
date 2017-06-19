using DataModel.Models.EntityManager;
using DataModel.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            RoleManager rm = new RoleManager();
            return View("~/Views/Admin/Role/Index.cshtml", rm.GetAllRole());
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RoleManager rm = new RoleManager();
            RoleViewModel role = rm.SelectRole(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Role/Details.cshtml", role);
        }


        // GET: /Role/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View("~/Views/Admin/Role/Create.cshtml");
        }

        // POST: /Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Name")] RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                RoleManager rm = new RoleManager();

                if (rm.CreateRole(roleViewModel))
                {
                    return RedirectToAction("Index");
                }

            }

            return View("~/Views/Admin/Role/Index.cshtml", roleViewModel);
        }

        // GET: /Role/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleManager rm = new RoleManager();
            RoleViewModel role = rm.SelectRole(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Role/Edit.cshtml", role);
        }

        // POST: /Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Name")] RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                RoleManager rm = new RoleManager();
                rm.UpdateRole(roleViewModel);
                return RedirectToAction("Index");
            }
            return View("~/Views/Admin/Role/Edit.cshtml", roleViewModel);
        }

        // GET: /Role/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleManager rm = new RoleManager();
            RoleViewModel role = rm.SelectRole(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Role/Delete.cshtml", role);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(string id)
        {
            RoleManager rm = new RoleManager();
            rm.RoleDelete(id);
            return RedirectToAction("Index");
        }        

        protected override void Dispose(bool disposing)
        {
            RoleManager rm = new RoleManager();
            rm.Dispose();
            base.Dispose(disposing);
        }
    }
}
