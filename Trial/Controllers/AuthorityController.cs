using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trial.Models;

namespace Trial.Controllers
{
    public class AuthorityController : Controller
    {
        // GET: Authority
        public ActionResult Authority()
        {
            user curUser = (user)Session["User"];
            if (curUser.position.Contains("Employer"))
            {
                /*Move to area for Employer*/
                return Redirect("~/Employer/HomeEmployer/EmployerFirstPage");
            }
            else if (curUser.position.Contains("Applicant"))
            {
                /*Move to area for Applicant*/
                return Redirect("~/Applicant/HomeApplicant/ApplicantFirstPage");
            }
            return Redirect("~/Admin/HomeAdmin/Index");
        }
    }
}