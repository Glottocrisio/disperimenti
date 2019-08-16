using *******.Models.DbAccess;
using *******.Models.Filters;
using *******.Models;
using *******.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace *******.Models
{
   
        public class DBAccess
        {

            public VMTextfieldFilter CourseCode { get; set; }


            public DBAccess()
            {

                CourseCode = new VMTextfieldFilter("Course code");
                /*data = new Dictionary<string, DataSet>();
                 public Dictionary<string, DataSet> data { get; set; }
            public VMBetweenDatesFilter DayDate { get; set; }
             DayDate = new VMBetweenDatesFilter(DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek), DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek + 27));*/
            }

            public static DataSet h ()
            {

                DbConnection con = new DbConnection();

                User currentUser = (User)HttpContext.Current.Session["User"];

                string request = "select distinct name,  startdate, enddate from e_component where organizer_id= *** and is_template = 0 and course_status_id <> 3 and archive = 0 ";
            }
        }
    }

