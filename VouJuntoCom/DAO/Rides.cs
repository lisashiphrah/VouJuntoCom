using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;
using VouJuntoCom.Models;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa uma instancia de carona no banco de dados.
	/// </summary>
	public class Rides
	{
		public Guid ID { get; set; }
		public Guid Car_ID { get; set; }
		public Guid DonorID { get; set; }
		public string DonorName { get; set; }
		public string FromPlace { get; set; }
		public string ToPlace { get; set; }
		public DateTime TimeFrom { get; set; }
		public int Seats { get; set; }
		public int? TrunkSize { get; set; }
		public DateTime? GiveUpTolerance { get; set; }
		public string Message { get; set; }
		public decimal Price { get; set; }
		public int RemainingSeats { get; set; }
		public bool Canceled { get; set; }
		public double? Distance { get; set; }
	}

	public class RidesManager
	{
		/// <summary>
		/// Busca caronas em que usuário passado como parâmetro é o motorista
		/// </summary>
		/// <param name="userID">ID do usuário a ser pesquisado</param>
		/// <returns>Lista com todas as caronas que este usuário disponibilizou</returns>
		public static List<RidesModel> GetDonorRides(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var listRides = (from rides in database.Ride where rides.DonorID == userID select rides).ToList();
			var listRidesModel = new List<RidesModel>();
			foreach (var ride in listRides)
			{
				listRidesModel.Add(Conversor.RidesToModel(ride));
			}
			return listRidesModel;
		}

		/// <summary>
		/// Retorna lista de caronas que usuário por parâmetro aceitou
		/// </summary>
		/// <param name="userID">ID do usuário</param>
		/// <returns>Lista com todas as caronas que este usuário aceitou</returns>
		public static List<RidesModel> GetReceiverRides(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var listRides = (from received in
								 (from rideUsers in database.RideUsers
								  where rideUsers.UserID == userID
								  select rideUsers)
							 join rides in database.Ride on received.RideID equals rides.ID
							 select rides).ToList();
			var listRidesModel = new List<RidesModel>();
			foreach (var ride in listRides)
			{
				listRidesModel.Add(Conversor.RidesToModel(ride));
			}
			return listRidesModel;
		}

		/// <summary>
		/// Verifica se usuário aceitou determinada carona
		/// </summary>
		/// <param name="userID">ID do usuário para verificação</param>
		/// <param name="rideID">ID da carona para verificação</param>
		/// <returns>True se aceitou. False se não aceitou.</returns>
		public static bool IsRideAccepted(Guid userID, Guid rideID)
		{
			DBConfigurations database = new DBConfigurations();
			var isRideAccepted = (from rideUsers in database.RideUsers
								  where rideUsers.UserID == userID && rideUsers.RideID == rideID
								  select rideUsers).Any();
			return isRideAccepted;
		}

		/// <summary>
		/// Responsável por criar uma nova carona
		/// </summary>
		/// <param name="ride"></param>
		/// <param name="error"></param>
		public static Guid CreateRide(RidesModel ride, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;
			var rideDTO = Conversor.RidesModelToDTO(ride);
			rideDTO.ID = Guid.NewGuid();
			rideDTO.RemainingSeats = rideDTO.Seats; //se foi recém criada, tem todos os assentos disponíveis
			database.Ride.Add(rideDTO);
			database.SaveChanges();

			int turn = 0;
			foreach (var point in ride.Direction.Path)
			{
				var direction = new Directions
				{
					ID = Guid.NewGuid(),
					Lat = point.Lat,
					Long = point.Long,
					Position = turn,
					Ride_ID = rideDTO.ID
				};
				turn++;
				database.Directions.Add(direction);
			}
			database.SaveChanges();
			return rideDTO.ID;
		}

		/// <summary>
		/// Retorna uma lista de caronas dos amigos do usuário
		/// </summary>
		/// <param name="userID">Usuário a ter as caronas de amigos pesquisada</param>
		/// <param name="from">Filtro de pesquisa - Data inicial</param>
		/// <param name="to">Filtro de pesquisa - Data final</param>
		/// <returns>Lista com caronas para o periodo selecionado</returns>
		public static List<RidesModel> RetrieveFriendsRides(Guid userID, DateTime? from, DateTime? to)
		{
			var listFriends = FriendshipManager.RetrieveFriendsID(userID);
			var ridesFromFriends = RidesManager.RetrieveRidesFromList(listFriends, from, to);
			return ridesFromFriends;
		}

		/// <summary>
		/// Seleciona todas as caronas para lista de usuários recebida.
		/// É retornada lista de caronas cadastradas par o mês vigente
		/// e que tenham ainda acentos disponíveis.
		/// </summary>
		/// <param name="listUsersID">Lista de usuários a ter as caronas pesquisadas</param>
		/// <returns>Lista com caronas</returns>
		public static List<RidesModel> RetrieveRidesFromList(List<Guid> listUsersID, DateTime? from, DateTime? to)
		{
			//Se não especificou filtro, pesquisa para caronas no mes vigente
			if ((from == null) && (to == null))
			{
				return RidesManager.RetrieveFromListNoFilter(listUsersID);
			}
			else
			{
				return RidesManager.RetrieveFromListWithFilter(listUsersID, from, to);
			}

		}

		/// <summary>
		/// Seleciona todas as caronas para lista de usuário recebida
		/// </summary>
		/// <param name="listUsersID">Lista de usuários</param>
		/// <param name="from">Data inicial de pesquisa</param>
		/// <param name="to">Data final de pesquisa</param>
		/// <returns>Lista com caronas selecionadas</returns>
		private static List<RidesModel> RetrieveFromListWithFilter(List<Guid> listUsersID, DateTime? fromd, DateTime? to)
		{
			DBConfigurations database = new DBConfigurations();
			var listRides = new List<Rides>();
			var listRidesModel = new List<RidesModel>();

			//Seleciona todas as caronas cadastradas para o mes vigente de cada usuário na lista e que tenham
			//acentos disponíveis
			foreach (var userID in listUsersID)
			{
				var listTemp = (from ride in database.Ride
								where
									(ride.DonorID == userID) &&
									(ride.TimeFrom <= to) &&
									(ride.TimeFrom >= fromd) &&
									(ride.RemainingSeats != 0)
								select ride).ToList();

				listRides.AddRange(listTemp);
			}

			foreach (var rideDTO in listRides)
			{
				listRidesModel.Add(Conversor.RidesToModel(rideDTO));
			}
			return listRidesModel;
		}

		/// <summary>
		/// Seleciona todas as caronas para lista de usuário recebida
		/// </summary>
		/// <param name="listUsersID">Lista de usuários</param>
		/// <returns>Lista com caronas dos usuário recebidos</returns>
		private static List<RidesModel> RetrieveFromListNoFilter(List<Guid> listUsersID)
		{
			DBConfigurations database = new DBConfigurations();
			var listRides = new List<Rides>();
			var listRidesModel = new List<RidesModel>();

			//Seleciona todas as caronas cadastradas para o mes vigente de cada usuário na lista e que tenham
			//acentos disponíveis
			foreach (var userID in listUsersID)
			{
				var listTemp = (from ride in database.Ride
								where
									(ride.DonorID == userID) &&
									(ride.TimeFrom.Month == DateTime.Now.Month) &&
									(ride.TimeFrom >= DateTime.Now) &&
									(ride.RemainingSeats != 0)
								select ride).ToList();

				listRides.AddRange(listTemp);
			}

			foreach (var rideDTO in listRides)
			{
				listRidesModel.Add(Conversor.RidesToModel(rideDTO));
			}
			return listRidesModel;
		}

		/// <summary>
		/// Retorna informa~ções de uma carona baseada no seu id
		/// </summary>
		/// <param name="guid">id da carona</param>
		/// <returns>RideModel</returns>
		public static RidesModel RetrieveRide(Guid guid)
		{
			DBConfigurations database = new DBConfigurations();
			var rideDTO = (from ride in database.Ride where ride.ID == guid select ride).First();
			var directions = (from direction in database.Directions orderby direction.Position 
										  where direction.Ride_ID == guid select direction).ToList();

			var rideModel = Conversor.RidesToModel(rideDTO); //preenche informações gerais da carona

			var directionsModel = new DirectionsModel(); //recupera informações de rotas
			foreach (var d in directions)
			{
				Point p = new Point(d.Lat, d.Long);
				directionsModel.Path.Add(p);
			}

			rideModel.Direction = directionsModel;

			return rideModel;

		}

		/// <summary>
		/// Envia uma requisição de reserva para o motorista
		/// </summary>
		/// <param name="username">Nome do usuário que solicita a reserva</param>
		/// <param name="rideID">ID da carona</param>
		public static void RequestRideReservation(Guid userID, Guid driverID, Guid rideID, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;
			try
			{
				RidesRequest request = new RidesRequest
				{
					DriverID = driverID,
					RideID = rideID,
					UserID = userID
				};

				database.RidesRequest.Add(request);
				database.SaveChanges();
			}
			catch (Exception)
			{
				error = ErrorEnum.ResendRideSolicitation;
			}
		}

		/// <summary>
		/// Aceita uma carona selecionada
		/// </summary>
		/// <param name="userID">ID do usuário que quer aceitar a carona</param>
		/// <param name="rideID">ID da carona que o usuário deseja aceitar </param>
		/// <returns>True se aceitado com sucesso</returns>
		public static bool AcceptRide(Guid userID, Guid rideID, string username)
		{
			DBConfigurations database = new DBConfigurations();
			try
			{
				var ride = database.Ride.First(r => r.ID == rideID);
				if (ride.RemainingSeats != 0)
				{
					ride.RemainingSeats--;
					RideUsers rideUser = new RideUsers
					{
						RideID = rideID,
						UserID = userID
					};
					var newNotification = new Notifications
					{
						ID = Guid.NewGuid(),
						New = true,
						Text = username + EnumUtils.ValueOf(NotificationsEnum.RideAccepted) + ride.ToPlace,
						UserID = userID
					};
					database.Notifications.Add(newNotification);
					database.RideUsers.Add(rideUser);
					database.SaveChanges();
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
			
		}

		/// <summary>
		/// Cancela sua presença em alguma carona que aceitou anteriormente
		/// </summary>
		/// <param name="userID">ID do usuário que deseja cancelar sua presença</param>
		/// <param name="rideID">ID da carona que o usuário deseja cancelar sua presença</param>
		/// <param name="userName">Username do usuário</param>
		public static void CancelRidePresence(Guid userID, Guid rideID, string userName)
		{
			DBConfigurations database = new DBConfigurations();
			var rideUser = (from rides in database.RideUsers where rides.UserID == userID && rides.RideID == rideID select rides).First();
			var ride = (from rides in database.Ride where rides.ID == rideID select rides).First();
			ride.RemainingSeats++;
			database.RideUsers.Remove(rideUser);
			database.SaveChanges();
			NotificationsManager.AddUserCancelRideNotification(userName, ride.ToPlace, ride.DonorID);
		}

		/// <summary>
		/// Método chamado quando o motorista deseja cancelar a carona que cadastrou anteriormente.
		/// Além de cancelar, notifica os usuários que aceitaram a carona que esta foi cancelada.
		/// </summary>
		/// <param name="rideID">ID da carona a ser cancelada</param>
		public static void CancelDonorRide(Guid rideID)
		{
			DBConfigurations database = new DBConfigurations();

			//Apaga carona
			var listaUsuariosCaronas = (from rideUsers in database.RideUsers where rideUsers.RideID == rideID select rideUsers).ToList();
			var listaUsuariosReserva = (from rideUsers in database.RidesRequest where rideUsers.RideID == rideID select rideUsers).ToList();

			var ride = (from rides in database.Ride where rides.ID == rideID select rides).First();
			var ridesUsers = (from rides in database.RideUsers where rides.RideID == rideID select rides).ToList();
			var ridesRequest = (from rides in database.RidesRequest where rides.RideID == rideID select rides).ToList();

			//Remove a carona
			database.Ride.Remove(ride);

			//Notifica lista de usuários que aceitaram a carona
			foreach (var userCarona in listaUsuariosCaronas)
			{
				var notification = new Notifications
				{
					ID = Guid.NewGuid(),
					New = true,
					Text = ride.DonorName + EnumUtils.ValueOf(NotificationsEnum.RideCancelled) + ride.ToPlace,
					UserID = userCarona.UserID
				};
				database.Notifications.Add(notification);
			}

			//Notifica lista de usuários que reservaram a carona
			foreach (var userCarona in listaUsuariosReserva)
			{
				var notification = new Notifications
				{
					ID = Guid.NewGuid(),
					New = true,
					Text = ride.DonorName + EnumUtils.ValueOf(NotificationsEnum.RideCancelled) + ride.ToPlace,
					UserID = userCarona.UserID
				};
				database.Notifications.Add(notification);
			}

			database.SaveChanges();
		}

		/// <summary>
		/// Retorna uma lista de caronas dada uma lista de caronas aceitas
		/// </summary>
		/// <param name="rideUsers">Lista de caronas aceitas</param>
		/// <returns>Lista de caronas</returns>
		public static List<Rides> GetRidesFromRideUsers(List<RideUsers> rideUsers)
		{
			DBConfigurations database = new DBConfigurations();
			var ridesList = new List<Rides>();
			foreach (var r in rideUsers)
			{
				var ride = (from rides in database.Ride where rides.ID == r.RideID select rides).First();
				ridesList.Add(ride);
			}
			return ridesList;
		}

		/// <summary>
		/// Retorna uma lista com todas as caronas com reserva solicitada pelo usuário
		/// </summary>
		/// <param name="userID">ID do usuário que solicitou a reserva</param>
		/// <returns>Lista de caronas reservadas</returns>
		public static List<RidesModel> GetRidesRequests(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();

			var ridesRequest = (from requests in database.RidesRequest where requests.UserID == userID select requests).ToList();

			var requestsID = ridesRequest.Select(ride => ride.RideID).ToList();
			var ridesList = (from rides in database.Ride where requestsID.Contains(rides.ID) select rides).ToList();

			return Conversor.ListRidesToListModel(ridesList);
		}
	}
}