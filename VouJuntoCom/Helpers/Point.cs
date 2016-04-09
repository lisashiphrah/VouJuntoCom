using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VouJuntoCom.Helpers
{
	/// <summary>
	/// Representa um ponto no mapa
	/// </summary>
	public class Point
	{
		public Point(string Lat, string Long)
		{
			this.Lat = Lat;
			this.Long = Long;
		}

		public string Lat { get; set; }
		public string Long { get; set; }
	}
}