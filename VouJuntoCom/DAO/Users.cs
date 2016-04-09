using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Models;
using VouJuntoCom.Helpers;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Web.Mvc;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que mapeia a tabela User do banco.
	/// </summary>
	public class Users
	{
		public Guid ID { get; set; }
		public Guid UserBankID { get; set; }
		public string FullName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public DateTime Birth { get; set; }
		public string CPF { get; set; }
		public string RG { get; set; }
		public string Email { get; set; }
		public string Sex { get; set; }
		public bool Notifications { get; set; }
		public int Points { get; set; }
		public string City { get; set; }
		public int QtdFriends { get; set; }
		public string CreditCardNumber { get; set; }

	}

	/// <summary>
	/// Efetua operações de CRUD relacionadas ao usuário
	/// </summary>
	public class UserManager
	{
		/// <summary>
		/// Adiciona uma nova instancia de usuário no banco de dados se esta não existir.
		/// Verifica se nome de usuário e CPF já estão em uso antes de adicionar.
		/// </summary>
		/// <param name="user">UserModel a ser adicionado</param>
		/// <returns>ID do usuário adicionado, ou uma Guid vazia em caso de erro de inserção.</returns>
		public static Guid? InsertUser(UserModel user, out ErrorEnum errorEnum)
		{
			if (!UserManager.IsUserLoginExist(user.Username))
			{
				if (!UserManager.CPFUserExist(user.CPF))
				{
					try
					{
						//Encripta a senha
						string encryptPass = Security.Encrypt(user.Password);
						user.Password = encryptPass;

						//Define informações do banco
						UserBanks userBankDAO = Conversor.UserBankModelToDTO(user.BankAccount);
						userBankDAO.ID = Guid.NewGuid();

						//Define configurações do usuário
						Users userDAO = Conversor.UserModelToDTO(user);
						userDAO.ID = Guid.NewGuid();
						userDAO.UserBankID = userBankDAO.ID;
						userDAO.Notifications = user.Notifications;

						errorEnum = ErrorEnum.NoErrors;
						var inserted = UserManager.AddUser(userDAO, userBankDAO);

						//Se inseriu com sucesso, insere imagem de perfil selecionada
						if ((inserted != Guid.Empty) && (inserted != null))
						{
							ImagesManager.SaveImage(user.FileImage, inserted);
						}
						return inserted;

					}
					catch (Exception ex)
					{
						errorEnum = ErrorEnum.ExceptionError;
						return null;
					}
				}
				else
				{
					errorEnum = ErrorEnum.ExistentCPF;
					return null;
				}
			}
			else
			{
				errorEnum = ErrorEnum.ExistentUsername;
				return null;
			}
		}

		/// <summary>
		/// Adiciona determinado usuário no banco de dados
		/// </summary>
		/// <param name="userDAO">DAO a ser adicionada no banco</param>
		/// <param name="userBankDAO">Informação bancária do usuário</param>
		/// <returns></returns>
		private static Guid AddUser(Users userDAO, UserBanks userBankDAO)
		{
			DBConfigurations database = new DBConfigurations();
			database.Users.Add(userDAO);
			database.UserBank.Add(userBankDAO);
			database.SaveChanges();
			return userDAO.ID;
		}

		/// <summary>
		/// Verifica se a ID selecionada para login não existe na base de dados
		/// </summary>
		/// <param name="loginID">Nome de usuário selecionado</param>
		/// <returns>True se usuário existir na base, False se não existir</returns>
		public static bool IsUserLoginExist(string userID)
		{
			DBConfigurations database = new DBConfigurations();
			bool exists = (from user in database.Users where user.Username == userID select user).Any();
			return exists;
		}

		/// <summary>
		/// Verifica se o CPF selecionado não existe na base de dados
		/// </summary>
		/// <param name="userCPF">CPF do usuário selecionado</param>
		/// <returns>True se o CPF existir e false se não existir</returns>
		public static bool CPFUserExist(string userCPF)
		{
			DBConfigurations database = new DBConfigurations();
			bool exists = (from user in database.Users where user.CPF == userCPF select user).Any();
			return exists;
		}

		/// <summary>
		/// Efetua o login do usuário e senha caso estes existam
		/// </summary>
		/// <param name="user">Usuário a ser logado</param>
		/// <param name="password">Senha inserida</param>
		/// <param name="message">Mensagem de erro para o controle</param>
		/// <returns></returns>
		public static UserModel LoginUser(string username, string password, out ErrorEnum errorEnum)
		{
			DBConfigurations database = new DBConfigurations();

			try
			{
				var encryptPassword = Security.Encrypt(password);
				var userDTO = (from user in database.Users where user.Username == username && user.Password == encryptPassword select user).First();
				if (userDTO != null)
				{
					var userModel = Conversor.UserDTOToModel(userDTO);
					var bankAccountDTO = (from userBank in database.UserBank where userBank.ID == userDTO.UserBankID select userBank).First();
					var userBankAccount = Conversor.UserBankToModel(bankAccountDTO);
					var bankDTO = (from banks in database.Banks where banks.ID == userBankAccount.BankID select banks).First();
					userBankAccount.Name = bankDTO.Nome;

					userModel.BankAccount = userBankAccount;
					errorEnum = ErrorEnum.NoErrors;
					return userModel;
				}
				else
				{
					errorEnum = ErrorEnum.InvalidUsername;
					return null;
				}
			}
			catch (Exception ex)
			{
				errorEnum = ErrorEnum.InvalidUsername;
				return null;
			}
		}

		/// <summary>
		/// Realiza consulta para trazer os dados do usuário no banco de dados e armazenar o resultado em uma
		/// variável de seção.
		/// </summary>
		/// <param name="userID">ID do usuário logado (Guid)</param>
		/// <param name="error">Enum de retorno de erros</param>
		/// <returns></returns>
		public static UserModel RetrieveUser(Guid userID, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;

			try
			{
				var userDTO = (from user in database.Users where user.ID == userID select user).First();
				var userModel = Conversor.UserDTOToModel(userDTO);

				#region Messages

				var messagesDTO = UserManager.GetMessages(userModel.ID);
				userModel.MessagesFromMe = new List<Messages>();
				userModel.MessagesToMe = new List<Messages>();

				foreach (var message in messagesDTO)
				{
					if (message.SenderID == userModel.ID)
					{
						userModel.MessagesFromMe.Add(message);
					}
					else
					{
						userModel.MessagesToMe.Add(message);
					}
				}
				#endregion

				#region Rides

				userModel.DonorRides = RidesManager.GetDonorRides(userModel.ID);
				userModel.ReceiverRides = RidesManager.GetReceiverRides(userModel.ID);
				userModel.OpenRequests = RidesManager.GetRidesRequests(userModel.ID);

				#endregion

				#region BankAccount

				var userBankAccount = UserBankManager.GetUserBankAccount(userDTO.UserBankID);
				userModel.BankAccount.Account = userBankAccount.Account;
				userModel.BankAccount.Agency = userBankAccount.Agency;
				userModel.BankAccount.BankID = userBankAccount.BankID;
				userModel.BankAccount.ID = userBankAccount.ID;

				#endregion

				#region Image

				userModel.FileContentResult = ImagesManager.RetrieveImage(userModel.ID);

				#endregion

				#region Friends Requests

				userModel.FriendsRequests = FriendshipManager.GetFriendsRequests(userModel.ID);

				#endregion

				#region ListNotifications

				userModel.ListNotifications = NotificationsManager.GetUserNotifications(userModel.ID);
				userModel.RidesRequests = RidesRequestManager.GetAllRequestsByDriver(userModel.ID);

				#endregion

				return userModel;
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return null;
			}
		}

		/// <summary>
		/// Retorna lista com todas as mensagens do usuário
		/// </summary>
		/// <param name="userID">ID do usuário</param>
		/// <returns>Lista com todas as mensagens do usuário</returns>
		private static List<Messages> GetMessages(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var messagesDTO = (from message in database.Messages
							   where (message.SenderID == userID || message.ReceiverID == userID)
							   select message).ToList();

			return messagesDTO;
		}

		/// <summary>
		/// Atualiza os dados do usuário
		/// </summary>
		/// <param name="model">Model do usuário logado para mudança de dados</param>
		/// <param name="error">Controle de erro</param>
		/// <returns>Usuário atualizado</returns>
		public static bool UpdateUser(UserModel model, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;

			try
			{
				var userToEdit = database.Users.First(i => i.ID == model.ID);

				// Atualiza usuário
				userToEdit.Birth = model.Birth;
				userToEdit.City = model.City;
				userToEdit.CPF = model.CPF;
				userToEdit.Email = model.Email;
				userToEdit.FullName = model.FullName;
				userToEdit.RG = model.RG;
				userToEdit.Sex = model.Sex;
				userToEdit.City = model.City;

				//Atualiza senha
				if (model.Password != null)
				{
					string encryptPass = Security.Encrypt(model.Password);
					userToEdit.Password = encryptPass;
				}

				//Atualiza imagem
				if (model.FileImage != null)
				{
					//Remove imagem antiga e adiciona a nova
					ImagesManager.RemoveImage(userToEdit.ID);
					ImagesManager.SaveImage(model.FileImage, userToEdit.ID);
				}

				//Atualiza conta
				var userBankToEdit = database.UserBank.First(i => i.ID == userToEdit.UserBankID);

				userBankToEdit.Account = model.BankAccount.Account;
				userBankToEdit.Agency = model.BankAccount.Agency;
				userBankToEdit.BankID = model.BankAccount.BankID;

				userToEdit.Notifications = model.Notifications;

				database.SaveChanges();
				return true;

			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return false;
			}
		}

		/// <summary>
		/// Verifica se senha digitada corresponde
		/// </summary>
		/// <param name="Password">Senha inserida</param>
		/// <returns>True se verdadeiro, False se falso</returns>
		public static bool ValidatePassword(string password, Guid userID, out ErrorEnum errorEnum)
		{
			DBConfigurations database = new DBConfigurations();
			errorEnum = ErrorEnum.NoErrors;
			try
			{
				var encryptPassword = Security.Encrypt(password);
				var userDTO = (from user in database.Users where user.ID == userID && user.Password == encryptPassword select user).First();
				if (userDTO != null)
				{
					return true;
				}
				else
				{
					errorEnum = ErrorEnum.InvalidPassword;
					return false;
				}

			}
			catch (Exception ex)
			{
				errorEnum = ErrorEnum.InvalidPassword;
				return false;
			}
		}

		/// <summary>
		/// Recupera apenas algumas informações do usuário
		/// </summary>
		/// <param name="userID">ID do usuário a ter informações recuperadas</param>
		/// <param name="error">Controle de erro</param>
		/// <returns>Usuário selecionado</returns>
		public static UserModel RetrieveUserLight(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();

			var userDTO = (from user in database.Users where user.ID == userID select user).First();
			var userModel = Conversor.UserDTOToModel(userDTO);
			userModel.FileContentResult = ImagesManager.RetrieveImage(userModel.ID);
			return userModel;
		}

		/// <summary>
		/// Efetua a remoção do usuário no sistema
		/// </summary>
		/// <param name="userID">Usuário a ser removido</param>
		/// <returns>true se removeu com sucesso</returns>
		public static bool RemoveAccount(Guid userID)
		{
			return true;
		}

		/// <summary>
		/// Retorna o nome dado o id do usuário
		/// </summary>
		/// <param name="userID">ID do usuário a ter o nome pesquisado</param>
		/// <returns>Nome do usuário</returns>
		public static string RetrieveNameById(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var name = (from user in database.Users where user.ID == userID select user).First().FullName;
			return name;
		}

		/// <summary>
		/// Recupera todas as informações relacionadas ao histórico do usuário.
		/// </summary>
		/// <param name="userID">Identificador do usuário</param>
		/// <param name="year">Ano para efetuar a pesquisa</param>
		/// <returns>HistoricModel com todas as identificações do usuário</returns>
		public static HistoricModel RetrieveUserHistory(Guid userID, int year)
		{
			DBConfigurations database = new DBConfigurations();

			var offeredRides = (from rides in database.Ride where rides.DonorID == userID && rides.TimeFrom.Year == year select rides).ToList();
			var receivedRidesUsers = (from rideUsers in database.RideUsers where rideUsers.UserID == userID select rideUsers).ToList();
			var receivedAllRides = RidesManager.GetRidesFromRideUsers(receivedRidesUsers);
			
			//filtra para apenas o ano recebido
			var receivedRides = new List<Rides>();
			foreach (var ride in receivedAllRides)
			{
				if (ride.TimeFrom.Year == year)
				{
					receivedRides.Add(ride);
				}
			}

			var drivedDistance = offeredRides.Select(distance => distance.Distance).Sum();
			var acceptedDistance = receivedRides.Select(distance => distance.Distance).Sum();

			double totalGain = 0.0;
			//calcula total ganho
			foreach (var offered in offeredRides)
			{
				if (offered.Seats != offered.RemainingSeats)
				{
					int aceitaram = offered.Seats - offered.RemainingSeats;
					totalGain = totalGain + Convert.ToDouble(offered.Price * aceitaram);
				}
			}

			double totalPaid = 0.0;
			//calcula total pago
			foreach (var received in receivedRides)
			{
				totalPaid = totalPaid + Convert.ToDouble(received.Price);
			}

			var historicModel = new HistoricModel
			{
				AcceptedDistance = acceptedDistance,
				DrivedDistance = drivedDistance,
				OfferedRides = Conversor.ListRidesToListModel(offeredRides),
				ReceivedRides = Conversor.ListRidesToListModel(receivedRides),
				TotalGain = totalGain,
				TotalPaid = totalPaid
			};

			//monta LucroAnual e oferecidas
			foreach (var offered in offeredRides)
			{
				int mes = offered.TimeFrom.Month;
				double lucroNoMes = historicModel.LucroAnualOferecidas[mes];
				lucroNoMes = lucroNoMes + Convert.ToDouble(offered.Price);
				historicModel.LucroAnualOferecidas[mes] = lucroNoMes;

				int caronasOferecidas = historicModel.CaronasAnuaisOferecidas[mes];
				caronasOferecidas++;
				historicModel.CaronasAnuaisOferecidas[mes] = caronasOferecidas;
			}

			//monta pagamento anual e aceitas
			foreach (var received in receivedRides)
			{
				int mes = received.TimeFrom.Month;
				double lucroNoMes = historicModel.LucroAnualAceitas[mes];
				lucroNoMes = lucroNoMes + Convert.ToDouble(received.Price);
				historicModel.LucroAnualAceitas[mes] = lucroNoMes;

				int caronasAceitas = historicModel.CaronasAnuaisAceitas[mes];
				caronasAceitas++;
				historicModel.CaronasAnuaisAceitas[mes] = caronasAceitas;
			}

			return historicModel;
		}
	}
}