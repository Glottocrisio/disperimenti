using *******.Models.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using *******.Models.DbAccess;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace *******.Models
{

    public class *******Report
    {

     
 
        public Dictionary <string, DataSet> data { get; set; }


      
        public VMDropDownList dropdown { get; set; }
    
        public VMDropDownList valuesYears { get; set; }

        public *******Report()
        {
            data = new Dictionary<string, DataSet>();
            dropdown = new VMDropDownList();
            valuesYears = new VMDropDownList();
            valuesYears.selectedValue = Convert.ToString(DateTime.Now.Year);
            data = new Dictionary<string, DataSet>();
        }
      
        //WE is for "with endorsment"
        public void getListOfCoursesWE(string selectedValue)
        {
          //data.Add("dropdown", *******DBAccess.coursename(id));
            data.Add("Endorsement", *******DBAccess.getendorsement(selectedValue));
        }

       public void getminyearcourse()
       {
           data.Add("dropdown", *******DBAccess.minyearcourse());
       }

       public void populateDropDownList() {

           valuesYears.values = *******DBAccess.getYearList();
           dropdown.values = *******DBAccess.getCourseList(valuesYears.values.First().Value.ToString());
        
       }



    }
} 
