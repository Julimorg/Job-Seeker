using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trial.Models;

namespace Trial.Areas.Applicant.Controllers
{
    public class HomeApplicantController : Controller
    {
        // GET: Applicant/HomeApplicant
        dbFinalTermDataContext db = new dbFinalTermDataContext();

        //Home
        public ActionResult ApplicantFirstPage()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        //Button find job by CV --> View ChooseYourCVList --> Choose --> call JDlist(?id) --> call FindByMyCV(?id)
        //--> new position; new location --> JDlist(?position, ?location) --> new Job List 

        //Joblist
        public ActionResult JDlist(int? size, int? page, int id = 1, string position = "", string location = "", bool useFindJob = false)
        {
            
            if(useFindJob == true) { FindByMyCV(id, ref position, ref location); }
            position = position.Trim();
            ViewBag.pos = position;
            ViewBag.locate = location;
            System.Diagnostics.Debug.WriteLine(position + " " + location);
            var listJD = (from JD in db.JDs
                         where (JD.location.Contains(location) && JD.isOffered == 0
                         && JD.status.Contains("Approved")) && (JD.company_name.Contains(position) || JD.hired_position.Contains(position))
                         select JD).OrderByDescending(model => model.date_upload);
            int pageSize = (size ?? 8);
            int pageNum = page ?? 1;
            return View(listJD.ToPagedList(pageNum, pageSize));
        }

        //JobDetail
        public ActionResult DetailJob(int id)
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


        //CVlist<Choose/feature>
        public void FindByMyCV(int id, ref string position, ref string location)
        {
            System.Diagnostics.Debug.WriteLine(id);
            user cur_user = (user)Session["User"];
            var suitable_CV = (from CV in db.CVs
                               where CV.CV_id == id
                               select new
                               {
                                   CV.applied_position,
                                   CV.location
                               }).ToList();
            foreach (var item in suitable_CV)
            {
                position = item.applied_position;
                location = item.location;
                System.Diagnostics.Debug.WriteLine(position);
                System.Diagnostics.Debug.WriteLine(location);
            }
        }

        //CVlist<Choose/view>
        public ActionResult ChooseYourCVList(int? size, int? page, string position = "", string location = "")
        {
            ViewBag.pos = position;
            ViewBag.locate = location;
            user cur_user = (user)Session["User"];
            var list = (from CV_manager in db.CV_managers
                        join CV in db.CVs
                        on CV_manager.CV_id equals CV.CV_id
                        where (CV_manager.user_id == cur_user.user_id && CV.status.Contains("Approved") 
                        && CV.location.Contains(location)) && (CV.applied_position.Contains(position) || CV.CV_name.Contains(position))
                        select new YourCV{ 
                            CVid =  CV.CV_id,
                            CVname = CV.CV_name,
                            CVpos = CV.applied_position,
                            dateUpload = CV.date_upload.ToShortDateString(),
                            CVstatus = CV.status,
                            CVlocation = CV.location
                        }).ToList().OrderByDescending(model => model.dateUpload);
            int pageSize = (size ?? 8);
            int pageNum = page ?? 1;
            return View(list.ToPagedList(pageNum, pageSize));
        }



        //YourCVlist
        public ActionResult YourCVList(int? size, int? page, string position = "", string location = "")
        {
            ViewBag.pos = position;
            ViewBag.locate = location;
            user cur_user = (user)Session["User"];
            var list = (from CV_manager in db.CV_managers
                        join CV in db.CVs
                        on CV_manager.CV_id equals CV.CV_id
                        where (CV_manager.user_id == cur_user.user_id
                        && CV.location.Contains(location)) && (CV.applied_position.Contains(position) || CV.CV_name.Contains(position))
                        select new YourCV{ 
                            CVid =  CV.CV_id,
                            CVname = CV.CV_name,
                            CVpos = CV.applied_position,
                            dateUpload = CV.date_upload.ToShortDateString(),
                            CVstatus = CV.status,
                            CVlocation = CV.location
                        }).ToList().OrderByDescending(model => model.dateUpload);
            int pageSize = (size ?? 8);
            int pageNum = page ?? 1;
            return View(list.ToPagedList(pageNum, pageSize));
        }

        //CVdetail
        public ActionResult Detail(int id) {
            var CV = db.CVs.Where(model => model.CV_id == id).First();
            return View(CV);
        }

        public ActionResult Delete(int id)
        {
            var CV = db.CVs.Where(model => model.CV_id == id).First();
            var CVmanage = db.CV_managers.Where(model => model.CV_id == id).First();
            db.CV_managers.DeleteOnSubmit(CVmanage);
            db.SubmitChanges();
            db.CVs.DeleteOnSubmit(CV);
            db.SubmitChanges();
            return RedirectToAction("YourCVList");
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

        //CVformEdit (reuse the view of add)
        public ActionResult Edit(int id)
        {
            var cur_CV = db.CVs.First(CV => CV.CV_id == id);
            return View(cur_CV);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            var cur_CV = db.CVs.First(CV => CV.CV_id == id);
            var CV_nameChanged = form["CV_name"];
            var applicant_nameChanged = form["applicant_name"];
            var locationChanged = form["location"];
            var applied_positionChanged = form["applied_position"];
            var CV_fileChanged = form["CV_file"];

            cur_CV.CV_name = CV_nameChanged;
            cur_CV.applicant_name = applicant_nameChanged;
            cur_CV.location = locationChanged;
            cur_CV.applied_position = applied_positionChanged;
            cur_CV.CV_file = CV_fileChanged;
            cur_CV.date_upload = DateTime.Now;
            cur_CV.status = "Wait for justify";
            UpdateModel(cur_CV);
            db.SubmitChanges();
            return RedirectToAction("YourCVList");
        }


        //CVform
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form, CV cur_CV)
        {
            var CV_nameChanged = form["CV_name"];
            var applicant_nameChanged = form["applicant_name"];
            var locationChanged = form["location"];
            var applied_positionChanged = form["applied_position"];
            var CV_fileChanged = form["CV_file"];

            cur_CV.CV_name = CV_nameChanged;
            cur_CV.applicant_name = applicant_nameChanged;
            cur_CV.location = locationChanged;
            cur_CV.applied_position = applied_positionChanged;
            cur_CV.CV_file = CV_fileChanged;
            cur_CV.date_upload = DateTime.Now;
            cur_CV.status = "Wait for justify";
            db.CVs.InsertOnSubmit(cur_CV);
            db.SubmitChanges();
            return UpdateManageCV();
        }
        public ActionResult UpdateManageCV()
        {
            var model = db.CVs.OrderByDescending(s => s.CV_id).FirstOrDefault();
            CV_manager cV_Manager = new CV_manager();
            cV_Manager.CV_id = model.CV_id;
            user curUser = (user)Session["User"];
            cV_Manager.user_id = curUser.user_id;
            db.CV_managers.InsertOnSubmit(cV_Manager);
            db.SubmitChanges();
            return RedirectToAction("YourCVList");
        }
    }
}