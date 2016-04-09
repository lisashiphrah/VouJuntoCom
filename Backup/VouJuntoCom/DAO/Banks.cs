using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.DAO
{
    /// <summary>
    /// Representa a classe 'Banks' do banco de dados.
    /// </summary>
	public class Banks
	{
		public Guid ID { get; set; }
		public string Nome { get; set; }
	}

    public class BanksDB
    {
        /// <summary>
        /// Recupera todos os bancos disponíveis no banco de dados.
        /// </summary>
        /// <returns>Lista com todos os bancos disponíveis.</returns>
        public static IEnumerable<Banks> RetrieveAllBanks()
        {
            DBConfigurations database = new DBConfigurations();
            return database.Banks;
        }
    }

}