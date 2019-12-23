using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
  
namespace program.Pages  
{  
    public class RidershipByDayModel : PageModel  
    {  
        public List<string> Day { get; set; }
        public List<int> NumRiders { get; set; }
        public Exception EX { get; set; }
  
        public void OnGet()  
        {
          Day = new List<string>();
          NumRiders = new List<int>();
          
          EX = null;
          
          Day.Add("Sunday");
          Day.Add("Monday");
          Day.Add("Tuesday");
          Day.Add("Wednesday");
          Day.Add("Thursday");
          Day.Add("Friday");
          Day.Add("Saturday");
 
          
          try
          {
            string sql = string.Format(@"
                                            SELECT DATENAME(WEEKDAY, TheDate) AS TheDay, Sum(DailyTotal) AS NumRiders
                                            FROM Riderships
                                            GROUP BY DATENAME(WEEKDAY, TheDate)
                                            ORDER BY DATENAME(WEEKDAY, TheDate);
                                        ");
          
            DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
              int numriders = Convert.ToInt32(row["NumRiders"]);

              NumRiders.Add(numriders);
            }
		      }
		      catch(Exception ex)
		      {
            EX = ex;
		      }
		      finally
		      { 
            // nothing at the moment
          } 
        }  
        
    }//class
}//namespace