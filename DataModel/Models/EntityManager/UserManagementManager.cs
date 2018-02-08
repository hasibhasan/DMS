using DataModel.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DataModel.Models.DB;
using System.Data.Entity;

namespace DataModel.Models.EntityManager
{
    public class UserManagementManager
    {
         private ApplicationDbContext context = new ApplicationDbContext(); 
         public UserManagementManager()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
         {
         }

         public UserManagementManager(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
         public UserManager<ApplicationUser> UserManager { get; private set; }
          public List<UserManagementViewModel> GetAllUsers()
          {

              List<UserManagementViewModel> UserRoleList = new List<UserManagementViewModel>();
              //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


              var userlist = context.Users.OrderBy(r => r.UserName).ToList();
              foreach (var item in userlist){
                  var roles = UserManager.GetRoles(item.Id);               
                  UserManagementViewModel userRoleListViewModel = new UserManagementViewModel
                  {
                      Id = item.Id,
                      UserName= item.UserName,
                      FirstName =item.FirstName,
                      LastName =item.LastName,
                      Address = item.Address,
                      Email = item.Email,
                      Gender = item.Gender,
                      NID = item.NID,
                      Phone = item.Phone,
                      Rolelist = roles,
                      IsAproved = item.EmailConfirmed,
                      IsLocked = item.LockoutEnabled,
                      DateOfBirth = item.DateOfBirth
                  };
                  UserRoleList.Add(userRoleListViewModel);
              }

              return UserRoleList;

          }
          public UserManagementViewModel SelectUserDetails(string id)
          {
              

              var item = context.Users.Find(id);
              var roles = UserManager.GetRoles(item.Id);
              UserManagementViewModel umvm = new UserManagementViewModel
              {
                  Id = item.Id,
                  UserName = item.UserName,
                  FirstName = item.FirstName,
                  LastName = item.LastName,
                  Address = item.Address,
                  Email = item.Email,
                  Gender = item.Gender,
                  NID = item.NID,
                  Phone = item.Phone,
                  Rolelist = roles,
                  IsAproved = item.EmailConfirmed,
                  IsLocked = item.LockoutEnabled,
                  DateOfBirth = item.DateOfBirth,
                  ImagePath = item.ImagePath

              };

              return umvm;  


          }

          public String GetAvatar(string id)
          {
              var item = context.Users.Find(id);
              return item.ImagePath;
          }  


          public bool UpdateUserAccount(UserManagementViewModel umViewModel)
          {

              try
              {
                  var newViewModel = context.Users.Find(umViewModel.Id);                  
                  newViewModel.Id = umViewModel.Id;
                  newViewModel.UserName = umViewModel.UserName;
                  newViewModel.Address = umViewModel.Address;
                  newViewModel.Email = umViewModel.Email;
                  newViewModel.FirstName = umViewModel.FirstName;
                  newViewModel.Gender = umViewModel.Gender;
                  newViewModel.LastName = umViewModel.LastName;
                  newViewModel.NID = umViewModel.NID;
                  newViewModel.Phone = umViewModel.Phone;
                  newViewModel.DateOfBirth = umViewModel.DateOfBirth;
                  context.Entry(newViewModel).State = EntityState.Modified;
                  context.SaveChanges();
                  return true;
              }
              catch
              {
                  return false;
              }



          }
          public bool UpdateUserImage(UserManagementViewModel umViewModel)
          {

              try
              {
                  var newViewModel = context.Users.Find(umViewModel.Id);
                  newViewModel.Id = umViewModel.Id;
                  newViewModel.ImagePath = umViewModel.ImagePath;
                  context.Entry(newViewModel).State = EntityState.Modified;
                  context.SaveChanges();
                  return true;
              }
              catch
              {
                  return false;
              }



          }

          public async Task<bool> UserDelete(string id)
          {
              try
              {
                  //var usermanager = new UserManager<Microsoft.AspNet.Identity.EntityFramework.IdentityUser>(new UserStore<IdentityUser>(new ApplicationDbContext()));
                  var user = await UserManager.FindByIdAsync(id);
                  var logins = user.Logins;
                  var rolesForUser = await UserManager.GetRolesAsync(id);
                  using (var transaction = context.Database.BeginTransaction())
                  {
                      foreach (var login in logins.ToList())
                      {
                          await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                      }

                      if (rolesForUser.Count() > 0)
                      {
                          foreach (var item in rolesForUser.ToList())
                          {
                              // item should be the name of the role
                              var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                          }
                      }
                      await UserManager.DeleteAsync(user);
                      transaction.Commit();
                  }

                  return true;
              }
              catch
              {
                  return false;
              }
          }

          public async Task<bool> LockUserAccount(string userId, int? forDays)
          {
              var result = await UserManager.SetLockoutEnabledAsync(userId, true);
              if (result.Succeeded)
              {
                  if (forDays.HasValue)
                  {
                      result = await UserManager.SetLockoutEndDateAsync(userId, DateTimeOffset.UtcNow.AddDays(forDays.Value));
                  }
                  else
                  {
                      result = await UserManager.SetLockoutEndDateAsync(userId, DateTimeOffset.MaxValue);
                  }
                  return true;
              }
              else
              {
                  return false;
              }
              
          }
          public async Task<bool> UnlockUserAccount(string userId)
          {
              var result = await UserManager.SetLockoutEnabledAsync(userId, false);
              if (result.Succeeded)
              {
                  await UserManager.ResetAccessFailedCountAsync(userId);
                  return true;
              }
              else
              {
                  return false;
              }
              
          }



          private bool disposed = false;
          protected virtual void Dispose(bool disposing)
          {

              if (!this.disposed)
                  if (disposing)
                      context.Dispose();

              this.disposed = true;
          }


          public void Dispose()
          {

              Dispose(true);
              GC.SuppressFinalize(this);
          }
          
    }
}
