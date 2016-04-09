using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.Models
{
	public class Notifications
	{
		public Guid ID { get; set; }
		public Guid UserID { get; set; }
		public string Text { get; set; }
		public bool New { get; set; }
	}

	public class NotificationsManager
	{
		/// <summary>
		/// Insere uma nova notificação de que algum contato aceitou a solicitação de amizade para determinado usuário
		/// </summary>
		/// <param name="userID">Usuário que vai receber a notificação</param>
		/// <param name="acceptedFriend">Nome do usuário que aceitou a amizade</param>
		public static void AddFriendAcceptedNotification(Guid userID, string acceptedFriend)
		{
			DBConfigurations database = new DBConfigurations();
			Notifications not = new Notifications
			{
				ID = Guid.NewGuid(),
				UserID = userID,
				Text = acceptedFriend + EnumUtils.ValueOf(NotificationsEnum.FriendAccepted),
				New = true
			};
			NotificationsManager.SendEmailNotification(userID, acceptedFriend + EnumUtils.ValueOf(NotificationsEnum.FriendAccepted));
			database.Notifications.Add(not);
			database.SaveChanges();
		}

		/// <summary>
		/// Envia uma notificação de aceitação ou negação da carona
		/// </summary>
		/// <param name="userID">ID do usuário que solicita a carona</param>
		/// <param name="driverName">Nome do motorista</param>
		/// <param name="accepted">Controla se é uma mensagem de que o motorista aceitou ou cancelou a carona</param>
		public static void AddRideRequestNotification(Guid userID, string driverName, bool accepted)
		{
			DBConfigurations database = new DBConfigurations();
			string text = "";

			if (accepted)
			{
				text = driverName + " aceitou sua solicitação de carona.";
			}
			else
			{
				text = driverName + " rejeitou sua solicitação de carona.";
			}

			Notifications not = new Notifications
			{
				ID = Guid.NewGuid(),
				UserID = userID,
				Text = text,
				New = true
			};
			NotificationsManager.SendEmailNotification(userID, text);
			database.Notifications.Add(not);
			database.SaveChanges();
		}

		/// <summary>
		/// Envia mensagem para o motorista que determinado usuário cancelou sua carona
		/// </summary>
		/// <param name="userName">Nome do usuário que cancelou</param>
		/// <param name="ridePlace">Nome do destino do cancelamento</param>
		/// <param name="driverID">ID do motorista</param>
		public static void AddUserCancelRideNotification(string userName, string ridePlace, Guid driverID)
		{
			DBConfigurations database = new DBConfigurations();
			Notifications not = new Notifications
			{
				ID = Guid.NewGuid(),
				UserID = driverID,
				Text = userName + " cancelou sua presença na carona para " + ridePlace,
				New = true
			};
			NotificationsManager.SendEmailNotification(driverID, userName + " cancelou sua presença na carona para " + ridePlace);
			database.Notifications.Add(not);
			database.SaveChanges();
		}

		/// <summary>
		/// Retorna todas as novas notificações baseadas no usuário recebido
		/// </summary>
		/// <param name="userId">ID do usuário a ser retornadas as notificações</param>
		/// <returns>Lista com todas as notificações não vistas pelo usuário</returns>
		public static List<Notifications> GetUserNotifications(Guid userId)
		{
			DBConfigurations database = new DBConfigurations();
			var listNotifications = (from notification in database.Notifications
									 where ((notification.UserID == userId) &&
									 (notification.New == true))
									 select notification).ToList();
			return listNotifications;
		}

		/// <summary>
		/// Marca todas as notificações como vistas
		/// </summary>
		/// <param name="userID">ID do usuário com a notificação</param>
		public static void RemoveNotifications(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var listNotifications = (from notification in database.Notifications
									 where ((notification.UserID == userID) &&
									 (notification.New == true))
									 select notification).ToList();

			foreach (var not in listNotifications)
			{
				var foundNot = database.Notifications.First(i => i.ID == not.ID);
				foundNot.New = false;
			}
			database.SaveChanges();
		}

		/// <summary>
		/// Envia um email com a notificação se o usuário desejou o serviço
		/// </summary>
		/// <param name="userID">Usuário para enviar o email</param>
		/// <param name="message">Mensagem do email</param>
		public static void SendEmailNotification(Guid userID, string messageText)
		{
			DBConfigurations database = new DBConfigurations();

			var user = (from users in database.Users where users.ID == userID select users).First();
			//Apenas envia email se usuário solicitou
			if (user.Notifications == true)
			{
				var fromAddress = new MailAddress("voujuntocom@gmail.com", "[VouJunto.com - Notificação]");
				var toAddress = new MailAddress(user.Email.ToString());
				var fromPassword = "a1234@45";
				var subject = "[VouJunto.com - Notificação]";
				var body = messageText;

				try
				{
					var smtp = new SmtpClient
					{
						Host = "smtp.gmail.com",
						Port = 587,
						EnableSsl = true,
						DeliveryMethod = SmtpDeliveryMethod.Network,
						UseDefaultCredentials = false,
						Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
					};
					using (var message = new MailMessage(fromAddress, toAddress)
					{
						Subject = subject,
						Body = body
					})
						smtp.Send(message);
				}
				catch (Exception)
				{
					//TODO: TRATAR
				}
			}
		}
	}
}
