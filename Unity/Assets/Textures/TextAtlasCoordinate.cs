//------------------------------------------------------------------------------

using System;
namespace AssemblyCSharp
{
		public class TextAtlasCoordinate
		{
		
		public int id { get; set; }
		public int x { get; set; }
		public int y { get; set; }
		public int weight { get; set; }
		public int height { get; set; }
		
		public TextAtlasCoordinate(string info)
		{
			string[] spliter = info.Split(' ');
			
			id =Convert.ToInt32(spliter[0]);
			
			x = Convert.ToInt32(spliter[2]);
			y = Convert.ToInt32(spliter[3]);
			weight = Convert.ToInt32(spliter[4]);
			height = Convert.ToInt32(spliter[5]);
		}
	}
}

