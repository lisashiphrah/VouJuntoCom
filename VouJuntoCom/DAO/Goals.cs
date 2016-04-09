using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa os troféus / metas a serem atingidas pelo usuário no sistema.
	/// </summary>
	public class Goals
	{
		public Guid ID { get; set; }
		public int Points { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}