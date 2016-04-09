using System;
using System.Collections.Generic;
using System.Data.Entity;
using VouJuntoCom.DAO;
using VouJuntoCom.Models;

namespace VouJuntoCom.Helpers
{
    /// <summary>
    /// Classe gerenciadora dos dados no banco.
    /// </summary>
	public class DBConfigurations : DbContext
    {
        #region Tabelas

        public DbSet<Users> Users { get; set; }
		public DbSet<UserBanks> UserBank { get; set; }
		public DbSet<Banks> Banks { get; set; }
		public DbSet<Cars> Cars { get; set; }
		public DbSet<UserCars> UserCars { get; set; }
		public DbSet<Friendships> Friendships { get; set; }
		public DbSet<VouJuntoCom.Models.Messages> Messages { get; set; }
		public DbSet<Rides> Ride { get; set; }
		public DbSet<Directions> Directions { get; set; }
		public DbSet<RideUsers> RideUsers { get; set; }
		public DbSet<Images> Images { get; set; }
		public DbSet<Notifications> Notifications { get; set; }
		public DbSet<RidesRequest> RidesRequest { get; set; }

        #endregion

		public DBConfigurations()
			: base("VouJunto_Database")
		{
			this.Configuration.ProxyCreationEnabled = true;
			this.Configuration.AutoDetectChangesEnabled = true;
		}
    }
}