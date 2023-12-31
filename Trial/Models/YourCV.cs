using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trial.Models
{
    public class YourCV
    {
        public int CVid {  get; set; }
        public string CVname { get; set; }
        public string CVpos { get; set; }
        public string dateUpload { get; set; }
        public string CVstatus { get; set; }
        public string CVlocation { get; set; }

    }
}