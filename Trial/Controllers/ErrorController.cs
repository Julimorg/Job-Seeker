using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trial.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult IsBlocked ()
        {
            return View();
        }

    }
}