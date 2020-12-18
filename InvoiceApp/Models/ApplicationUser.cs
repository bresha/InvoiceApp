using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
    }
}
