using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using VouJuntoCom.DAO;
using System.Web;
using System.Web.Mvc;

namespace VouJuntoCom.Models
{
	/// <summary>
	/// Classe responsável por representar um usuário no sistema
	/// </summary>
	public class UserModel
	{
		private List<Notifications> listNotifications;
		private List<CarModel> cars;
		private List<Messages> messagesToMe;
		private List<Messages> messagesFromMe;
		private List<RidesModel> donorRides;
		private List<RidesModel> receiverRides;
		private List<UserModel> friendsRequests;
		private List<RidesRequest> ridesRequests;
		private List<RidesModel> openRequests;

		public UserModel()
		{
			this.BankAccount = new UserBankModel();
			this.Cars = new List<CarModel>();
		}

		public Guid ID { get; set; }

		//public int Image { get; set; }

		[Display(Name = "Nome Completo")]
		public string FullName { get; set; }

		[Display(Name = "Usuário")]
		public string Username { get; set; }

		[Display(Name = "Senha")]
		public string Password { get; set; }

		[Display(Name = "Data de Nascimento")]
		public DateTime Birth { get; set; }

		[Display(Name = "CPF")]
		public string CPF { get; set; }

		[Display(Name = "RG")]
		public string RG { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		[Display(Name = "Sexo")]
		public string Sex { get; set; }

		[Display(Name = "Pontuação")]
		public int Points { get; set; }

		[Display(Name = "Cidade")]
		public string City { get; set; }

		public string CreditCardNumber { get; set; }

		public bool Notifications { get; set; }

		[Display(Name = "Amigos")]
		public int QtdFriends { get; set; }

		[Display(Name = "Imagem de Perfil:")]
		public HttpPostedFileBase FileImage { get; set; }

		public FileContentResult FileContentResult { get; set; }

		public List<Notifications> ListNotifications
		{
			get
			{
				if (listNotifications == null) return new List<Notifications>();
				else return listNotifications;
			}
			set
			{
				listNotifications = value;
			}
		}

		public UserBankModel BankAccount { get; set; }

		public List<CarModel> Cars
		{
			get
			{
				if (cars == null) return new List<CarModel>();
				else return cars;
			}
			set
			{
				cars = value;
			}
		}

		public List<Messages> MessagesToMe
		{
			get
			{
				if (messagesToMe == null) return new List<Messages>();
				else return messagesToMe;
			}
			set
			{
				messagesToMe = value;
			}
		}

		public List<Messages> MessagesFromMe
		{
			get
			{
				if (messagesFromMe == null) return new List<Messages>();
				else return messagesFromMe;
			}
			set
			{
				messagesFromMe = value;
			}
		}

		public List<RidesModel> DonorRides
		{
			get
			{
				if (donorRides == null) return new List<RidesModel>();
				else return donorRides;
			}
			set
			{
				donorRides = value;
			}
		}

		public List<RidesModel> ReceiverRides
		{
			get
			{
				if (receiverRides == null) return new List<RidesModel>();
				else return receiverRides;
			}
			set
			{
				receiverRides = value;
			}
		}

		public List<UserModel> FriendsRequests
		{
			get
			{
				if (friendsRequests == null) return new List<UserModel>();
				else return friendsRequests;
			}
			set
			{
				friendsRequests = value;
			}
		}

		public List<RidesRequest> RidesRequests
		{
			get
			{
				if (ridesRequests == null) return new List<RidesRequest>();
				else return ridesRequests;
			}
			set
			{
				ridesRequests = value;
			}
		}

		public List<RidesModel> OpenRequests
		{
			get
			{
				if (openRequests == null) return new List<RidesModel>();
				else return openRequests;
			}
			set
			{
				openRequests = value;
			}
		}
	}
}
