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
    public class RoleController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            AdminManager adm = new AdminManager();
            return View("~/Views/Admin/Role/Index.cshtml", adm.GetAllRole());
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdminManager adm = new AdminManager();
            RoleViewModel role = adm.SelectRole(id);
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
                AdminManager adm = new AdminManager();

                if (adm.CreateRole(roleViewModel))
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
            AdminManager adm = new AdminManager();
            RoleViewModel role = adm.SelectRole(id);
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
                AdminManager adm = new AdminManager();
                adm.UpdateRole(roleViewModel);
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
            AdminManager adm = new AdminManager();
            RoleViewModel role = adm.SelectRole(id);
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
            AdminManager adm = new AdminManager();
            adm.RoleDelete(id);
            return RedirectToAction("Index");
        }        

        protected override void Dispose(bool disposing)
        {
            AdminManager adm = new AdminManager();
            adm.Dispose();
            base.Dispose(disposing);
        }
    }
}
