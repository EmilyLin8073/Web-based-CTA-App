//
// One CTA Line
//

namespace program.Models
{

  public class Line
	{
	
		// data members with auto-generated getters and setters:
        public int StationID { get; set; }
		public string StationName { get; set; }
        public int NumOfStops { get; set; }

	
		// default constructor:
		public Line()
		{ }
		
		// constructor:
		public Line(int id, string name, int numOfStops)
		{
			StationID = id;
			StationName = name;
            NumOfStops = numOfStops;
		}
		
	}//class

}//namespace