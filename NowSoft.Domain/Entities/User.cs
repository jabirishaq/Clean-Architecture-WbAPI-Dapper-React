using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public DateTime? LoginTime { get; set; }
        public decimal Balance { get; set; }
    }
}
