using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.Models
{
	/// <summary>
	/// Classe responsável por armazenar uma lista de pontos (latitude e longitude)
	/// </summary>
	public class DirectionsModel
	{
		public DirectionsModel()
		{
			this.Path = new List<Point>();
		}

		/// <summary>
		/// Recebe toda a string enviada pela API do Google e transforma esta em uma lista de Points.
		/// </summary>
		/// <param name="directions">String com todos os pontos enviados pela API do google
		/// Recebe em formato: (-30.027700000000003, -51.22874),(-30.027700000000003, -51.228730000000006),(-30.027620000000002, -51.22842000000001)...
		/// </param>
		public DirectionsModel(string directions)
		{
			this.Path = new List<Point>();

			var temp = directions.Replace("\r", "").Replace("\n", "").Replace("\t", ""); //com um pouco de mágica
			temp = temp.Replace("),(", "*").Replace(")(", "*").Replace("(", "").Replace(",", ""); //separamos em arrays de pontos
			temp = temp.Replace(")", "");
			var arrayDirections = temp.Split('*');
			//cada elemento agora tem o formato de: -30.027700000000003 -51.22874
			for (int index = 0; index < arrayDirections.Length; index++)
			{
				var elements = arrayDirections[index].Split(' ');
				var newPoint = new Point(elements[0], elements[1]);
				this.Path.Add(newPoint);
			}
			var i = 0;
		}

		public List<Point> Path { get; set; }

	}
}