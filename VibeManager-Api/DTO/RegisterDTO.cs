using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VibeManager_Api.DTO
{
    public class RegisterDTO
    {
        public string fullname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int id_rol { get; set; }
    }

}