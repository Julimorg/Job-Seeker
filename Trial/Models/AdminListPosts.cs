using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trial.Models
{
    public class AdminListPosts
    {
        public int id {  get; set; }
        public string email {  get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string upload_date { get; set; }

        public string status { get; set; }
    }
}