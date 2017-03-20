using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models.ViewModel
{
    public class AdminViewModel
    {

    }
    public class RoleViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string Name { get; set; }
    }
}
