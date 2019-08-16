using *******.Models;
using *******.Models.Util;
using *******Controller.Filters;
using *******Controller.Models;
using SelectPdf;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;




namespace *******Controller.Controllers
{
    [*******ActionFilter]
    public class *******Controller : Controller
    {
        public VMreport report = new VMreport();


        public ActionResult Index()
        {
            report.getListOfCourses();
            return View(report);
        }

        [HttpPost]
        public ActionResult Search(VMreport bindedReport)
        {
            bindedReport.getListOfCourses();
            
            return View("~/Views/*******/Index.cshtml", bindedReport);
        }



        
        public ActionResult ListOfParticipants(int  id)
        {
            ViewBag.Title = "******* Courses' Progress";
            ViewBag.courseId = id;
            report.getListOfParticipants(id);
           
            return View(report);
        }

        public ActionResult ExportListOfParticipants(int id)
        {
            // Send an email with 
            // subject = ******* Report
            // body = this is my report body for the course *component_id*
            // sender = trainingzone.support@*******.int
            // recipient = cosimo.palma.ext@*******.int
            User currentUser = (User)Session["User"];
            report.getListOfParticipants(id);
           
           MailSender.sendMail(currentUser.email, "******* Report", report.buildMailBody(id), "cosimo.palma.ext@*******.int", null, false);
            return null;
            //return Json( new[] {"cosimo.palma.ext@*******.int", "******* Report", "this is my report body for the course" +id, "trainingzone.support@*******.int" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Reload()
        {
          
            report.getListOfCourses();
            return View("~/Views/*******/Index.cshtml", report);
        }





        public string RenderViewAsString(string viewName, object model)
        {
            // create a string writer to receive the HTML code
            StringWriter stringWriter = new StringWriter();

            // get the view to render
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext,
                      viewName, null);
            // create a context to render a view based on a model
            ViewContext viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                new ViewDataDictionary(model),
                new TempDataDictionary(),
                stringWriter
            );

            // render the view to a HTML code
            viewResult.View.Render(viewContext, stringWriter);

            // return the HTML code
            return stringWriter.ToString();
        }

        [HttpGet]
        public ActionResult ConvertHtmlPageToPdf(string token, string caller, string role = "")
        {
            // get the HTML code of this view

            string validation = SecurityStringBuilderHelper.SecurityStringBuilder("*******CourseProgress", caller);
            if (validation == token)
            {
                report.isPush = "true";
                report.role = role;
                report.getListOfCourses();
                ViewBag.mediatype = "print";
                return View("~/Views/*******/Index.cshtml", report);

         

            }
            else {
                //Response.Clear();
                //Response.Flush();
                  return RedirectToAction("Error","Core");
            }
          
        }
 
        [HttpGet]
        public ActionResult PushReportingEndedCourse(string token, string caller, string role = "", string courseid = "")
        {
            // get the HTML code of this view

            string validation = SecurityStringBuilderHelper.SecurityStringBuilder("*******CourseProgress", caller);
            if (validation == token)
            {
                report.isPush = "true";
                report.role = role;
                ViewBag.courseId = courseid;
                report.getListOfParticipants(Convert.ToInt32(courseid));
                 return View("~/Views/*******/ListOfParticipants.cshtml", report);
            }
            else {
                  return RedirectToAction("Error","Core");
            }
        }

      
    }
}