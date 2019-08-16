using *******.Models.DbAccess;
using *******.Models.Filters;
using *******.Models;
using *******.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.ComponentModel.DataAnnotations;
namespace *******.Models
{
    public class *******DBAccess
    {
       
       
        public  Dictionary <string, DataSet> data { get; set; }
        public string selectedValue { get; set; }

        public string selectedYear { get; set; }
        public SelectList valuesYears { get; set; }

        public *******DBAccess()
        {
            valuesYears = new SelectList(new List<SelectListItem> {
                             new SelectListItem() {Value=(DateTime.Today.Year -1).ToString(),Text =(DateTime.Today.Year-1).ToString()},
                            new SelectListItem() {Value=DateTime.Today.Year.ToString(),Text =DateTime.Today.Year.ToString()},
                           
            }, "Value", "Text");

            data = new Dictionary<string, DataSet>();
        }
         

        public static DataSet minyearcourse()
        {
            DbConnection con = new DbConnection();
            User currentUser = (User)HttpContext.Current.Session["User"];
            List<SelectListItem> newList = new List<SelectListItem>();
            DbConnection database = new DbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataSet resultset = new DataSet();
            cmd.Connection = database.connection;


            cmd.CommandText = "select distinct   min(year(ec.startdate)) as MINyear, name as Coursename  from e_component ec   where  ec.language_id=27 and ec.course_status_id <> 3 and ec.archive = 0 and is_template = 0 and ec.organizer_id = 2822610 and name not like '*********'  and name not like '*******'  ";



         
            cmd.CommandText  += " group by name having count(name)>1 order by MINyear desc";

            

            cmd.CommandType = CommandType.Text;

            database.connection.Open();
            da.SelectCommand = cmd;
            da.Fill(resultset);
            database.connection.Close();
            newList.Add(new SelectListItem() { Value = "noselected" , Text = "Select a course" });
            
            foreach (System.Data.DataTable table in resultset.Tables)
            {
                foreach (System.Data.DataRow row in table.Rows)
                {
                  //  SelectListItem selListItem = new SelectListItem() { Value = Convert.ToString(row["MINyear"]), Text = Convert.ToString(row["MINyear"]) + " " + (String)row["Coursename"] };
                    SelectListItem selListItem = new SelectListItem() { Value = Convert.ToString(row["Coursename"]), Text = (String)row["Coursename"] };
                    newList.Add(selListItem);
                }
            }
            return resultset;
        }

        public static List<SelectListItem> getCourseList(string selectedYear)
        {
     
            User currentUser = (User)HttpContext.Current.Session["User"];
            List<SelectListItem> newList = new List<SelectListItem>();
            DbConnection database = new DbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataSet resultset = new DataSet();
            cmd.Connection = database.connection;


            cmd.CommandText = "select * from (select distinct min(year(ec.startdate)) as MINyear, RTRIM(LTRIM(CASE WHEN CHARINDEX('(',name) > 0 THEN LEFT(name, CHARINDEX('(',name)-1) ELSE name END)) as Coursename from e_component ec where ec.language_id=27 and ec.course_status_id <> 3 and ec.archive = 0 and is_template = 0 and ec.organizer_id = 2822610 and name not like 'On-the-job Training'  and name not like 'CBT - Network Operations Officer ATFCM' and ec.userdefined_id not like '%CBT%' and ec.userdefined_id not like '%CNV%' and ec.userdefined_id not like '%IND%' group by name  having count(name)>1 ) templist where templist.MINyear like ? order by templist.Coursename asc ";

            cmd.Parameters.Add("MINyear", OleDbType.VarChar, selectedYear.ToString().Length).Value = selectedYear;

            cmd.CommandType = CommandType.Text;
            resultset = database.executeQuery(cmd);

          

            newList.Add(new SelectListItem() { Value = "Please select a course", Text = " Please select a course " });

            foreach (System.Data.DataTable table in resultset.Tables)
            {
                foreach (System.Data.DataRow row in table.Rows)
                {
                    //  SelectListItem selListItem = new SelectListItem() { Value = Convert.ToString(row["MINyear"]), Text = Convert.ToString(row["MINyear"]) + " " + (String)row["Coursename"] };
                    SelectListItem selListItem = new SelectListItem() { Value = Convert.ToString(row["coursename"]), Text = (String)row["coursename"] };
                    newList.Add(selListItem);
                }
            }
            return newList;
        }




