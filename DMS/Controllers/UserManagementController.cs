using DataModel.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DMS.Controllers
{
    
    public class UserManagementController : Controller
    {
        //
        // GET: /UserManagement/
        public ActionResult Index()
        {
            UserManagementManager umm = new UserManagementManager();
            var a = umm.GetAllUsers();         
            return View(a);
        }

        //
        // GET: /UserManagement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserManagement/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserManagement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /UserManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserManagement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UserManagement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // POST: /Role/Delete/5
        [HttpPost, ActionName("RoleDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult UserDeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserManagementManager umm = new UserManagementManager();
                umm.UserDelete(id);

            }
            return RedirectToAction("Index");
        }
    }
}
