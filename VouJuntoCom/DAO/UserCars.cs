using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa a associação entre usuário e carona.
	/// </summary>
	public class UserCars
	{
		[Key]
		[Column(Order = 0)]
		public Guid UserID { get; set; }
		[Key]
		[Column(Order = 1)]
		public Guid CarID { get; set; }
	}

	public class UserCarsManager
	{
		public static void InsertUserCars(Guid userID, Guid carID)
		{
			DBConfigurations database = new DBConfigurations();
			database.UserCars.Add(new UserCars {
				CarID = carID,
				UserID = userID
			});
			database.SaveChanges();
		}
	}
}