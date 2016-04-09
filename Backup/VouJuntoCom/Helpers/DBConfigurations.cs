using System;
using System.Collections.Generic;
using System.Data.Entity;
using VouJuntoCom.DAO;

namespace VouJuntoCom.Helpers
{
    /// <summary>
    /// Classe gerenciadora dos dados no banco.
    /// </summary>
	public class DBConfigurations : DbContext
    {
        #region Tabelas

        public DbSet<User> User { get; set; }
		public DbSet<UserBank> UserBank { get; set; }
		public DbSet<Banks> Banks { get; set; }
		public DbSet<Car> Car { get; set; }

        #endregion

		public DBConfigurations()
			: base("VouJunto_Database")
		{
			this.Configuration.ProxyCreationEnabled = true;
			this.Configuration.AutoDetectChangesEnabled = true;
		}
    }
}