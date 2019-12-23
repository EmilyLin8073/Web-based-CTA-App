using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace program.Pages  
{  
    public class Top10StationModel : PageModel  
    {  
        public List<Models.Top10Station> Top10StationList { get; set; }
		public Exception EX { get; set; }
  
        public void OnGet()  
        {  
            Top10StationList = new List<Models.Top10Station>();
				
					
			// clear exception:
			EX = null;
					
			try
			{
                string sql;

				sql = string.Format(@"
                                       SELECT TOP 10 Stations.StationID, Stations.Name,
                                       (SELECT SUM(DailyTotal) FROM Riderships WHERE Stations.StationID = Riderships.StationID ) AS DailyRidership, 
                                       (SELECT COUNT(StopID) FROM Stops WHERE Stations.StationID = Stops.StationID) AS NumOfStops,
                                       (SELECT SUM(CAST(ADA AS INT)) FROM Stops WHERE Stations.StationID = Stops.StationID) AS HandicapInfo
                                        FROM Stations
                                        GROUP BY Stations.StationID, Stations.Name
                                        ORDER BY DailyRidership DESC;
                                     ");

				DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

				foreach (DataRow row in ds.Tables[0].Rows)
				{
					Models.Top10Station s = new Models.Top10Station();
                    if(s==null) break;

					s.StationID = Convert.ToInt32(row["StationID"]);
					s.StationName = Convert.ToString(row["Name"]);
                    s.NumOfStops = Convert.ToInt32(row["NumOfStops"]);
                    s.HandicapInfo = Convert.ToString(row["HandicapInfo"]);
                                
					// avg could be null if there is no ridership data:
					if (row["DailyRidership"] == System.DBNull.Value)
                        s.DailyRidership = 0;
					else
						s.DailyRidership = Convert.ToInt32(row["DailyRidership"]);
                                    
                     //For handicap info
                     if (row["HandicapInfo"] == System.DBNull.Value)
                         s.HandicapInfo = "None";
					 else if (Int32.Parse(s.HandicapInfo) == s.NumOfStops)
                         s.HandicapInfo = "All";
					 else if(Int32.Parse(s.HandicapInfo) < s.NumOfStops && Int32.Parse(s.HandicapInfo) != 0)
                         s.HandicapInfo = "Some";
                     else
                         s.HandicapInfo = "None";
                                    
//                      if(s==null)
//                      {
//                         EX = new Exception("ERR");
// //                        EX.Message = "ERR";
//                         break;
//                      }
					 Top10StationList.Add(s);
				}
            }
		    catch(Exception ex)
            {
                EX = new Exception(ex.ToString()+" Error here.");//ex.ToString();  //new Exception(ex.Message+" Error here.");
			}
			finally
			{
                // nothing at the moment
            }
		}
			
    }//class  
}//namespace