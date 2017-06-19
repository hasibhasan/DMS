using DataModel.Models.EntityManager;
using DataModel.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DMS.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        //
        // GET: /UserManagement/
        public ActionResult Index()
        {
            UserManagementManager umm = new UserManagementManager();
            var userlist = umm.GetAllUsers();         
            return View("~/Views/Admin/UserManagement/Index.cshtml",userlist);
        }

        //
        // GET: /UserManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserManagementManager umm = new UserManagementManager();
            UserManagementViewModel uvv = umm.SelectUserDetails(id);
            if (uvv == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/UserManagement/Details.cshtml", uvv);
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
        public ActionResult Edit(string id)
        {
            return View();
        }

        //
        // POST: /UserManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
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

        public ActionResult Delete(string id)
        {
            return View();
            
        }

        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    UserManagementManager umm = new UserManagementManager();
                    var res = await umm.UserDelete(id);

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> LockUserAccount(string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    UserManagementManager umm = new UserManagementManager();
                    var res = await umm.LockUserAccount(id,365);

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> UnlockUserAccount(string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    UserManagementManager umm = new UserManagementManager();
                    var res = await umm.UnlockUserAccount(id);

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult UploadAvatar()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = System.Guid.NewGuid().ToString("N") + testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = System.Guid.NewGuid().ToString("N") + file.FileName;
                        }
                        string directory = "~/Uploads/";
                        // Get the complete folder path and store the file inside it.  
                        string dbPath = directory + fname;
                        fname = Path.Combine(Server.MapPath(directory), fname);
                        file.SaveAs(fname);
                        UserManagementManager umm = new UserManagementManager();
                        UserManagementViewModel um = new UserManagementViewModel
                        {
                            Id = User.Identity.GetUserId(),                            
                            ImagePath = dbPath.Replace(@"~", "")

                        };
                        umm.UpdateUserImage(um);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }  



        protected override void Dispose(bool disposing)
        {
            UserManagementManager umm = new UserManagementManager();
            umm.Dispose();
            base.Dispose(disposing);
        }
        
    }
}
