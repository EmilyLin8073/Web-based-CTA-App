using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace program.Pages  
{  
    public class LineInfoModel : PageModel  
    {  
				public List<Models.Line> LineList { get; set; }
				public string Input { get; set; }
				public Exception EX { get; set; }
  
        public void OnGet(string input)  
        {  
				  LineList = new List<Models.Line>();
					
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
                                                    (SELECT COUNT(StopID) FROM Stops WHERE Stations.StationID = Stops.StationID) AS NumOfStops
                                                    FROM Stations, Lines, StopDetails, StationOrder, Stops
                                                    WHERE Lines.Color LIKE '%{0}%'
                                                    AND Lines.LineID = StopDetails.LineID
                                                    AND StopDetails.StopID = Stops.StopID
                                                    AND Stops.StationID = Stations.StationID
                                                    AND StationOrder.StationID = Stations.StationID
                                                    AND StationORder.LineID = Lines.LineID
                                                    GROUP BY Stations.StationID, Stations.Name, StationOrder.Position
                                                    ORDER BY StationOrder.Position
                                                    ", input);

							DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

							foreach (DataRow row in ds.Tables[0].Rows)
							{
								Models.Line l = new Models.Line();

								l.StationID = Convert.ToInt32(row["StationID"]);
								l.StationName = Convert.ToString(row["Name"]);
                                l.NumOfStops = Convert.ToInt32(row["NumOfStops"]);
                                    

								LineList.Add(l);
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