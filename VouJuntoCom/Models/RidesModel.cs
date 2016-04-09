using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.Models
{
	/// <summary>
	/// Classe que representa uma instancia de carona no banco de dados.
	/// </summary>
	public class RidesModel
	{
		public Guid ID { get; set; }
		public Guid DonorID { get; set; }
		public CarModel Car { get; set; }
		public string DonorName { get; set; }
		public string FromPlace { get; set; }
		public string ToPlace { get; set; }
		public DateTime? TimeFrom { get; set; }
		public int? Seats { get; set; }
		public int? TrunkSize { get; set; }
		public DateTime? GiveUpTolerance { get; set; }
		public string Message { get; set; }
		public decimal Price { get; set; }
		public DirectionsModel Direction { get; set; }
		public int RemainingSeats { get; set; }
		public bool Canceled { get; set; }
		public double? Distance { get; set; }
	}

	
}