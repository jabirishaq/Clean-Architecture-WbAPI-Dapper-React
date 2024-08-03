using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public DateTime? LoginTime { get; set; }
        public decimal Balance { get; set; }
    }
}