        public static DataSet getendorsement(string selectedValue)
        {

            DbConnection con = new DbConnection();
            OleDbCommand cmd = new OleDbCommand();
            User currentUser = (User)HttpContext.Current.Session["User"];

          
        


       string   request = "select distinct cast(convert(varchar(10), *******.coursestartdate, 110) as date) as startdate, "
            + "cast(convert(varchar(10), *******.courseenddate, 110) as date) as enddate, "
            + "*******.componentid as course_id, *******.name as coursename, "
            + "listperson.person_id, "
            + "lastname +' '+ firstname as name, "
            + "listperson.group_name as endorsment, "
            + "CASE "
            + "WHEN *******.[status] in (8,9) AND *******.name like '%TO BE SCHEDULED%' AND *******.courseenddate > GETDATE() THEN 'To be scheduled' "
            + "WHEN *******.[status] in (8,9) AND *******.name like '%TO BE SCHEDULED%' AND *******.courseenddate < GETDATE() THEN 'Scheduling overdue' "
            + "WHEN *******.[status] in (8,9) AND *******.name not like '%TO BE SCHEDULED%' AND *******.name not like '%TEACHING TEAM%' AND *******.coursestartdate > GETDATE() THEN 'Scheduled' "
            + "WHEN *******.[status] in (8,9) AND *******.name not like '%TO BE SCHEDULED%' AND *******.name not like '%TEACHING TEAM%' AND *******.courseenddate > GETDATE() THEN 'in Training' "
            + "WHEN *******.[status] in (8,9) AND *******.name not like '%TO BE SCHEDULED%' AND *******.name not like '%TEACHING TEAM%' AND *******.courseenddate < GETDATE() THEN 'Attendance record outstanding' "
            + "WHEN *******.[status] in (10,11) AND *******.name not like '%TO BE SCHEDULED%' AND *******.name not like '%TEACHING TEAM%' AND *******.coursestartdate < GETDATE() THEN 'Attended' "
            + "WHEN *******.[status] in (10,11) AND *******.name like '%TEACHING TEAM%' THEN 'TUTOR' "
            + "WHEN *******.[status] = 7 AND *******.name not like '%TO BE SCHEDULED%' AND *******.name not like '%TEACHING TEAM%' THEN 'To be rescheduled' "
            + "WHEN *******.[status] IS NULL then 'Missing from schedule' "
            + "ELSE 'Input error' "
            + "end as [status] "
            + "from "
            + "("
                + "select distinct p.firstname, p.lastname, pgs.person_id, pgd.group_name "
                + "from platformgroupspecification pgs join "
                + "person p on p.person_id = pgs.person_id join "
                + "platformgroup pg on pg.group_id=pgs.group_id join "
                + "platformgroup_d pgd on pgd.group_id=pg.group_id "
                + "where pgs.group_id in (select group_id from platformgroup_d pfd where language_id = 27 and substring(pgd.group_name,0, charindex(' ',pgd.group_name)) in ((select distinct * from dbo.euro_Split(( select reverse(substring(reverse((select  distinct replace(edd.content+edd.content2,'+',',') + ',' from e_component ec join e_description edd on ec.component_id=edd.component_id and edd.metatag_id = 5859452 where  ec.name like ? for XML Path ('') )), 2, 999999))), ',')))) ) listperson left join ("
                + "select ec.component_id as componentid, ec.startdate as coursestartdate, ec.enddate as courseenddate, ec.name, pf.* from e_component ec  join portfolio pf on ec.component_id = pf.component_id where ec.name like ? and ec.organizer_id = 2822610) ******* "
            + "on *******.person_id = listperson.person_id ";

           

            request += "order by name asc";

            cmd.Parameters.Add("course", OleDbType.VarChar, selectedValue.ToString().Length+1).Value = selectedValue+"%";
            cmd.Parameters.Add("courses", OleDbType.VarChar, selectedValue.ToString().Length+1).Value = selectedValue+"%";

           cmd.CommandType = CommandType.Text;
           cmd.CommandText = request;

           return con.executeQuery(cmd);
        }



        public static List<SelectListItem> getYearList()
        {

            User currentUser = (User)HttpContext.Current.Session["User"];
            List<SelectListItem> newList = new List<SelectListItem>();
            DbConnection database = new DbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataSet resultset = new DataSet();
            cmd.Connection = database.connection;


                cmd.CommandText = "select distinct  min(year(ec.startdate)) as MINyear  from e_component ec   where  ec.language_id=27 and ec.course_status_id <> 3 and ec.archive = 0 and is_template = 0 and ec.organizer_id = 2822610 and name not like 'On-the-job Training'  and name not like 'CBT - Network Operations Officer ATFCM' and ec.userdefined_id not like '%CBT%' and ec.userdefined_id not like '%CNV%' and ec.userdefined_id not like '%IND%' group by name having count(name)>1  order by MINyear desc ";

        

            cmd.CommandType = CommandType.Text;

            database.connection.Open();
            da.SelectCommand = cmd;
            da.Fill(resultset);
            database.connection.Close();
           

            foreach (System.Data.DataTable table in resultset.Tables)
            {
                foreach (System.Data.DataRow row in table.Rows)
                {
               
                    SelectListItem selListItem = new SelectListItem() { Value = Convert.ToString(row["MINyear"]), Text = Convert.ToString(row["MINyear"])};
                    newList.Add(selListItem);
                }
            }
            return newList;
        }


      

    }
}