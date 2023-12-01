using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldConduit.Application.Users.DTOs
{
    internal class MinimalProfileDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ProfileImage { get; set; }
    }
}
