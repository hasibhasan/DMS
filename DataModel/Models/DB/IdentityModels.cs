using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace DataModel.Models.DB
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }                 
        public string Address { get; set; }      
        public string Phone { get; set; }       
        public string NID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImagePath { get; set; }




    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}