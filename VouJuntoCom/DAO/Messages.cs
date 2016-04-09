using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VouJuntoCom.DAO
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
		public string Title { get; set; }
		public string Message { get; set; }
	}
}