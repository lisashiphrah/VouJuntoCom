using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa os troféus atingidos pelo usuário.
	/// </summary>
	public class UserGoals
	{
		[Key]
		[Column(Order = 0)]
		public Guid UserID { get; set; }
		[Key]
		[Column(Order = 1)]
		public Guid GoalID { get; set; }
	}
}