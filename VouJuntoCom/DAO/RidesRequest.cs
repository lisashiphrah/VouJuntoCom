using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;
using VouJuntoCom.Models;

namespace VouJuntoCom.DAO
{
	[Table("RidesRequest", Schema = "dbo")]
	public class RidesRequest
	{
		[Key]
		[Column(Order = 0)]
		public Guid RideID { get; set; }
		[Key]
		[Column(Order = 1)]
		public Guid UserID { get; set; }
		public Guid DriverID { get; set; }
	}

	public class RidesRequestManager
	{
		/// <summary>
		/// Busca todas as reservas efetuadas pelo usuário userID
		/// </summary>
		/// <param name="userID">ID do usuário a ser utilizado na pesquisa</param>
		/// <returns>Retorna uma lista com todsa as reservas em aberto do usuário</returns>
		public static List<RidesRequest> GetAllRequestedByUser(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var listRequests = (from requests in database.RidesRequest where requests.UserID == userID select requests).ToList();
			return listRequests;
		}

		/// <summary>
		/// Busca todsa as socilitações de reserva em aberto para este motorista
		/// </summary>
		/// <param name="driverID">ID do motorista</param>
		/// <returns>Lista com todas as solicitações de reserva</returns>
		public static List<RidesRequest> GetAllRequestsByDriver(Guid driverID)
		{
			DBConfigurations database = new DBConfigurations();
			var listRequests = (from requests in database.RidesRequest where requests.DriverID == driverID select requests).ToList();
			return listRequests;
		}

		/// <summary>
		/// Verifica se usuário solicitou a reserva de uma carona
		/// </summary>
		/// <param name="userID">ID do usuário</param>
		/// <param name="rideID">ID da carona</param>
		/// <returns>True se usuário efetuou a reserva, false se não</returns>
		public static bool IsRideRequested(Guid userID, Guid rideID)
		{
			DBConfigurations database = new DBConfigurations();
			var isRequested = (from requests in database.RidesRequest
							   where requests.UserID == userID &&
							   requests.RideID == rideID
							   select requests).Any();
			return isRequested;
		}

		/// <summary>
		/// Cancela uma reserva de uma carona
		/// </summary>
		/// <param name="userID">ID do usuário que deseja cancelar a reserva</param>
		/// <param name="driverID">ID do motorista da reserva</param>
		/// <param name="rideID">ID da carona a ser cancelada</param>
		/// <param name="error">Variável de controle de erro</param>
		public static void CancelRideReservation(Guid userID, Guid driverID, Guid rideID, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;
			try
			{

				var isRequested = (from requests in database.RidesRequest
								   where requests.UserID == userID &&
								   requests.RideID == rideID &&
								   requests.DriverID == driverID
								   select requests).First();
				database.RidesRequest.Remove(isRequested);
				database.SaveChanges();
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
			}
		}

		/// <summary>
		/// Aceita uma solicitação de reserva
		/// </summary>
		/// <param name="userID">Usuário que pediu a carona</param>
		/// <param name="rideID">Identificador da carona</param>
		/// <param name="username">Nome do usuário que solicitou a carona</param>
		/// <param name="driverName">Nome do motorista</param>
		public static void AcceptReservation(Guid userID, Guid rideID, string username)
		{
			DBConfigurations database = new DBConfigurations();
			var reserve = (from reservation in database.RidesRequest
							   where
								   reservation.RideID == rideID &&
								   reservation.UserID == userID
							   select reservation).First();

			database.RidesRequest.Remove(reserve);
			database.SaveChanges();
			RidesManager.AcceptRide(reserve.UserID, reserve.RideID, username);
			NotificationsManager.AddRideRequestNotification(userID, username, true); 
		}

		/// <summary>
		/// Rejeita uma solicitação de reserva
		/// </summary>
		/// <param name="userID">Usuário que pediu a carona</param>
		/// <param name="rideID">Identificador da carona</param>
		/// <param name="username">Nome do usuário que solicitou a carona</param>
		/// <param name="driverName">Nome do motorista</param>
		public static void RejectReservation(Guid userID, Guid rideID, string username)
		{
			DBConfigurations database = new DBConfigurations();
			var reserve = (from reservation in database.RidesRequest
						   where
							   reservation.RideID == rideID &&
							   reservation.UserID == userID
						   select reservation).First();

			database.RidesRequest.Remove(reserve);
			database.SaveChanges();
			NotificationsManager.AddRideRequestNotification(userID, username, false); 
		}

		/// <summary>
		/// Retorna uma lista de caronas com a solicitação em aberta deste usuário
		/// </summary>
		/// <param name="userID">ID do usuário a ter suas solicitações de reserva pesquisadas</param>
		/// <returns>Uma lista com todas as caronas reservadas</returns>
		public static List<RidesModel> GetAllRidesFromReservation(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var listRidesRequest = RidesRequestManager.GetAllRequestedByUser(userID);
			var listRides = new List<RidesModel>();

			foreach (var rideRequest in listRidesRequest)
			{
				var ride = (from rides in database.Ride where rides.ID == rideRequest.RideID select rides).First();
				listRides.Add(Conversor.RidesToModel(ride));
			}
			return listRides;
		}
	}
}