using CustomsBasis.Models.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CustomsBasis.Models.DbAccess;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Data.OleDb;
using CustomsBasis.Models;

namespace *******.Models
{


    public class ExaminerOverview
    {
        public int personid { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Examiner { get; set; }
        public string Country { get; set; }
        public string Organization { get; set; }
        public string is*******admin { get; set; }
        public string is*******paper1 { get; set; }
        public string is*******paper2ope { get; set; }
        public string is*******paper2ele { get; set; }
        public string is*******paper3ope { get; set; }
        public string is*******paper3ele { get; set; }
        public string maxACRenddate { get; set; }
        public string maxREFenddate { get; set; }
        public string maxL6Eenddate { get; set; }
    }

    public class commenti
    {
        public string id { get; set; }
        public string comment { get; set; }
        public string upname { get; set; }
        public string creationdate { get; set; }
    }


    public class *******Report
    {
        public  Dictionary<string, DataSet> data { get; set; }
        public List<ExaminerOverview> Results { get; set; }
        public List<ExaminerOverview> Resultsmall { get; set; }
        public Dictionary<string, string> examinerid { get; set; }
        public List<ExaminerOverview> comparee { get; set; }
        public List<commenti> comments { get; set; }
        public User user { get; set; }
        public int userstak { get; set; }




        public *******Report()
        {
            data = new Dictionary<string, DataSet>();
            examinerid = new Dictionary<string, string>();
            Results = new List<ExaminerOverview>();
            user = (User)HttpContext.Current.Session["User"];
            userstak = Convert.ToInt32(*******DB.examinerinfo(user.person_id).Tables[0].Rows[0]["Stakeid"]);
        }

        public void getfirsttable()
        {
            if (user.isFoPOf*******LicencedOrg().Count > 1)
            {
                data.Add("Firsttable", *******DB.firsttable(user.person_id));
            }
            else   {
            
            data.Add("Firsttable", *******DB.firsttable());
            }
            //data.Add("isfopof*******org", *******DB.fopof*******org(user.person_id));
            examinerid = *******DB.getpeople();
            Results = *******DB.getExaminersbig();

        }

        public void getExaminersList(int id)
        {
            data.Add("orginfo", *******DB.orgInfo(id));
            data.Add("Secondtable", *******DB.getExaminers(id));
            Resultsmall = *******DB.getExaminersmall(id);

        }



        public void getcoc(int id)
        {
            data.Add("cardinfo", *******DB.examinerinfo(id));
            data.Add("courseinfo", *******DB.courses(id));
            data.Add("roles", *******DB.getroles(id));
            //data.Add("certificate", *******DB.getCertificate(id));
            //data.Add("name", *******DB.personname(id));
            Results = *******DB.getExaminersbig();
            comments = *******DB.displayComments(id);
        }

    
       

    }
}