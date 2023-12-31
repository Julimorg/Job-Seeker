using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trial.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult FirstPage()
        {
            return View();

        }
        public ActionResult HomePage()
        {
            return View();
            /* if (Session["User"] == null)
             {
                 System.Diagnostics.Debug.WriteLine("Is null");
                 return RedirectToAction("FirstPage");
             }
             else
             {
                 System.Diagnostics.Debug.WriteLine("Is not null");
                 return View();
             }*/
        }
    }
}