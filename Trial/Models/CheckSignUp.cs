using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trial.Models
{
    public class CheckSignUp
    {
        public string email { get; set; }
        public string userfname { get; set; }
        public string gender { get; set; }
        public string position { get; set; }
        public string password { get; set; }
        public string confirmPSW { get; set; }
        public bool isAgreePolicy { get; set; }
    }
}