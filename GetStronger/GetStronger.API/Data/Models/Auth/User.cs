using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Auth;
internal class User: IdentityUser
{
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }
}
