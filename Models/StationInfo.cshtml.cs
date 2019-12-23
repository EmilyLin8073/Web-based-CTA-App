using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace program.Pages  
{  
    public class StationInfoModel : PageModel  
    {  
				public List<Models.Station> StationList { get; set; }
				public string Input { get; set; }
				public Exception EX { get; set; }
  
        public void OnGet(string input)  
        {  
				  StationList = new List<Models.Station>();
					
					// make input available to web page:
					Input = input;
					
					// clear exception:
					EX = null;
					
					try
					{
						//
						// Do we have an input argument?  If not, there's nothing to do:
						//
						if (input == null)
						{
							//
							// there's no page argument, perhaps user surfed to the page directly?  
							// In this case, nothing to do.
							//
						}
						else  
						{
							// 
							// Lookup CTA(s) based on input, which could be id or a partial name:
							// 
							string sql;

						  // lookup station(s) by partial name match:
							input = input.Replace("'", "''");

							sql = string.Format(@"
                                                    SELECT Stations.StationID, Stations.Name,
                                                    (SELECT AVG(DailyTotal) FROM Riderships WHERE Stations.StationID = Riderships.StationID ) AS AvgDailyRidership, 
                                                    (SELECT COUNT(StopID) FROM Stops WHERE Stations.StationID = Stops.StationID) AS NumOfStops,
                                                    (SELECT SUM(CAST(ADA AS INT)) FROM Stops WHERE Stations.StationID = Stops.StationID) AS HandicapInfo
                                                    FROM Stations, Riderships, Stops
                                                    WHERE Stations.Name LIKE '%{0}%'
                                                    GROUP BY Stations.StationID, Stations.Name
                                                    ORDER BY Stations.Name ASC;
                                                    ", input);

							DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

							foreach (DataRow row in ds.Tables[0].Rows)
							{
								Models.Station s = new Models.Station();

								s.StationID = Convert.ToInt32(row["StationID"]);
								s.StationName = Convert.ToString(row["Name"]);
                                s.NumOfStops = Convert.ToInt32(row["NumOfStops"]);
                                s.HandicapInfo = Convert.ToString(row["HandicapInfo"]);
                                
								// avg could be null if there is no ridership data:
								if (row["AvgDailyRidership"] == System.DBNull.Value)
									s.AvgDailyRidership = 0;
								else
									s.AvgDailyRidership = Convert.ToInt32(row["AvgDailyRidership"]);
                                    
                                //For handicap info
                                if (row["HandicapInfo"] == System.DBNull.Value)
									s.HandicapInfo = "None";
								else if (Int32.Parse(s.HandicapInfo) == s.NumOfStops)
                                    s.HandicapInfo = "All";
								else if(Int32.Parse(s.HandicapInfo) < s.NumOfStops && Int32.Parse(s.HandicapInfo) != 0)
                                    s.HandicapInfo = "Some";
                                else
                                    s.HandicapInfo = "None";
                                    

								StationList.Add(s);
							}
						}//else
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