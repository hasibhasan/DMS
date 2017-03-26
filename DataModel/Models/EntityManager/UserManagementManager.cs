using DataModel.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DMS.Models;

namespace DataModel.Models.EntityManager
{
    public class UserManagementManager
    {
          private ApplicationDbContext context = new ApplicationDbContext();        

          public List<UserManagementViewModel> GetAllUsers()
          {

              List<UserManagementViewModel> UserRoleList = new List<UserManagementViewModel>();
              var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


              var userlist = context.Users.OrderBy(r => r.UserName).ToList();
              foreach (var item in userlist){
                var roles = userManager.GetRoles(item.Id);               
                  UserManagementViewModel userRoleListViewModel = new UserManagementViewModel
                  {
                      Id = item.Id,
                      UserName= item.UserName,
                      Address = item.Address,
                      Email = item.Email,
                      Gender = item.Gender,
                      NID = item.NID,
                      Phone = item.Phone
                   
                     
                      
                  };
                  UserRoleList.Add(userRoleListViewModel);
              }

              return UserRoleList;

          }
          public async Task<bool> UserDelete(string id)
          {
              try
              {
                  var usermanager = new UserManager<Microsoft.AspNet.Identity.EntityFramework.IdentityUser>(new UserStore<IdentityUser>(new ApplicationDbContext()));
                  var user = await usermanager.FindByIdAsync(id);
                  var logins = user.Logins;
                  var rolesForUser = await usermanager.GetRolesAsync(id);
                  using (var transaction = context.Database.BeginTransaction())
                  {
                      foreach (var login in logins.ToList())
                      {
                          await usermanager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                      }

                      if (rolesForUser.Count() > 0)
                      {
                          foreach (var item in rolesForUser.ToList())
                          {
                              // item should be the name of the role
                              var result = await usermanager.RemoveFromRoleAsync(user.Id, item);
                          }
                      }
                      await usermanager.DeleteAsync(user);
                      transaction.Commit();
                  }

                  return true;
              }
              catch
              {
                  return false;
              }
          }
          
    }
}
