using DataModel.Models.DB;
using DataModel.Models.ViewModel;
using DMS.Models;
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
    public class AdminManager
    {
        private DDataEntities db = new DDataEntities();

        public List<RoleViewModel> GetAllRole()
        {
            
            var roles = db.AspNetRoles.Select(o => new RoleViewModel
            {
                Id = o.Id,
                Name =  o.Name                
            }).ToList();

            return roles;            
        }        

        public RoleViewModel SelectRole(string id)
        {
            
            var roles = db.AspNetRoles.Find(id);
            var roleViewMode = new RoleViewModel{
                Id= roles.Id,
                Name = roles.Name
            };

            return roleViewMode;            
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

      
        public bool UpdateRole(RoleViewModel roleViewModel){

            try
            {
                var newViewModel = db.AspNetRoles.Find(roleViewModel.Id);

                newViewModel.Id = roleViewModel.Id;
                newViewModel.Name = roleViewModel.Name;

                db.Entry(newViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch{
                return false;
            }

            
            
        }


    }
}
