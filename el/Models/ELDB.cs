using CustomsBasis.Models.DbAccess;
using CustomsBasis.Models.Filters;
using CustomsBasis.Models;
using CustomsBasis.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.ComponentModel.DataAnnotations;




namespace EL.Models
{

    public class ELDB
    {
        
   
        public  Dictionary <string, DataSet> data { get; set; }
        public string id { get; set; }
        List<ExaminerOverview> Results { get; set; }
        List<commenti> Result { get; set;  }

        public *******DB()
        {

            data = new Dictionary<string, DataSet>();
            Results = new List<ExaminerOverview>();
        }
                
        public static DataSet firsttable(int id=0)
        {

            DbConnection con = new DbConnection();
            OleDbCommand cmd = new OleDbCommand();
            User currentUser = (User)HttpContext.Current.Session["User"];


            string request = " select   sta.Stakeholder_id, sta.Internal_id, cod.country_name as Country, sta.Company_Name as Organization, pfop.firstname+' '+ UPPER(pfop.lastname) as *******_Focal_Point, pfop.email as *******_Email,  pdfop.firstname+' '+Upper(pdfop.lastname) as [*******_Deputy_Focal_Point] , pdfop.email as *******_FoP_Dep_Email , count(*******ex.login) as [Examiners' number] from euro_Stakeholder sta 	join (select left(Internal_id, 2) as leftint, Name from euro_Stakeholder) stak on sta.Name=stak.Name  	join country co on co.external_id=stak.leftint 	join country_d cod on cod.country_id=co.id and language_id=27	left join euro_*******_FocalPoints fop on fop.Stakeholder_ID=sta.Stakeholder_id and fop.isSubstitute=0	left join person pfop on pfop.person_id=fop.Person_ID 	left join euro_*******_FocalPoints dfop on dfop.Stakeholder_ID=sta.Stakeholder_id and dfop.isSubstitute=1	left join person pdfop on pdfop.person_id=dfop.Person_ID 	join costcenter cc on sta.Stakeholder_id=cc.object_id and is_topical_published_version=1	left join (select p.login, p.costcenter_id from person p 		join person_custom pc on pc.person_id=p.person_id 		where  (pc.IS_*******_ADMIN=1 or  pc.IS_*******_PAPER1=1  or pc.IS_*******_PAPER2_ELE=1 or pc.IS_*******_PAPER2_OPE=1 or	pc.IS_*******_PAPER3_ELE=1 or pc.IS_*******_PAPER3_OPE=1)) as *******ex  on cc.costcenter_id=*******ex.costcenter_id where sta.*******=1 ";
            if (id != 0)
            {
                request += " and (pfop.person_id=? or sta.Stakeholder_id in ( select sta.Stakeholder_ID from euro_FocalPoints fo left join euro_Stakeholder sta on sta.Stakeholder_id=fo.Stakeholder_ID  where sta.*******=1 and Person_ID=?))";

                cmd.Parameters.Add("person", OleDbType.VarChar, id.ToString().Length).Value = id;
                cmd.Parameters.Add("persona", OleDbType.VarChar, id.ToString().Length).Value = id;

            }
            request+= "group by sta.Stakeholder_id, sta.Internal_id, cod.country_name , sta.Company_Name , pfop.firstname,  pfop.lastname , pfop.email ,  pdfop.firstname, pdfop.lastname  , pdfop.email order  by Country";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = request;
            return con.executeQuery(cmd);
        }

