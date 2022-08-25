using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName  => string.Concat(FirstName, " ", LastName);
        public string PartName => string.Concat(FirstName, " ", LastName.Substring(0,1), ".");
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public Details FavoriteAnime { get; set; }
        public string Bio { get; set; }
        public string Avatar { get; set; } = "https://www.looper.com/img/gallery/the-entire-avatar-the-last-airbender-timeline-explained/intro-1579265101.jpg";
       
        public string Password { get; set; }
        public bool IsPrivate { get; set; } = false;
        public bool ShowFullName { get; set; } = false;
        public DateTime DateOfBirth { get; set; }

        public DateTime Joined { get; set; } = DateTime.Now;

        public List<Users> Followers { get; set; } = new List<Users>();
    }
}
