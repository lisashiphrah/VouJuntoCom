using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using VouJuntoCom.DAO;

namespace VouJuntoCom.Models
{
    public class UserBankModel
    {
		public Guid ID { get; set; }

		public Guid BankID { get; set; }

		[Display(Name = "Banco")]
		public string Name { get; set; }

		[Display(Name = "Agência")]
		public string Agency { get; set; }

		[Display(Name = "Conta")]
		public string Account { get; set; }

    }
}