 public static DataSet firsttablesmall(int id)
        {

            DbConnection con = new DbConnection();
            DataSet result = new DataSet();
            OleDbCommand cmd = new OleDbCommand();
            User currentUser = (User)HttpContext.Current.Session["User"];


            string request = " select   sta.Stakeholder_id, sta.Internal_id, cod.country_name as Country, sta.Company_Name as Organization, pfop.firstname+' '+ pfop.lastname as *******_Focal_Point, pfop.email as *******_Email,  pdfop.firstname+' '+pdfop.lastname as [*******_Deputy_Focal_Point] , pdfop.email as *******_FoP_Dep_Email , count(*******ex.login) as [Examiners' number] from euro_Stakeholder sta 	join (select left(Internal_id, 2) as leftint, Name from euro_Stakeholder) stak on sta.Name=stak.Name  	join country co on co.external_id=stak.leftint 	join country_d cod on cod.country_id=co.id and language_id=27	left join euro_*******_FocalPoints fop on fop.Stakeholder_ID=sta.Stakeholder_id and fop.isSubstitute=0	left join person pfop on pfop.person_id=fop.Person_ID 	left join euro_*******_FocalPoints dfop on dfop.Stakeholder_ID=sta.Stakeholder_id and dfop.isSubstitute=1	left join person pdfop on pdfop.person_id=dfop.Person_ID 	join costcenter cc on sta.Stakeholder_id=cc.object_id and is_topical_published_version=1	left join (select p.login, p.costcenter_id from person p 		join person_custom pc on pc.person_id=p.person_id 		where  (pc.IS_*******_ADMIN=1 or  pc.IS_*******_PAPER1=1  or pc.IS_*******_PAPER2_ELE=1 or pc.IS_*******_PAPER2_OPE=1 or	pc.IS_*******_PAPER3_ELE=1 or pc.IS_*******_PAPER3_OPE=1)) as *******ex  on cc.costcenter_id=*******ex.costcenter_id where sta.*******=1  and pfop.person_id=? group by sta.Stakeholder_id, sta.Internal_id, cod.country_name , sta.Company_Name , pfop.firstname,  pfop.lastname , pfop.email ,  pdfop.firstname, pdfop.lastname  , pdfop.email order  by Country";

            cmd.Parameters.Add("stakeholderId", OleDbType.VarChar, id.ToString().Length).Value = id;

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = request;

            return con.executeQuery(cmd);

             }


        public static DataSet getExaminers(int id)
        {

            DbConnection con = new DbConnection();
            DataSet result = new DataSet();
            OleDbCommand cmd = new OleDbCommand();
            User currentUser = (User)HttpContext.Current.Session["User"];

            string request = "select distinct  upper(p.lastname) as Lastname, p.firstname as Firstname, upper(p.lastname)+' '+ p.firstname as Examiner,  *******.* from euro_*******_ICAO *******  join person p on p.person_id=*******.person_id  join  costcenter cost on p.costcenter_id=cost.costcenter_id  join euro_Stakeholder sta on cost.object_id=sta.Stakeholder_id  join (select left(Internal_id, 2) as leftint, Name from euro_Stakeholder) stak on sta.Name=stak.Name left join country co on co.external_id=stak.leftint  left  join country_d cod on cod.country_id=co.id left join person_custom pc on pc.person_id=p.person_id where    cod.language_id=27   and Stakeholder_id= ? and (pc.IS_*******_ADMIN=1 or  pc.IS_*******_PAPER1=1  or IS_*******_PAPER2_ELE=1 or  IS_*******_PAPER2_OPE=1 or  IS_*******_PAPER3_ELE=1 or IS_*******_PAPER3_OPE=1) order by Lastname";

            cmd.Parameters.Add("stakeholderId", OleDbType.VarChar, id.ToString().Length).Value = id;

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = request;

            return con.executeQuery(cmd);



        }

        public static DataSet internalstakid(string internalid)
        {

            DbConnection con = new DbConnection();
            DataSet result = new DataSet();
            OleDbCommand cmd = new OleDbCommand();
            User currentUser = (User)HttpContext.Current.Session["User"];

            string request = " select distinct  Internal_id, Stakeholder_id from   euro_Stakeholder where *******_FirstName not like '' and Internal_id like ? ";

            cmd.Parameters.Add("internalId", OleDbType.VarChar, internalid.ToString().Length).Value = internalid;

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = request;



            return con.executeQuery(cmd);



        }


       
     
