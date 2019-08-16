using CustomsBasis.Models;
using CustomsBasis.Models.DbAccess;
using CustomsBasis.Models.Util;
using System;
using System.Data;
using System.Data.OleDb;

namespace *******.Models
{
 class *******exporticao
    {


        public static void execute()
        {
           
         string role= "";
         string subject = "Reminder ******* refresh course";
            
        
            DataSet result = new DataSet();
            DbConnection com = new DbConnection();
            OleDbCommand cmdm = new OleDbCommand();
           

             //this query gives a single table where both results of the view and of the table are shown
            string request = "select  pc.DISPLAYED_FIRSTNAME + ' '+  pc.DISPLAYED_LASTNAME  as Examiner,a.person_id,  d.[*******_Paper_2_ELE], d.[*******_Paper_2_OPE], d.[*******_Paper_3_ELE], d.[*******_Paper_3_OPE] , a.[*******_Paper_2_ELE], a.[*******_Paper_2_OPE], a.[*******_Paper_3_ELE], a.[*******_Paper_3_OPE], pc.*******_NA_EMAIL  as *******FoPmail, pc.*******_NA_FULLNAME as *******fopname from (select * from  v_*******_icao c EXCEPT Select * from euro_*******_ICAO b) a inner JOIN euro_*******_ICAO d ON a.person_id = d.person_id join person_custom pc on pc.person_id=a.person_id  order by person_id";

            cmdm.CommandText = request;
            cmdm.CommandType = CommandType.Text;

    

             result = com.executeQuery(cmdm);

          
            DataColumnCollection column = result.Tables[0].Columns;
        
            foreach (DataRow row in result.Tables[0].Rows)
            {
                
               
                int id = Convert.ToInt32(row["person_id"]);
                string *******fopmail = row["*******FoPmail"].ToString();
                string *******fopname = row["*******fopname"].ToString();

               
                // the following conditionals include the update of the person_custom and the mailing system
                if (row[2].ToString() != row[6].ToString())
                {
                   
                       
                        string perscost = " UPDATE euro_*******_ICAO SET [*******_PAPER_2_ELE] = ? where person_id= ?";
                         cmdm.CommandText = perscost;
                        cmdm.CommandType = CommandType.Text;
                       
                        cmdm.Parameters.Add("paper2", OleDbType.VarChar, row[6].ToString().Length).Value = row[6].ToString();
                        cmdm.Parameters.Add("person_id1", OleDbType.Integer).Value = id;
                        com.executeUpdateQuery(cmdm);
                     

                        if (row[2].ToString() == "Overdue")
                        {
                            MailSender.sendMail(User.getUserEmail(id), subject, buildMailBodyfop(id, Convert.ToString(row["Examiner"]),  Convert.ToString(row["*******fopname"]), role = column[6].ColumnName, row[6].ToString()), 14, 0, id, User.getUserLogin(id), "ScheduledTask", "cosimo.palma.ext@*******.int", ccrecipient: *******fopmail);
                        }
                        else
                        {
                            MailSender.sendMail(User.getUserEmail(id), subject, buildMailBody(id, Convert.ToString(row["Examiner"]), role = column[6].ColumnName, row[6].ToString()), 14, 0, id, User.getUserLogin(id), "ScheduledTask", "cosimo.palma.ext@*******.int", ccrecipient: null);
                        }
                
                }
                if (row[3].ToString() != row[7].ToString())
                {





                    string perscost = " UPDATE euro_*******_ICAO SET [*******_PAPER_2_OPE] = ? where person_id= ? ";

                    cmdm.CommandText = perscost;
                    cmdm.CommandType = CommandType.Text;
                   
                    cmdm.Parameters.Add("paper2", OleDbType.VarChar, row[7].ToString().Length).Value = row[7].ToString();
                    cmdm.Parameters.Add("person_id1", OleDbType.Integer).Value = id;
                    com.executeUpdateQuery(cmdm);
                    
                if (row[3].ToString() == "Overdue")
                {

                    MailSender.sendMail(User.getUserEmail(id), subject, buildMailBodyfop(id, Convert.ToString(row["*******fopname"]), Convert.ToString(row["Examiner"]), role = column[7].ColumnName, row[7].ToString()), 14, 0, id, User.getUserLogin(id), "*******Comp", "cosimo.palma.ext@*******.int", ccrecipient: *******fopmail);
                }
                MailSender.sendMail(User.getUserEmail(id), subject, buildMailBody(id, Convert.ToString(row["Examiner"]), role = column[7].ColumnName, row[7].ToString()), 14, 0, id, User.getUserLogin(id), "*******Comp", "cosimo.palma.ext@*******.int", ccrecipient: null);
                }


                if (row[4].ToString() != row[8].ToString()) {



                    string perscost = " UPDATE euro_*******_ICAO SET [*******_PAPER_3_ELE] = ? where person_id= ? ";

                        cmdm.CommandText = perscost;
                        cmdm.CommandType = CommandType.Text;

                    
                        cmdm.Parameters.Add("paper3", OleDbType.VarChar, row[8].ToString().Length).Value = row[8].ToString();
                        cmdm.Parameters.Add("person_id1", OleDbType.Integer).Value = id;
                        com.executeUpdateQuery(cmdm);
                    
                        

                    if (row[4].ToString() == "Overdue")
                    {

                        MailSender.sendMail(User.getUserEmail(id), subject, buildMailBodyfop(id, Convert.ToString(row["Examiner"]), Convert.ToString(row["*******fopname"]), role = column[8].ColumnName, row[8].ToString()), 14, 0, id, User.getUserLogin(id), "*******Comp", "cosimo.palma.ext@*******.int", ccrecipient: *******fopmail);
                    }
                    else
                    {
                        MailSender.sendMail(User.getUserEmail(id), subject, buildMailBody(id, Convert.ToString(row["Examiner"]), role = column[8].ColumnName, row[8].ToString()), 14, 0, id, User.getUserLogin(id), "*******Comp", "cosimo.palma.ext@*******.int", ccrecipient: null);
                    }
                }

                if (row[5].ToString() != row[9].ToString())
                {

                    string perscost = "UPDATE euro_*******_ICAO SET [*******_PAPER_3_OPE]= ? where person_id= ? ";

                     cmdm.CommandText = perscost;
                    cmdm.CommandType = CommandType.Text;
                    cmdm.Parameters.Add("paper3", OleDbType.VarChar, row[9].ToString().Length).Value = row[9].ToString();
                    cmdm.Parameters.Add("person_id1", OleDbType.Integer).Value = id;
                    com.executeUpdateQuery(cmdm);
                 
                    
                    if (row[9].ToString() == "Overdue")
                    {

                        MailSender.sendMail(User.getUserEmail(id), subject, buildMailBodyfop(id, Convert.ToString(row["Examiner"]), Convert.ToString(row["*******fopname"]), role = column[9].ColumnName, row[9].ToString()), 14, 0, id, User.getUserLogin(id), "*******Comp", "*******.support@*******.int", ccrecipient: *******fopmail);
                    }

                    MailSender.sendMail(User.getUserEmail(id), subject, buildMailBody(id, Convert.ToString(row["Examiner"]), role = column[9].ColumnName, row[9].ToString()), 14, 0, id, User.getUserLogin(id), "*******Comp", "*******.support@*******.int", ccrecipient: null);
                }
            }



            string requestforinsert = "select a.person_id, pc.DISPLAYED_LASTNAME + ' '+ pc.DISPLAYED_FIRSTNAME as Examiner, pc.*******_NA_EMAIL as *******FoPmail,a.*******_Paper_2_ELE, a.*******_Paper_2_OPE, a.*******_Paper_3_ELE, a.*******_Paper_3_OPE, d.[*******_PAPER_2_ELE], d.[*******_PAPER_2_OPE], d.[*******_PAPER_3_ELE], d.[*******_PAPER_3_OPE] from (select * from  v_*******_icao c except Select * from euro_*******_ICAO b) a 	left JOIN euro_*******_ICAO d ON a.person_id = d.person_id 	join person_custom pc on pc.person_id=a.person_id 	where  d.person_id is null	 order by person_id";
            //select a.person_id, a.IS_*******_ADMIN,a.IS_*******_PAPER1,a.*******_Paper_2_ELE, a.*******_Paper_2_OPE, a.*******_Paper_3_ELE, a.*******_Paper_3_OPE,  a.MAX_ACR_ENDDATE,a.MAX_REF_ENDDATE,a.MAX_L6E_ENDDATE d.[******* Paper 2 (ELE)] from (select * from  v_*******_icao c except Select * from euro_*******_ICAO b) a 	left JOIN euro_*******_ICAO d ON a.person_id = d.person_id 	join person_custom pc on pc.person_id=a.person_id 	where  d.person_id is null	 order by person_id";
                  
            //      " ;

            cmdm.CommandText = requestforinsert;
            cmdm.CommandType = CommandType.Text;

            result = com.executeQuery(cmdm);


            //foreach (DataRow row in result.Tables[0].Rows)
            //{


            //    string perscost = "  insert into euro_*******_ICAO select * from  v_*******_icao c  where person_id = ? ";

            //    cmdm.CommandText = perscost;
            //    cmdm.CommandType = CommandType.Text;
            //    cmdm.Parameters.Add("person_id", OleDbType.Integer).Value = Convert.ToInt32(row["person_id"]);
            //    com.executeInsertQuery(cmdm);



            //}
            DbConnection cok = new DbConnection();
            OleDbCommand cmddelk = new OleDbCommand();
            OleDbCommand cmdk = new OleDbCommand();
            cmdk.Connection = cok.connection;

            string richiesta = "DELETE FROM euro_*******_ICAO ";

            cmddelk.CommandText = richiesta;
            cmddelk.CommandType = CommandType.Text;
            cok.executeQuery(cmddelk);


            string req = "INSERT INTO euro_*******_ICAO Select * from v_*******_ICAO";
            cmdk.CommandText = req;
            cmdk.CommandType = CommandType.Text;
            cok.executeInsertQuery(cmdk);

        }


