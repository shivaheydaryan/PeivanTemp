using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.API.Login
{
    public class LoginAPIResult
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string  Lastname { get; set; }
        public string token { get; set; }
        public List<job> jobs { get; set; }
    }
    

}