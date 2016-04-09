using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe correspondente a tabela Directions do banco
	/// </summary>
	public class Directions
	{
		public Guid ID { get; set; }
		public Guid Ride_ID { get; set; }
		public string Lat { get; set; }
		public string Long { get; set; }
		public int Position { get; set; }
	}

	/// <summary>
	/// Realiza as operações de CRUD relativas a tabela de Directions
	/// </summary>
	public class DirectionsManager
	{

	}
}