              public static string buildMailBodyfop (int id, string efop, string examiner, string role, string status)
        {
                
            string body = "<STRONG>Dear " + efop + ",</STRONG></P>"
            + "<p style='font-size: 10pt; color: #000000; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>Please be informed that the role " + role + " of the examiner "+examiner+" is no longer valid since he has missed the last deadline for requesting the refresher course.</p>"
            + "<p style='font-size: 10pt; color: #000000; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'> Please contact the ******* Admynistrator to remove or restore him. To get a full overview,  <a href='" + CustomsBasis.Models.Util.ServerData.getServerHostName() + "/ilp/pages/internal-dashboard.jsf?dashboardId=6692664&returnUrl=/ilp/customs/Reports/*******' style='color:#3399CC'><span style='color:#3399CC'>open the competency scheme</span></a>.</p>";
          
            body += "<br /><p style='font-size: 10pt; color: #000000; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>Kind regards,<BR><EM>Training Zone Support</EM><br />";

            return body;
        }

              public static string buildMailBody(int id, string examiner,  string role, string status)
              {
                  string body = "";

                  if(status == "Attention required"){
                   body = "<STRONG>Dear " + examiner + ",</STRONG></P>"
            + "<p style='font-size: 10pt; color: #FF9933; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>Please be informed that your role " + role + " is stil  valid only until the " + DateTime.Today.AddMonths(6).ToString("dd/MM/yyyy") + ".  </p>"
            + "<p style='font-size: 10pt; color: #000000; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>To register for a refresher course, please <a href='https://*******.*******.int/ilp/pages/coursedescription.jsf?courseId=6804162&catalogId=1121948' style='color:#3399CC'><span style='color:#3399CC'>open the competency scheme</span></a>.</p>";

                  body += "<br /><p style='font-size: 10pt; color: #000000; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>Kind regards,<BR><EM>Training Zone Support</EM><br />";
                  }

                  else{
                       body = "<STRONG>Dear " + examiner + ",</STRONG></P>"
               + "<p style='font-size: 10pt; color: #CC0033; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>Please be informed that your role " + role + " is stil  valid only until the " + DateTime.Today.AddMonths(3).ToString("dd/MM/yyyy") + ".  </p>"
            + "<p style='font-size: 10pt; color: #000000; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>To register for a refresher course, please <a href='https://*******.*******.int/ilp/pages/coursedescription.jsf?courseId=6804162&catalogId=1121948' style='color:#3399CC'><span style='color:#3399CC'>open the competency scheme</span></a>.</p>";

                      body += "<br /><p style='font-size: 10pt; color: #000000; margin-top: 0; margin-right: 0; margin-bottom: 1.6em; margin-left: 0;'>Kind regards,<BR><EM>Training Zone Support</EM><br />";
                  }
                  return body;


              }
       
        }
    
}


          
    // About the mailing
//Looking at the requirement document (the initial presentation I made to Pieke), 4 mails need to be sent: the last mail listed is to the ******* FoP, not the participant. That is the mail when they go grey. The mail below is the mail when they go red, so that correctly goes to the participant.

    

