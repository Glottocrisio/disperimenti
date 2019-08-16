using *******.Models.DbAccess;
using *******.Models.Filters;
using *******.Models;
using *******.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace *******Controller.Models
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





        public static DataSet getCourses(String _startDate, String _endDate,  VMTextfieldFilter CourseCode, String role = "")
        {



            DbConnection con = new DbConnection();

            User currentUser = (User)HttpContext.Current.Session["User"];

           

            string request = "select distinct  course.component_id, name, case when course.startdate is null and isdate(right(course.userdefined_id, 10)) = 1  then convert(date, convert(date, right((course.userdefined_id), 10)), 103)  when course.startdate is null and isnumeric(right(course.userdefined_id, 4)) = 1 then convert(date, right(course.userdefined_id, 4)+'-01-01') else course.startdate end as startdate, case when course.enddate is null and isdate(right(course.userdefined_id, 10)) = 1 then (select max(convert(date, deadline_date)) from portfolio where  component_id = course.component_id)   when course.enddate is null and isnumeric(right(course.userdefined_id, 4)) = 1 then convert(date, right(course.userdefined_id, 4)+'-12-31') else course.enddate end as enddate, course.userdefined_id as coursecode, ect.time_mode_id, (select count(*)   from portfolio pfr where  pfr.component_id = course.component_id  and pfr.[status]=8 and (GETDATE() < pfr.deadline_date or pfr.deadline_date is null) ) as [Registered], (select count(*)   from portfolio pfs where  pfs.component_id = course.component_id  and pfs.[status]=9 and (GETDATE() < pfs.deadline_date or pfs.deadline_date is null)) as [Started], (select count(*)from portfolio  pfp where pfp.component_id = course.component_id  and (pfp.[status]=10 or pfp.[status]=11)) as [Passed], (select count(*) from portfolio pfo where  pfo.component_id = course.component_id  and pfo.[status] in (8,9) and pfo.deadline_date is not null and (getdate()>course.enddate or getdate()>pfo.deadline_date)) as [Overdue],  (select count(*) from portfolio where  component_id = course.component_id  and [status]=12) as [Failed] "
            + "from  e_component course left join portfolio pf  on course.component_id=pf.component_id  inner join e_componenttype ect on course.type_id = ect.componenttype_id left join e_description ed on ed.component_id=course.component_id  and metatag_id = 10068"
            + "where course.is_template = 0 and course_status_id <> 3 and course.organizer_id = 195673 and course.archive = 0  ";

            if (role == "*******CRO" || role == "ADMIN" || (role == "" && (currentUser.is*******CRO() || currentUser.isSysAdmin())))
            {
            }
            else
            {
                if (role == "TUTOR" || (role == "" && currentUser.isTutor()))
                {
                    request += " and course.component_id in (Select component_id from person_component_assignment WHERE person_id = " + currentUser.person_id + " and tutor_subrole_id in (1, 2))";
                }
                else
                {
                    request += " and  course.component_id in (Select component_id from portfolio pf WHERE pf.person_id in ( Select person_id from person p where p.superior_email = '" + currentUser.email + "' ) )";
                }
            }


            request += " and ( (ect.time_mode_id = 0 and (course.startdate BETWEEN '" + _startDate + "' AND '" + _endDate + "' or (course.enddate > '" + _startDate + "' AND course.startdate < '" + _startDate + "'))) "
            + "OR (ect.time_mode_id = 1 and isdate(right(course.userdefined_id, 10)) = 1 and (right(course.userdefined_id, 10) BETWEEN '" + _startDate + "' AND '" + _endDate + "' or ((select top(1)(deadline_date) from portfolio where component_id = course.component_id order by deadline_date desc ) > '" + _startDate + "' AND right(course.userdefined_id, 10) < '" + _startDate + "'))) "

            + " or (ect.time_mode_id = 1 and isnumeric(right(course.userdefined_id, 4)) = 1 and (right(course.userdefined_id, 4) + '-01-01' BETWEEN '" + _startDate + "' AND '" + _endDate + "' or (right(course.userdefined_id, 4) + '-12-31'> '" + _startDate + "' AND right(course.userdefined_id, 4) + '-01-01' < '" + _startDate + "'))))";
          
          

            if (CourseCode != null && CourseCode.value != null && CourseCode.value != "")
            {
                request += " and course.userdefined_id like '%" + CourseCode.value + "%' ";
            }
          
            
            request += "ORDER by component_id";

            return con.executeQuery(request);
        }

        public static bool hasFeedbackForm(string component_id)
        {
            DbConnection con = new DbConnection();
            string requete = "select Forms.object_id as FormObjectID from 	e_componenttype FormsTemplate, 	e_component Forms, 	e_composing eco, e_componenttype CourseTemplates, e_component Courses,	t_qti_content cs1,	t_qti_content cs2,	e_component ec where 	Forms.component_id = cs1.content_id and	cs1.template_id = cs2.content_id and	Forms.type_id = FormsTemplate.componenttype_id and 	FormsTemplate.characteristic_id = 16 and 	eco.component_id = Forms.component_id and 	Courses.component_id = eco.composing_id and 	ec.component_id = Courses.component_id and 	CourseTemplates.componenttype_id = Courses.type_id and	CourseTemplates.learning_form_id in (113,2,1722400,218356,4615133)	and ec.component_id = " + component_id;

            return  (con.getResultAsString(requete) == "" ? false : true) ;
        }

        public static DataSet getParticipantList(int id, String role = "")
        {

            DbConnection con = new DbConnection();
            User currentUser = (User)HttpContext.Current.Session["User"];

            string request = "select p.firstname as [first name], p.LASTNAME AS [Last name], pc.JOB_TITLE AS [Job Title], p.email AS [E-mail],case when CHARINDEX('/', reverse(unit)) = 2 then (case when right(unit,1)='D' then 'Deco' when right(unit,1)='H' then 'Hannover' when right(unit,1)='B' then 'Brussels' else null end) else 'none' end as sector, case when pf.[status]=8 and (GETDATE() < pf.deadline_date and pf.deadline_date is not null )  then 'Registered' when pf.[status]=8 and pf.deadline_date is null  then 'Registered' when pf.[status]=9 and  (GETDATE() < pf.deadline_date and pf.deadline_date is not null) then 'Started' when pf.[status]=9 and  pf.deadline_date is null then 'Started'  when pf.[status]=12 then 'Failed' when pf.[status] in (10, 11) then 'Passed' when pf.[status] in (8, 9) and ( getdate()>pf.deadline_date and pf.deadline_date is not null)  then 'Overdue'    when pf.[status] in (8, 9) and ( getdate()>ec.enddate and pf.deadline_date is not null)  then 'Overdue'  end as [status], pf.enddate as [Enddate],  case when pf.deadline_date is null then ec.enddate else pf.deadline_date end as Duedate from e_component ec left join e_description ed on ec.component_id= ed.component_id and ed.metatag_id=10068 inner join portfolio pf on ec.component_id = pf.component_id inner join person p on pf.person_id  = p.person_id inner join  person_custom pc on p.person_id= pc.person_id where organizer_id = 195673 and course_status_id <> 3 and p.person_id not in (2) and ec.component_id = " + id + " and pf.[status] not in (7) ";


            if (role == "TUTOR" || (role == "" && (currentUser.is*******CRO() || currentUser.isSysAdmin())))
            {
            }
            else if (currentUser != null && currentUser.isTutor())
                {
                    request += " and pf.person_id in (select person_id from portfolio WHERE component_id in (Select component_id from person_component_assignment WHERE person_id = " + currentUser.person_id +                        " and tutor_subrole_id in (1, 2)))";
                }
            else
            {
                request += " and pf.person_id in ( Select person_id from person p where p.superior_email = '" + currentUser.email + "' )";
            }
            request += "ORDER BY case when pf.[status]=8 and (GETDATE() < pf.deadline_date and pf.deadline_date is not null )  then 3 when pf.[status]=8 and pf.deadline_date is null  then 3 when pf.[status]=9 and  (GETDATE() < pf.deadline_date and pf.deadline_date is not null) then 2 when pf.[status]=9 and  pf.deadline_date is null then 2  when pf.[status]=12 then 4 when pf.[status] in (10, 11) then 1 when pf.[status] in (8, 9) and ( getdate()>pf.deadline_date and pf.deadline_date is not null)  then 5    when pf.[status] in (8, 9) and ( getdate()>ec.enddate and pf.deadline_date is not null)  then 5  end asc";
            return con.executeQuery(request);
        }

        public static DataSet courseInfo(int id) {
            DbConnection con = new DbConnection();

            string request = "select name,startdate,enddate from e_component where component_id= " +id;
            return con.executeQuery(request);
        }
    }
}