        public static List<ExaminerOverview> getExaminersbig()
        {

            DbConnection con = new DbConnection();
            DataSet result = new DataSet();

            User currentUser = (User)HttpContext.Current.Session["User"];

            string request = "select distinct  upper(p.lastname) as Lastname, p.firstname as Firstname, cod.country_name as Country, sta.Company_Name as Organization,  case when cast( ******* as varchar(20))=1 then 'Valid' else '' end as *******, *******.*******, *******.*******, *******.*******_Paper_2_OPE, *******.*******_Paper_3_ELE, *******.*******_Paper_3_OPE, cast( *******.MAX_ACR_ENDDATE as varchar(10)) as MAX_ACR_ENDDATE, cast(*******.MAX_REF_ENDDATE as varchar(10)) as MAX_REF_ENDDATE, cast(*******.MAX_L6E_ENDDATE as varchar(10)) as MAX_L6E_ENDDATE from euro_*******_ICAO *******  join person p on p.person_id=*******.person_id  join  costcenter cost on p.costcenter_id=cost.costcenter_id  join euro_Stakeholder sta on cost.object_id=sta.Stakeholder_id  join (select left(Internal_id, 2) as leftint, Name from euro_Stakeholder) stak on sta.Name=stak.Name left join country co on co.external_id=stak.leftint  left  join country_d cod on cod.country_id=co.id left join person_custom pc on pc.person_id=p.person_id where    cod.language_id=27  order by Lastname";

                    result = con.executeQuery(request);


            
                  List<ExaminerOverview> Results = new List<ExaminerOverview>();

                  foreach (DataRow row in result.Tables[0].Rows)
                  {
                      Results.Add(
                              new ExaminerOverview
                              {

                                  Lastname = Convert.ToString(row["Lastname"]),
                                  Firstname = Convert.ToString(row["Firstname"]),
                                  Country = Convert.ToString(row["Country"]),
                                  Organization = Convert.ToString(row["Organization"]),
                                  is*******admin = Convert.ToString(row["IS_*******_ADMIN"]),
                                  is*******paper1 = Convert.ToString(row["IS_*******_PAPER1"]),
                                  is*******paper2ele = Convert.ToString(row["*******_Paper_2_ELE"]),
                                  is*******paper2ope = Convert.ToString(row["*******_Paper_2_OPE"]),
                                  is*******paper3ele = Convert.ToString(row["*******_Paper_3_ELE"]),
                                  is*******paper3ope = Convert.ToString(row["*******_Paper_3_OPE"]),
                                  maxACRenddate = Convert.ToString(row["MAX_ACR_ENDDATE"]),
                                  maxREFenddate = Convert.ToString(row["MAX_REF_ENDDATE"]),
                                  maxL6Eenddate = Convert.ToString(row["MAX_L6E_ENDDATE"]),


                              });




                  }

            return Results;
        }

        


        public static Dictionary<string, string> getpeople()
        {


            DataSet resultset = new DataSet();


            DbConnection database = new DbConnection();


            Dictionary<string, string> newDic = new Dictionary<string, string>();

            string query = "select distinct upper(p.lastname) +' '+p.firstname as Examiner, 	p.person_id as ID from person_custom pc join person p on p.person_id = pc.person_id where  IS_*******_ADMIN=1 or IS_*******_PAPER1=1 or  IS_*******_PAPER2_ELE=1 or IS_*******_PAPER2_OPE=1 or IS_*******_PAPER3_ELE=1 or IS_*******_PAPER3_OPE=1 order by Examiner";


            resultset = database.executeQuery(query);


            foreach (System.Data.DataRow row in resultset.Tables[0].Rows)
            {
                newDic[(string)(row["Examiner"])] = Convert.ToString(row["ID"]);


            }
            return newDic;
        }

