using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.Models
{
	/// <summary>
	/// Classe que representa uma instancia de mensagem do banco.
	/// </summary>
	public class Messages
	{
		public Guid ID { get; set; }
		public Guid SenderID { get; set; }
		public Guid ReceiverID { get; set; }
		public string SenderName { get; set; }
		public string ReceiverName { get; set; }
		public string Message { get; set; }
		public DateTime SendedDate { get; set; }
		public bool New { get; set; }
	}

	public class MessagesManager
	{
		/// <summary>
		/// Efetua o envio de uma mensagem a um usuário
		/// </summary>
		/// <param name="senderID">Guid do usuário que envioou a mensagem</param>
		/// <param name="senderName">Nome do usuário que enviou a mensagem</param>
		/// <param name="receiverName">Nome do recebedor da mensagem</param>
		/// <param name="receiverID">ID do recebedor da mensagem</param>
		/// <param name="message">Mensagem</param>
		public static void SendMessage(string senderID, string senderName, string receiverName, string receiverID, string message)
		{
			DBConfigurations database = new DBConfigurations();
			Messages m = new Messages
			{
				ID = Guid.NewGuid(),
				Message = message,
				New = true,
				ReceiverID = new Guid(receiverID),
				ReceiverName = receiverName,
				SendedDate = DateTime.Now,
				SenderID = new Guid(senderID),
				SenderName = senderName
			};

			database.Messages.Add(m);
			database.SaveChanges();
		}

		public static void MarkAsRead(Guid messageID)
		{
			DBConfigurations database = new DBConfigurations();
			var message = database.Messages.First(m => m.ID == messageID);
			message.New = false;
			database.SaveChanges();
		}

		public static void RemoveMessage(Guid messageID)
		{
			DBConfigurations database = new DBConfigurations();
			var message = database.Messages.First(m => m.ID == messageID);
			database.Messages.Remove(message);
			database.SaveChanges();
		}
	}
}