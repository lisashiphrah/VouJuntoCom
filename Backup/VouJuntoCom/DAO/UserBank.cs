using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace VouJuntoCom.DAO 
{
	/// <summary>
	/// Classe que representa a tabela BankAccount do banco
	/// </summary>
	public class UserBank 
	{
		public Guid ID { get; set; }
		public Guid BankID { get; set; }
		public string Agency { get; set; }
		public string Account { get; set; }
	}
}
