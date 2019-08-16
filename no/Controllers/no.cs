using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using *******.Models.Util;
using *******.Models;
using *******.Filters;
using *******.Models;
using *******.Models.DbAccess;

namespace *******.Controllers
{
    
    [CoreActionFilter]
    public class *******Controller : Controller
    {



       public *******Report report = new *******Report();
            
        
        
            public ActionResult Index()  {
              
             
                report.populateDropDownList();
               
                return View(report);
            }


            [AcceptVerbs(HttpVerbs.Post)]
            public JsonResult LoadCourses(string SelectedYear)
            {
                
                var courselist = *******DBAccess.getCourseList(SelectedYear);


                //var coursess = courselist.Select(m => new SelectListItem()
                //{
                //    Text = m.Value,

                //});
                return Json(courselist, JsonRequestBehavior.DenyGet);
            }

        [HttpPost]
            public ActionResult Search(*******Report bindedReport)
        {
            ViewBag.courseId = bindedReport.dropdown.selectedValue;
            
             bindedReport.populateDropDownList();
            bindedReport.getListOfCoursesWE(ViewBag.courseId);
           
            return View("~/Views/*******/Index.cshtml", bindedReport);
        }

        
        


    }
      

    }
