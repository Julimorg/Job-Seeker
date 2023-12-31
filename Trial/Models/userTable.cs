using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trial.Models
{
    public class userTable
    {
        public int id { get; set; }
        public string email { get; set; }
        public string full_name { get; set; }
        public string position { get; set; }

        public int is_blocked { get; set; }
    }
}