using DataModel.Models.DB;
using DataModel.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models.EntityManager
{
    public class RoleManager
    {
        private DDataEntities db = new DDataEntities();

        public List<RoleViewModel> GetAllRole()
        {

            try
            {
                var roles = db.AspNetRoles.Select(o => new RoleViewModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();
                return roles;
            }
            catch
            {
                return null;
            }



        }

        public RoleViewModel SelectRole(string id)
        {

            var roles = db.AspNetRoles.Find(id);
            var roleViewModel = new RoleViewModel
            {
                Id = roles.Id,
                Name = roles.Name
            };

            return roleViewModel;
        }
        public bool CreateRole(RoleViewModel roleViewModel)
        {

            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (!roleManager.RoleExists(roleViewModel.Name))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = roleViewModel.Name;
                roleManager.Create(role);
                return true;
            }
            else
            {
                return false;
            }

        }


        public bool UpdateRole(RoleViewModel roleViewModel)
        {

            try
            {
                var newViewModel = db.AspNetRoles.Find(roleViewModel.Id);

                newViewModel.Id = roleViewModel.Id;
                newViewModel.Name = roleViewModel.Name;

                db.Entry(newViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }



        }
        public bool RoleDelete(string id)
        {
            try
            {
                AspNetRole aspnetrole = db.AspNetRoles.Find(id);
                db.AspNetRoles.Remove(aspnetrole);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed)
                if (disposing)
                    db.Dispose();

            this.disposed = true;
        }


        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
