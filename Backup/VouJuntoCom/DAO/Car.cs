using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa a tabela Car do banco.
	/// </summary>
	public class Car
	{
		public Guid ID { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string Color { get; set; }
		public string RENAVAM { get; set; }
		public int? Digits { get; set; }
		public bool ArConditioning { get; set; }
		public bool Radio { get; set; }
		public bool Smoke { get; set; }
		public bool Pet { get; set; }
	}
}