using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Trial.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Trial.Areas.Employer.Controllers
{
    public class HomeEmployerController : Controller
    {
        // GET: Employer/HomeEmployer
        dbFinalTermDataContext db = new dbFinalTermDataContext();



        public ActionResult EmployerFirstPage()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


        public ActionResult CVlist(int? size, int? page, int id = 1, string position = "", string location = "", bool useFindJob = false)
        {
            
            
            if (useFindJob == true) { FindByMyJD(id, ref position, ref location);}
            ViewBag.pos = position;
            ViewBag.locate = location;
            System.Diagnostics.Debug.WriteLine(position);
            var listCV = (from CV in db.CVs
                         where (CV.location.Contains(location) && CV.status.Contains("Approved")) 
                         && (CV.applied_position.Contains(position.Trim()) || CV.applicant_name.Contains(position)
                         || position.Trim().IndexOf(CV.applied_position.Trim()) >= 0)
                         select CV).OrderByDescending(model => model.date_upload);
            int pageSize = (size ?? 8);
            int pageNum = page ?? 1;
            return View(listCV.ToPagedList(pageNum, pageSize));
        }


        public ActionResult DetailCV(int id)
        {
            var CV = db.CVs.Where(model => model.CV_id == id).First();
            return View(CV);
        }

        public ActionResult GetPDF(string directory)
        {
            System.Diagnostics.Debug.WriteLine(directory);
            string filepath = "~" + directory;
            return File(filepath, "application/pdf");
        }


        public void FindByMyJD(int id, ref string position, ref string location)
        {
            user cur_user = (user)Session["User"];
            var suitable_JD = (from JD in db.JDs
                               where JD.JD_id == id 
                              select new
                              {
                                  JD.hired_position,
                                  JD.location
                              }).ToList();
            foreach (var item in suitable_JD)
            {
                position = item.hired_position;
                location = item.location;
            }
        }

        public ActionResult ChooseYourJDList(int? size, int? page, string position = "", string location = "")
        {
            user cur_user = (user)Session["User"];
            var list = (from JD_manager in db.JD_managers
                        join JD in db.JDs
                        on JD_manager.JD_id equals JD.JD_id
                        where (JD_manager.user_id == cur_user.user_id && JD.status.Contains("Approved") && JD.isOffered == 0
                        && JD.location.Contains(location)) && (JD.hired_position.Contains(position) || JD.company_name.Contains(position) || JD.JD_name.Contains(position))
                        select new YourJD
                        {
                            JDid = JD.JD_id,
                            JDname = JD.JD_name,
                            JDpos = JD.hired_position,
                            JDlocation = JD.location,
                            dateUpload = JD.date_upload.ToShortDateString(),
                            JDstatus = JD.status,
                            JDoffer = (JD.isOffered == 0)? "Opened" : "Closed",
                            JDimage = JD.company_images,
                            JDcompName = JD.company_name,
                        }).ToList().OrderByDescending(model => model.dateUpload);
            int pageSize = (size ?? 8);
            int pageNum = page ?? 1;
            return View(list.ToPagedList(pageNum, pageSize));
        }


        public ActionResult YourJDList(int? size, int? page, string position = "", string location = "")
        {
            user cur_user = (user)Session["User"];
            var list = (from JD_manager in db.JD_managers
                        join JD in db.JDs
                        on JD_manager.JD_id equals JD.JD_id
                        where (JD_manager.user_id == cur_user.user_id
                        && JD.location.Contains(location)) && (JD.hired_position.Contains(position) || JD.company_name.Contains(position) || JD.JD_name.Contains(position))
                        select new YourJD
                        {
                            JDid = JD.JD_id,
                            JDname = JD.JD_name,
                            JDpos = JD.hired_position,
                            JDlocation = JD.location,
                            dateUpload = JD.date_upload.ToShortDateString(),
                            JDstatus = JD.status,
                            JDoffer = (JD.isOffered == 0)? "Opened " : "Closed",
                            JDimage = JD.company_images,
                            JDcompName= JD.company_name,
                        }).ToList().OrderByDescending(model => model.dateUpload);
            int pageSize = (size ?? 8);
            int pageNum = page ?? 1;
            return View(list.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Detail(int id)
        {
            var JD = db.JDs.Where(model => model.JD_id == id).First();
            return View(JD);
        }

        public ActionResult Delete(int id)
        {
            var JD = db.JDs.Where(model => model.JD_id == id).First();
            var JDmanage = db.JD_managers.Where(model => model.JD_id == id).First();
            db.JD_managers.DeleteOnSubmit(JDmanage);
            db.SubmitChanges();
            db.JDs.DeleteOnSubmit(JD);
            db.SubmitChanges();
            return RedirectToAction("YourJDList");
        }

        //0: if offerd;  1: is not offered
        public ActionResult isOffer(int id)
        {
            var job_offered = db.JDs.First(JD => JD.JD_id == id);
            if (job_offered.isOffered == 0)
            {
                job_offered.isOffered = 1;
            }
            else if (job_offered.isOffered == 1)
            {
                job_offered.isOffered = 0;
            }
            UpdateModel(job_offered);
            db.SubmitChanges();
            return RedirectToAction("Detail",new {@id = id});
        }


        public string ProcessUpload(HttpPostedFileBase fileImage)
        {
            System.Diagnostics.Debug.WriteLine("I'm in processUpload" + fileImage.ToString());
            if (fileImage == null)
            {
                System.Diagnostics.Debug.WriteLine("Is null");
                return " ";
            }
            /*            ViewBag.filePic = "/Content/image/" + fileImage.FileName.ToString();*/
            fileImage.SaveAs(Server.MapPath("~/Content/images/" + fileImage.FileName));
            return "/Content/images/" + fileImage.FileName;
        }


        public ActionResult Edit(int id)
        {
            var cur_JD = db.JDs.First(JD => JD.JD_id == id);
            return View(cur_JD);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            var cur_JD = db.JDs.First(JD => JD.JD_id == id);
            var JD_nameChanged = form["JD_name"];
            var company_nameChanged = form["company_name"];
            var locationChanged = form["location"];
            var hired_positionChanged = form["hired_position"];
            var position_desChanged = form["position_des"];
            var company_imagesChanged = form["company_images"];
            var JD_fileChanged = form["JD_file"];

            cur_JD.JD_name = JD_nameChanged;
            cur_JD.company_name = company_nameChanged;
            cur_JD.location = locationChanged;
            cur_JD.hired_position = hired_positionChanged;
            cur_JD.position_des = position_desChanged;
            cur_JD.company_images = company_imagesChanged;
            cur_JD.JD_file = JD_fileChanged;
            cur_JD.date_upload = DateTime.Now;
            cur_JD.status = "Wait for justify";
            cur_JD.isOffered = 0;
            UpdateModel(cur_JD);
            db.SubmitChanges();
            return RedirectToAction("YourJDList");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form, JD cur_JD)
        {
            var JD_nameChanged = form["JD_name"];
            var company_nameChanged = form["company_name"];
            var locationChanged = form["location"];
            var hired_positionChanged = form["hired_position"];
            var position_desChanged = form["position_des"];
            var company_imagesChanged = form["company_images"];
            var JD_fileChanged = form["JD_file"];

            cur_JD.JD_name = JD_nameChanged;
            cur_JD.company_name = company_nameChanged;
            cur_JD.location = locationChanged;
            cur_JD.hired_position = hired_positionChanged;
            cur_JD.position_des = position_desChanged;
            cur_JD.company_images = company_imagesChanged;
            cur_JD.JD_file = JD_fileChanged;
            cur_JD.date_upload = DateTime.Now;
            cur_JD.status = "Wait for justify";
            cur_JD.isOffered = 0;
            db.JDs.InsertOnSubmit(cur_JD);
            db.SubmitChanges();
            return UpdateManageJD();
        }

        public ActionResult UpdateManageJD()
        {
            var model = db.JDs.OrderByDescending(s => s.JD_id).FirstOrDefault();
            JD_manager jD_Manager = new JD_manager();
            jD_Manager.JD_id = model.JD_id;
            user curUser = (user)Session["User"];
            jD_Manager.user_id = curUser.user_id;
            db.JD_managers.InsertOnSubmit(jD_Manager);
            db.SubmitChanges();
            return RedirectToAction("YourJDList");
        }
    }
}