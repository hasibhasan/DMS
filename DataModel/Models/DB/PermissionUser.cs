//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class PermissionUser
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public bool GrantPermission { get; set; }
        public long PermissionId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Permission Permission { get; set; }
    }
}