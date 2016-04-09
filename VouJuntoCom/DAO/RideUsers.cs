using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa a associação dos recebedores da carona com a carona.
	/// </summary>
	public class RideUsers
	{
		[Key]
		[Column(Order = 0)]
		public Guid RideID { get; set; }
		[Key]
		[Column(Order = 1)]
		public Guid UserID { get; set; }
	}
}