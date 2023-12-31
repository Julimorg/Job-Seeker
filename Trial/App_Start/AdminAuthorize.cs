using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Cryptography.X509Certificates;
using Trial.Models;

namespace Trial.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        dbFinalTermDataContext db = new dbFinalTermDataContext();
        public int idFeature { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            user user = (user)HttpContext.Current.Session["User"];
            if (user != null)
            {
                #region
                var count = db.authorities.Count(m => m.user_id == user.user_id && m.canDo_id == idFeature);
                if (count != 0)
                {
                    return;
                }
                else
                {
                    var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "HomeAdmin",
                            action = "AccessDenied",
                            area = "Admin",
                            returnUrl = returnUrl.ToString()
                        }));
                }
                #endregion
                return;
            }
            else
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "User",
                        action = "Login_SignUp",
                        returnUrl = returnUrl.ToString()
                    }));
            }

        }
    }
}