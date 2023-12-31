using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Linq;
using Trial.App_Start;
using Trial.Models;

namespace Trial.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin

        dbFinalTermDataContext db = new dbFinalTermDataContext();
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                System.Diagnostics.Debug.WriteLine("Is null");
                return RedirectToAction("FirstPage","HomePage");
            }
            return View();
        }




        /*List of users*/
        [AdminAuthorize(idFeature = 1)]
        public ActionResult ListOfUsers(int? size, int? page, string account = "", string position = "", int isBlocked = 0)
        {
            ViewBag.pos = position;
            var users_list = from user in db.users
                             where user.email.Contains(account) && user.position.Contains(position) && !user.email.Contains("admin")
                             && user.isBlocked.Equals(isBlocked)
                             select new userTable
                             { 
                                 id = user.user_id,
                                 email = user.email,
                                 full_name = user.user_fname,
                                 position = user.position,
                                 is_blocked = (int)user.isBlocked
                             };
            int pageSize = (size ?? 5);
            int pageNum = page ?? 1;
            return View(users_list.ToPagedList(pageNum, pageSize));
        }



        [AdminAuthorize(idFeature = 3)]
        public ActionResult User_Detail(int id, string position) 
        {
            int numOfProj = 0;
            if (position.Contains("Applicant")) {
                numOfProj = (from users in db.users
                            join CV_manager in db.CV_managers
                            on users.user_id equals CV_manager.user_id
                            where users.user_id == id
                            join CV in db.CVs
                            on CV_manager.CV_id equals CV.CV_id
                            select CV).Count();

            }
            else
            {
                numOfProj = (from users in db.users
                            join JD_manager in db.JD_managers
                            on users.user_id equals JD_manager.user_id
                             where users.user_id == id
                             join JD in db.JDs
                            on JD_manager.JD_id equals JD.JD_id
                            select JD).Count();
            }
            ViewBag.numProj = numOfProj;
            var user = db.users.Where(model => model.user_id == id).FirstOrDefault();
            return View(user);
        }


        [AdminAuthorize(idFeature = 5)]
        public ActionResult Block_UnBlock(int id)
        {
            
            var blocked_user = db.users.First(user => user.user_id == id);
            string mailForNoti = System.IO.File.ReadAllText(Server.MapPath("~/Content/mail/mail.html"));
            string mailForNoti2 = System.IO.File.ReadAllText(Server.MapPath("~/Content/mail/mail2.html"));
            //Hasnt been blocked
            if (blocked_user.isBlocked == 0)
            {
                blocked_user.isBlocked = 1;
                Trial.Common.Common.SendMail("JOB SEEKER", "USER #" + blocked_user.user_id.ToString(), mailForNoti, blocked_user.email);

            }
            //Has been blocked
            else if (blocked_user.isBlocked == 1)
            {
                blocked_user.isBlocked = 0;
                Trial.Common.Common.SendMail("JOB SEEKER", "USER #" + blocked_user.user_id.ToString(), mailForNoti2, blocked_user.email);
            }
            UpdateModel(blocked_user);
            db.SubmitChanges();
            return RedirectToAction("ListOfUsers", new { @isBlocked = 1 });
        }



        [AdminAuthorize(idFeature = 2)]
        /*List of post*/
        public ActionResult ListOfPosts(int? size, int? page, string category = "", string account = "",  string status = "Wait for justify")
        {

            var CVposts_list = (from CV_manager in db.CV_managers
                                join user in db.users
                                on CV_manager.user_id equals user.user_id
                                where user.email.Contains(account)
                                join CV in db.CVs
                                on CV_manager.CV_id equals CV.CV_id
                                where CV.status.Contains(status)
                                select new AdminListPosts
                                {
                                    id = CV.CV_id,
                                    email = user.email,
                                    name = CV.applicant_name,
                                    category = "CV",
                                    upload_date = CV.date_upload.ToShortDateString(),
                                    status = CV.status,
                                }).ToList().OrderByDescending(model => model.upload_date);
/*            System.Diagnostics.Debug.WriteLine("items: " + CVposts_list.Count);
            foreach (var item in CVposts_list)
            {
                System.Diagnostics.Debug.WriteLine("item: " + item);
            }*/


            var JDposts_list = (from JD_manager in db.JD_managers
                                join user in db.users
                                on JD_manager.user_id equals user.user_id
                                where user.email.Contains(account)
                                join JD in db.JDs
                                on JD_manager.JD_id equals JD.JD_id
                                where JD.status.Contains(status)
                                select new AdminListPosts
                                {
                                    id = JD.JD_id,
                                    email = user.email,
                                    name = JD.company_name,
                                    category = "JD",
                                    upload_date = JD.date_upload.ToShortDateString(),
                                    status = JD.status,
                                }).ToList().OrderByDescending(model => model.upload_date);
            /*            System.Diagnostics.Debug.WriteLine("items2: " + JDposts_list.Count);
                        foreach (var item in JDposts_list)
                        {
                            System.Diagnostics.Debug.WriteLine("item2: " + item);
                        }*/
            ViewBag.mode = category;
            int pageSize = (size ?? 6);
            int pageNum = page ?? 1;
            if (category.Equals("Curriculum Vitae"))
            {
                /*ViewBag.mode = "Curriculum Vitae";*/
                return View(CVposts_list.ToPagedList(pageNum, pageSize));
            }
            else if (category.Equals("Job Description"))
            {
                /*ViewBag.mode = "Job Description";*/
                return View(JDposts_list.ToPagedList(pageNum, pageSize));
            }
            /*ViewBag.mode = "";*/
            var res = CVposts_list.Union(JDposts_list).ToList();
            return View(res.ToPagedList(pageNum, pageSize));
        }


        [AdminAuthorize(idFeature = 4)]

        public ActionResult Post_Detail_CV(int id)
        {
            var CV = db.CVs.Where(model => model.CV_id == id).First();
            return View(CV);
                
        }



        [AdminAuthorize(idFeature = 4)]
        public ActionResult Post_Detail_JD(int id)
        {
            var JD = db.JDs.Where(model => model.JD_id == id).First();
            return View(JD);

        }




        public ActionResult GetPDF(string directory)
        {
            System.Diagnostics.Debug.WriteLine(directory);
            string filepath = "~" + directory;
            return File(filepath, "application/pdf");
        }



        [AdminAuthorize(idFeature = 6)]
        public ActionResult Post_Justify(int id, string category, string status)
        {
            System.Diagnostics.Debug.WriteLine(status);
            if (category.Equals("CV")) {
                var postCV = db.CVs.First(CV => CV.CV_id == id);
                postCV.status = status;
                UpdateModel(postCV);
                db.SubmitChanges();
                if (status.Contains("Approved")) {
                    return RedirectToAction("ListOfPosts", new { @status = "Approved" });
                }
                else
                {
                    return RedirectToAction("ListOfPosts", new { @status = "Rejected" });
                }
                
            }
            else
            {
                var postJD = db.JDs.First(JD => JD.JD_id == id);
                postJD.status = status;
                UpdateModel(postJD);
                db.SubmitChanges();
                if (status.Contains("Approved"))
                {
                    return RedirectToAction("ListOfPosts", new { @status = "Approved" });
                }
                else
                {
                    return RedirectToAction("ListOfPosts", new { @status = "Rejected" });
                }
            }
        }


        public ActionResult AccessDenied()
        {
            return View();
        }

        [AdminAuthorize(idFeature = 7)]
        public ActionResult SendMail(string email)
        {
            ViewBag.Email = email;  
            return View();
        }
        
        
        [HttpPost]
        public ActionResult SendMail(FormCollection form)
        {
            string toEmailChanged = form["toEmail"];
            string subjectChanged = form["subject"];
            string titleChanged = form["title"];
            string contentChanged = form["content"];
            string messageChanged = form["message"];
            string mailForNoti3 = System.IO.File.ReadAllText(Server.MapPath("~/Content/mail/mail3.html"));
            mailForNoti3 = mailForNoti3.Replace("{{Title}}", titleChanged);
            mailForNoti3 = mailForNoti3.Replace("{{Content}}", contentChanged);
            mailForNoti3 = mailForNoti3.Replace("{{Message}}", messageChanged);
            Trial.Common.Common.SendMail("JOB SEEKER", subjectChanged, mailForNoti3, toEmailChanged);

            return RedirectToAction("ListOfUsers");
        }
    }
}