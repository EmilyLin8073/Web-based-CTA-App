//
// One CTA Station
//

namespace program.Models
{

  public class Top10Station
	{
	
		// data members with auto-generated getters and setters:
        public int StationID { get; set; }
		public string StationName { get; set; }
		public int DailyRidership { get; set; }
        public int NumOfStops { get; set; }
        public string HandicapInfo { get; set; }
	
		// default constructor:
		public Top10Station()
		{ }
		
		// constructor:
		public Top10Station(int id, string name, int dailyRidership, int numOfStops, string handicapInfo)
		{
			StationID = id;
			StationName = name;
			DailyRidership = dailyRidership;
            NumOfStops = numOfStops;
            HandicapInfo = handicapInfo;
		}
		
	}//class

}//namespace