           public static DataSet getCertificate(int id)
           {

               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();
               User currentUser = (User)HttpContext.Current.Session["User"];

               string request = " select distinct p.firstname+'  '+ p.lastname as Examiner, pc.IS_*******_ADMIN, pc.IS_*******_PAPER1, pc.IS_*******_PAPER2_ELE, pc.IS_*******_PAPER2_OPE, pc.IS_*******_PAPER3_ELE, pc.IS_*******_PAPER3_OPE,  name as Course , ec.startdate,  p.person_id, max(ec.enddate) as enddate from person_custom pc join person p on p.person_id=pc.person_id join portfolio pf on pf.person_id=pc.person_id join e_component ec on ec.component_id=pf.component_id where p.person_id=? group by  p.firstname, p.lastname, pc.IS_*******_ADMIN, pc.IS_*******_PAPER1, pc.IS_*******_PAPER2_ELE, pc.IS_*******_PAPER2_OPE, pc.IS_*******_PAPER3_ELE, pc.IS_*******_PAPER3_OPE,  name, ec.startdate,  p.person_id";

               cmd.Parameters.Add("personid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               return con.executeQuery(cmd);
             
           }

           public static DataSet personname(int id)
           {
               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();

               string request = "select distinct lastname+' '+firstname as Examiner from person where person_id=?";

               cmd.Parameters.Add("personid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               return con.executeQuery(cmd);
           }


           public static DataSet fopof*******org(int id)
           {
               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();

               string request = "select sta.Stakeholder_ID from euro_FocalPoints fo  left join euro_Stakeholder sta on sta.Stakeholder_id=fo.Stakeholder_ID where sta.*******=1 and Person_ID=?";

               cmd.Parameters.Add("personid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               return con.executeQuery(cmd);
           }

          

           public static DataSet orgInfo(int id)
           {
               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();

               string request = "select distinct  cod.country_name as Country, sta.Company_Name as Organization, *******_FirstName+' '+ *******_LastName as *******_Focal_Point, *******_Email, Stakeholder_id,  *******_FoP_Dep_FirstName+' '+*******_FoP_Dep_LastName as [*******_Deputy_Focal_Point] from euro_Stakeholder sta join (select left(Internal_id, 2) as leftint, Name from euro_Stakeholder) stak on sta.Name=stak.Name join country co on co.external_id=stak.leftint join country_d cod on cod.country_id=co.id where *******=1 and inCRF=1 and language_id=27 and Stakeholder_id=?";

               cmd.Parameters.Add("stakeholderid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               return con.executeQuery(cmd);
               
           }
        

           public static DataSet courses(int id)
           {
               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();

               string request = "select distinct p.lastname+' '+ p.firstname as Examiner,  name as Course , (SELECT lsd.name from r_location_structure ls, r_location_structure_d lsd WHERE ls.node_id=ec.location_id AND ls.node_id=lsd.node_id and lsd.Language_id=27) as Location,  cast(max(ec.startdate) as varchar(10)) as startdate,  p.person_id as person_id, cast( max(ec.enddate) as varchar(10))  as enddate from e_component ec join portfolio pf on ec.component_id=pf.component_id join person p on p.person_id=pf.person_id join person_custom pc on pc.person_id=pf.person_id where ec.language_id=27 and is_template=0 and organizer_id= 1082138 and ec.startdate is not null and ec.enddate is not null and ec.typeupdate_id in (4656366, 1082267, 1082349, 1524296)  and name not like '%MTG%' and p.person_id=? group by  p.lastname, p.firstname, name, ec.location_id, ec.component_id, ec.startdate,  p.person_id order by startdate";

               cmd.Parameters.Add("personid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               return con.executeQuery(cmd);
           }

        

           public static DataSet examinerinfo(int id)
           {
               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();

               string request = "select  p.person_id, p.lastname, p.firstname, COMPANY_DEPARTMENT, JOB_TITLE, PRIVATE_PHONE, BIRTHDATE, email,  picture, country_name, efop.Person_ID  as FoPid, defop.Person_ID  as DeFoPid, stak.Stakeholder_id as Stakeid from person_custom pc join person p on p.person_id=pc.person_id join country_d cod on cod.country_id=p.country_id left join costcenter cc on cc.costcenter_id=p.costcenter_id left join euro_Stakeholder stak on stak.Stakeholder_id=cc.object_id  left join euro_*******_FocalPoints efop  on efop.Stakeholder_ID=stak.Stakeholder_id and efop.isSubstitute=0 left join euro_*******_FocalPoints defop  on efop.Stakeholder_ID=stak.Stakeholder_id and efop.isSubstitute=1 where p.person_id=? and cod.language_id=27";

               cmd.Parameters.Add("personid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               return con.executeQuery(cmd);
               
           }

         public static DataSet getroles(int id)
           {
               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();

               string request = " select distinct  p.person_id,sta.Company_Name as Organization,  case when cast(*******.IS_*******_ADMIN as varchar(20))=1 then 'Valid' else '' end as IS_*******_ADMIN, *******.IS_*******_PAPER1, *******.*******_Paper_2_ELE, *******.*******_Paper_2_OPE, *******.*******_Paper_3_ELE, *******.*******_Paper_3_OPE,  cast( *******.MAX_ACR_ENDDATE as varchar(10)) as MAX_ACR_ENDDATE, cast(*******.MAX_REF_ENDDATE as varchar(10)) as MAX_REF_ENDDATE, cast(*******.MAX_L6E_ENDDATE as varchar(10)) as MAX_L6E_ENDDATE from euro_*******_ICAO *******  join person p on p.person_id=*******.person_id  join  costcenter cost on p.costcenter_id=cost.costcenter_id  join euro_Stakeholder sta on cost.object_id=sta.Stakeholder_id  join (select left(Internal_id, 2) as leftint, Name from euro_Stakeholder) stak on sta.Name=stak.Name left join country co on co.external_id=stak.leftint  left  join country_d cod on cod.country_id=co.id left join person_custom pc on pc.person_id=p.person_id where p.person_id=?";

               cmd.Parameters.Add("personid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               return con.executeQuery(cmd);
               
           }

           public static List<ExaminerOverview> getExaminersmall(int id)
           {

               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();

               DataSet result = new DataSet();

               User currentUser = (User)HttpContext.Current.Session["User"];

               string request = "select distinct   upper(p.lastname) as Lastname, p.firstname as Firstname, upper(p.lastname)+' '+p.firstname as Examiner, cod.country_name as Country, sta.Company_Name as Organization,  case when cast(*******.IS_*******_ADMIN as varchar(20))=1 then 'Valid' else '' end as IS_*******_ADMIN, *******.IS_*******_PAPER1, *******.*******_Paper_2_ELE, *******.*******_Paper_2_OPE, *******.*******_Paper_3_ELE, *******.*******_Paper_3_OPE,  cast( *******.MAX_ACR_ENDDATE as varchar(10)) as MAX_ACR_ENDDATE, cast(*******.MAX_REF_ENDDATE as varchar(10)) as MAX_REF_ENDDATE, cast(*******.MAX_L6E_ENDDATE as varchar(10)) as MAX_L6E_ENDDATE from euro_*******_ICAO *******  join person p on p.person_id=*******.person_id  join  costcenter cost on p.costcenter_id=cost.costcenter_id  join euro_Stakeholder sta on cost.object_id=sta.Stakeholder_id  join (select left(Internal_id, 2) as leftint, Name from euro_Stakeholder) stak on sta.Name=stak.Name left join country co on co.external_id=stak.leftint  left  join country_d cod on cod.country_id=co.id left join person_custom pc on pc.person_id=p.person_id where    cod.language_id=27  and Stakeholder_id= 100554 and (pc.IS_*******_ADMIN=1 or  pc.IS_*******_PAPER1=1  or IS_*******_PAPER2_ELE=1 or  IS_*******_PAPER2_OPE=1 or  IS_*******_PAPER3_ELE=1 or IS_*******_PAPER3_OPE=1) order by Examiner";

               cmd.Parameters.Add("stakeholderId", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               result = con.executeQuery(cmd);

               List<ExaminerOverview> Results = new List<ExaminerOverview>();

               foreach (DataRow row in result.Tables[0].Rows)
               {
                   Results.Add(
                           new ExaminerOverview
                           {
                               Lastname = Convert.ToString(row["Lastname"]),
                               Firstname = Convert.ToString(row["Firstname"]),
                               Examiner = Convert.ToString(row["Examiner"]),
                               Country = Convert.ToString(row["Country"]),
                               Organization = Convert.ToString(row["Organization"]),
                               is*******admin = Convert.ToString(row["IS_*******_ADMIN"]),
                               is*******paper1 = Convert.ToString(row["IS_*******_PAPER1"]),
                               is*******paper2ele = Convert.ToString(row["*******_Paper_2_ELE"]),
                               is*******paper2ope = Convert.ToString(row["*******_Paper_2_OPE"]),
                               is*******paper3ele = Convert.ToString(row["*******_Paper_3_ELE"]),
                               is*******paper3ope = Convert.ToString(row["*******_Paper_3_OPE"]),
                               maxACRenddate = Convert.ToString(row["MAX_ACR_ENDDATE"]),
                               maxREFenddate = Convert.ToString(row["MAX_REF_ENDDATE"]),
                               maxL6Eenddate = Convert.ToString(row["MAX_L6E_ENDDATE"]),



                           });



               }
                              return Results;
           
           }


           public static List<commenti> displayComments(int id)
           {

               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();
               DataSet result = new DataSet();

               User currentUser = (User)HttpContext.Current.Session["User"];
               string request = "select id, comment, (select firstname+' '+lastname from person where person_id= updater_id) as upname, creation_date from euro_*******_check where person_id=? and archive=0";

               cmd.Parameters.Add("personid", OleDbType.VarChar, id.ToString().Length).Value = id;

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;


               result= con.executeQuery(cmd);

               List<commenti> Result = new List<commenti>();

               foreach (DataRow row in result.Tables[0].Rows)
               {
                   Result.Add(
                           new commenti
                           {
                               id = Convert.ToString(row["id"]),
                               comment = Convert.ToString(row["comment"]),
                               upname = Convert.ToString(row["upname"]), 
                               creationdate = Convert.ToString(row["creation_date"]),
                           

                           });



               }
               return Result;



           }


           internal static void insertComment(int id, string comment)
           {
               
               DbConnection con = new DbConnection();
               OleDbCommand cmd = new OleDbCommand();
               User currentUser = (User)HttpContext.Current.Session["User"];

               string request = "insert into euro_*******_check values (?, ?, ?, ?, ?, 0 )";

               cmd.Parameters.Add("examinerid", OleDbType.VarChar, id.ToString().Length).Value = id;
               cmd.Parameters.Add("comment", OleDbType.VarChar, comment.Length).Value = comment;
               cmd.Parameters.Add("creation_date", OleDbType.VarChar, DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss").Length).Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
               cmd.Parameters.Add("last_updater", OleDbType.VarChar, DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss").Length).Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
               cmd.Parameters.Add("updater_id", OleDbType.VarChar, currentUser.person_id.ToString().Length).Value = currentUser.person_id.ToString();
       

               cmd.CommandType = CommandType.Text;
               cmd.CommandText = request;

               con.executeInsertQuery(cmd);
           }


           public static void deleteComment(int id)
           {
               DbConnection database = new DbConnection();
               OleDbCommand cmdUpdate = new OleDbCommand();
               OleDbDataAdapter daUpdate = new OleDbDataAdapter();
               cmdUpdate.Connection = database.connection;

               string requestUpdate = "Update euro_*******_check set last_updated = ?, archive=1 where id = ?  ";
               cmdUpdate.Parameters.Add("last_updated", OleDbType.VarChar, DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss").Length).Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
               cmdUpdate.Parameters.Add("creationdate", OleDbType.VarChar, id.ToString().Length).Value = id.ToString();

               cmdUpdate.CommandText = requestUpdate;
               cmdUpdate.CommandType = CommandType.Text;

               database.connection.Open();
               cmdUpdate.Prepare();
               daUpdate.UpdateCommand = cmdUpdate;

               cmdUpdate.ExecuteNonQuery();

               database.connection.Close();
           }


           public static void updateComment(string comment, string id)
           {
               DbConnection database = new DbConnection();
               OleDbCommand cmdUpdate = new OleDbCommand();
               OleDbDataAdapter daUpdate = new OleDbDataAdapter();
               cmdUpdate.Connection = database.connection;

               string requestUpdate = "Update euro_*******_check set comment =?, last_updated = ? where id = ?  ";

               cmdUpdate.Parameters.Add("comment", OleDbType.VarChar, comment.Length).Value = comment;
               cmdUpdate.Parameters.Add("last_updated", OleDbType.VarChar, DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss").Length).Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
               cmdUpdate.Parameters.Add("id", OleDbType.VarChar, id.ToString().Length).Value = id.ToString();

               cmdUpdate.CommandText = requestUpdate;
               cmdUpdate.CommandType = CommandType.Text;

               database.connection.Open();
               cmdUpdate.Prepare();
               daUpdate.UpdateCommand = cmdUpdate;

               cmdUpdate.ExecuteNonQuery();

               database.connection.Close();
           }

    }

    }