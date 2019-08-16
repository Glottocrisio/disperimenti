using *******.Models.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using *******.Models.DbAccess;

namespace *******Controller.Models
{
    
    public class VMreport
    {

            public VMBetweenDatesFilter DayDate { get; set; }
            public VMTextfieldFilter CourseCode { get; set; }
        public Dictionary<string, DataSet> data { get; set; }
        public string LastAction { get; set; }
        public string role { get; set; }
        
            public string isPush { get; set; }


            public VMreport()
            {
                DayDate = new VMBetweenDatesFilter(DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek), DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek + 27));
                CourseCode = new VMTextfieldFilter("Course code");
                data = new Dictionary<string, DataSet>();
                isPush = "false";
                role = "";
            }


        public void getListOfCourses()
        {
            data.Add("CoursesList", DBAccess.getCourses(DayDate.DayDateStart.ToString("yyyy-MM-dd"), DayDate.DayDateEnd.ToString("yyyy-MM-dd"), CourseCode,role));
        }

        public void getListOfParticipants(int id)
        {
            data.Add("Course Info", DBAccess.courseInfo(id));
            data.Add("ParticipantsList", DBAccess.getParticipantList(id, role));
        }

        public bool hasFeedbackForm(string id)
        {
            return DBAccess.hasFeedbackForm(id);
        }

        public string buildMailBody(int id)
        {

            System.Data.DataTable result = data["ParticipantsList"].Tables[0];

      
          string  mailBody = "<table id=\"mainTable\"  style= \"font-family: Arial, Helvetica, sans-serif; border: 2px, solid, black;\"> "
            + " <thead>"
            + " <tr>"
            + "<th>First name</th>"
            + " <th>Last name</th>"
            + "<th>Job title</th>"
            + "<th>E-mail</th>"
            + "<th>Sector</th>"
            + "<th>status</th>"
            + "<th class=\"dateFormat-ddmmyyyy\">End date</th>"
            + " </tr>"
            + "</thead>";

            foreach (System.Data.DataRow row in result.Rows)
            {
              
                mailBody += "<tr>"
     
        /*mailBody= mailBody+ ...*/         + " <td>" + Convert.ToString(row["First name"]) + "</td>"
                  + "<td>" + Convert.ToString(row["Last name"]) + "</td>"
                  + "<td>" + Convert.ToString(row["Job title"]) + "</td>"



                     + "<td class=\"reportHelp\" title=\""+ Convert.ToString(row["E-mail"])+ "\">"+Convert.ToString(row["E-mail"]) +"</td> ";


             mailBody += "<td>" + Convert.ToString(row["Sector"]) + "</td>";

                switch (Convert.ToString(row["status"]))
                {

                    case "Passed":
                        mailBody += " <td class=\"reportHelp\" style=\"color: #99CC00;\" title=\"" + Convert.ToString(row["status"]) + "\">" + Convert.ToString(row["status"]) + "</td>";
                        break;

                    case "Started":
                        mailBody += "  <td class=\"reportHelp\" style=\"color: #FFCC00;\"  title=\"" + Convert.ToString(row["status"]) + "\">" + Convert.ToString(row["status"]) + "</td> ";
                        break;
                   
                    case "Registered":
                        mailBody += " <td class=\"reportHelp\" style=\"color: #FF9933;\"  title=\"" + Convert.ToString(row["status"]) + "\">" + Convert.ToString(row["status"]) + "</td>";
                        break;
                    case "Failed":
                    case "Overdue":
                    default:
                        mailBody += " <td class=\"reportHelp\" style=\"color: #CC0033;\" title=\"" + Convert.ToString(row["status"]) + "\">" + Convert.ToString(row["status"]) + "</td>";
                        break;
                }



                mailBody += " <td>" + ((row["Enddate"] is DBNull) ? "" : Convert.ToDateTime(row["Enddate"]).ToString("dd/MM/yyyy")) + "</td>";
                mailBody += " </tr>";
             
               

               
            }
            mailBody += " </table>";
         
             return mailBody;
        }
    }
}
