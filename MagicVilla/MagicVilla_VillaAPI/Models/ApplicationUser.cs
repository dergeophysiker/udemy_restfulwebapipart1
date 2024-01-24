using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace MagicVilla_VillaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {


        public string Name { get; set; }
    }
}
