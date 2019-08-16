using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using *******.Models;
using *******.Filters;
using *******.Models;
using System.Text;
using *******.Models.Util;
using System.IO;
using System.Web.UI;
using System.Web.Hosting;
using System.Data.SqlClient;
using System.Data;
using *******.Models.DbAccess;
using *******.Filters;




namespace *******.Controllers

{   
    
    [*******ActionFilter]
    public class *******Controller : Controller
    {
        public *******Report report = new *******Report();


        public ActionResult Index()
        {
            User tempUser = Session["User"] as User;


               report.getfirsttable();
               return View("~/Views/*******/Index.cshtml", report);
                  
        }
    
       

        [HttpGet]
        public ActionResult Personrole(int id)
        {
            User tempUser = Session["User"] as User;
            if (tempUser.isSysAdmin() || tempUser.is*******Admin()|| tempUser.isFoPOf*******LicencedOrg().Contains(id))
            {
                report.getExaminersList(id);
            }
            else if (tempUser.isFoPOf*******LicencedOrg().Count > 1)
            {
                return RedirectToAction("Index");
            }
            else if (tempUser.isFoPOf*******LicencedOrg().Count == 1)
            {
                return RedirectToAction("Personrole", new { id = tempUser.isFoPOf*******LicencedOrg().FirstOrDefault().ToString() });
            }
            else
            {
                return RedirectToAction("Error");
            }

            return View("Personrole", report);
        }


        [HttpGet]
        public ActionResult fopredirect()
        {
            return View("~/Views/Shared/fopredirect.cshtml");
        }

    
        public ActionResult Certificate(int id) 
         {

            User tempUser = Session["User"] as User;

            DataSet exam = *******DB.examinerinfo(id);
            if ((tempUser.isFocalPoint() || tempUser.is*******FoP()) && !(new[] { exam.Tables[0].Rows[0]["FoPid"].ToString(), exam.Tables[0].Rows[0]["DeFoPid"].ToString() }.Contains(tempUser.person_id.ToString())))
            {
                return View("~/Views/Shared/Error.cshtml");

            }

            else
            {
                report.getcoc(id);
                return View("Certificate", report);
            }
        }

        [HttpGet]
        public ActionResult displayComments(int id)
        {
            report.comments= *******DB.displayComments(id);
            return PartialView("_Comments", report.comments );
           
        }

        [HttpPost]
        public ActionResult AutocompleteCertificate()
        {
            int id = Convert.ToInt32(Request.Form["selected-hidden"]);
            report.getcoc(id);
            
            return View("Certificate", report);
        }


        [HttpPost]
        public ActionResult Exportcsv()
        {
            Encoding encoding = Encoding.UTF8;
            string csv = Convert.ToString(Request.Form["csv"]);

            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition", "attachment;filename=*******_Examiners_Overview" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv");
            Response.Charset = encoding.EncodingName;
            Response.ContentType = "application/text";
            Response.ContentEncoding = Encoding.Unicode;
            Response.Output.Write(csv);
            Response.Flush();
            Response.End();

            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult insertComment(int id, string comment)
        {

            User tempUser = Session["User"] as User;

            *******DB.insertComment(id, comment);
            
            return null;
        }

        [HttpPost]
        public ActionResult deleteComment(int id)
        {

            User tempUser = Session["User"] as User;

            *******DB.deleteComment(id);
            return null;
        }

         [HttpPost]
        public ActionResult updateComment( string comment, string id)

             {

            User tempUser = Session["User"] as User;

            *******DB.updateComment(comment, id);
            return null;
        }




        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }


        public ActionResult RefreshData()
        {
            *******exporticao.execute();
            report.getfirsttable();

            return  View("~/Views/*******/Index.cshtml", report);
        }

        
    }
}
