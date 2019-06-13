using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.API.Login
{
    public class job
    {
        public string name { get; set; }
        public List<School> schools { get; set; }
        public List<AccessLevels> accesseLevels { get; set; }
    }
}