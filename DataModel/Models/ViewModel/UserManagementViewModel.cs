﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models.ViewModel
{
    public class UserManagementViewModel
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }


        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }


        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }


        [Display(Name = "NID")]
        public string NID { get; set; }

        [Display(Name = "Assigned Role")]
        public IList<string> Rolelist { get; set; }
    }
   
}
