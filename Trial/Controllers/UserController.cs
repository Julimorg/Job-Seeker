using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Trial.Models;

namespace Trial.Controllers
{
    
    public class UserController : Controller
    {
        // GET: User
        
        dbFinalTermDataContext db = new dbFinalTermDataContext();

        public ActionResult Login_SignUp()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CheckValidNew(CheckSignUp model)
        {
            System.Diagnostics.Debug.WriteLine("I'm in");
            bool isWrong = false;
            string types_str = "";
            string messages_str = "";
            var check = db.users.Where(i => i.email == model.email).FirstOrDefault();
            if (check != null)
            {
                System.Diagnostics.Debug.WriteLine("This email has been registered");
                isWrong = true;
                types_str += "1" + " ";
                messages_str += "This email has been registered\n";
            }
            if (model.confirmPSW == null)
            {
                System.Diagnostics.Debug.WriteLine("Confirmation must not be null");
                isWrong = true;
                types_str += "5" + " ";
                messages_str += "Confirmation must not be null\n";
            }
            else if (!model.password.Equals(model.confirmPSW))
            {
                System.Diagnostics.Debug.WriteLine("Password and confirmation password must be the same");
                isWrong = true;
                types_str += "2" + " ";
                messages_str += "Password and confirmation password must be the same\n";
            }
            if (model.isAgreePolicy == false)
            {
                System.Diagnostics.Debug.WriteLine("It seems like you haven't checked our Privacy Policy");
                isWrong = true;
                types_str += "3" + " ";
                messages_str += "It seems like you haven't checked our Privacy Policy\n";

            }
            if (!model.email.Contains("@gmail.com"))
            {
                System.Diagnostics.Debug.WriteLine("Your email is not valid");
                isWrong = true;
                types_str += "4" + " ";
                messages_str += "Your email is not valid\n";
            }
            if (isWrong)
            {
                return Json(new
                {
                    success = false,
                    type = types_str,
                    message = messages_str,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return SignUp(model, "", "");
            }
        }

        public ActionResult SignUp(CheckSignUp newAccount, string Email, string Position)
        {
            user model = new user();
            if (!Email.IsNullOrWhiteSpace())
            {
                model.email = Email;
                model.position = Position;
                model.isBlocked = 0;
            }
            else
            {
                model.email = newAccount.email;
                model.user_fname = newAccount.userfname;
                model.gender = newAccount.gender;
                model.position = newAccount.position;
                model.password = newAccount.password;
                model.isBlocked = 0;
            }
            db.users.InsertOnSubmit(model);
            db.SubmitChanges();
            if (!Email.IsNullOrWhiteSpace())
            {
                System.Diagnostics.Debug.WriteLine("Is null");
                Session["User"] = model;
                return RedirectToAction("Authority", "Authority");
            }
            Session["User"] = model;
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckValid(CheckLogin model)
        {
            System.Diagnostics.Debug.WriteLine("I'm in");
            user user_mo = db.users.Where(i => i.email == model.email && i.password == model.password).FirstOrDefault();
            if (user_mo != null)
            {
                
                System.Diagnostics.Debug.WriteLine("C# " + user_mo.email + " " + user_mo.password);
                if (user_mo.isBlocked == 1)
                {
                    return Json(new { success = false, type = "Blocked" }, JsonRequestBehavior.AllowGet);
                }
                return Login(user_mo, "");
                /*TempData["model"]  = user_mo;*/
                /*return RedirectToAction("Login");*/
                /*return Json(new { success = true }, JsonRequestBehavior.AllowGet);*/
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("C# Is Null");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Login(user user_mo,string Email /*string password*/)
        {
            System.Diagnostics.Debug.WriteLine("I'm in Login");
            //GG login
            if (!Email.IsNullOrWhiteSpace())
            {
                user GG_user = db.users.Where(i => i.email == Email).FirstOrDefault();
                if (GG_user.isBlocked == 1)
                {
                    System.Diagnostics.Debug.WriteLine("You're blocked");
                    return RedirectToAction("IsBlocked", "Error");
                }
                Session["User"] = GG_user;
                return RedirectToAction("Authority", "Authority");
            }
            /*user user_mo = db.users.Where(i => i.email == email && i.password == password).First();*/
            /*user user_mo = (user)TempData["model"];*/
            Session["User"] = user_mo;
            
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            Session.Remove("User");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login_SignUp", "User");
        }
    }
}