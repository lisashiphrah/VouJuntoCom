using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using VouJuntoCom.DAO;

namespace VouJuntoCom.Models
{
    /// <summary>
    /// Classe responsável por representar um usuário no sistema
    /// </summary>
    public class UserModel
    {
        public UserModel()
        {
            this.BankAccount = new UserBankModel();
            this.Cars = new List<CarModel>();
        }

        public Guid ID { get; set; }

        //public int Image { get; set; }

        [Display(Name="Nome Completo")]
        public string FullName { get; set; }

        [Display(Name="Usuário")]
        public string Username { get; set; }

        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name="Data de Nascimento")]
        public DateTime Birth { get; set; }

        [Display(Name="CPF")]
        public string CPF { get; set; }

        [Display(Name = "RG")]
        public string RG { get; set; }

        [Display(Name="Email")]
        public string Email { get; set; }

        [Display(Name = "Sexo")]
        public string Sex { get; set; }

		public bool Notifications { get; set; }

        public UserBankModel BankAccount { get; set; }

        public IEnumerable<CarModel> Cars { get; set; }
    }
}
