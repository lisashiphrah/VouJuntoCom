using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Models;
using VouJuntoCom.DAO;

namespace VouJuntoCom.Helpers
{
    /// <summary>
    /// Classe responsavel por converter um Model para DTO e vice-versa.
    /// </summary>
    public class Conversor
    {
        #region User
        /// <summary>
        /// Efetua a conversão de um tipo User (DTO) para um tipo UserModel
        /// </summary>
        /// <param name="userDTO">DTO do usuário</param>
        /// <returns>Model com os dados da DTO</returns>
        public static UserModel UserDTOToModel(Users userDTO)
        {
            var userModel = new UserModel
            {
                Birth = userDTO.Birth,
                CPF = userDTO.CPF,
                Email = userDTO.Email,
                FullName = userDTO.FullName,
                ID = userDTO.ID,
                Password = userDTO.Password,
                RG = userDTO.RG,
                Sex = userDTO.Sex,
                Username = userDTO.Username,
				Points = userDTO.Points,
				City = userDTO.City,
				QtdFriends = userDTO.QtdFriends,
				CreditCardNumber = userDTO.CreditCardNumber
            };
            return userModel;
        }

        /// <summary>
        /// Mapeia propriedades de um objeto UserModel para objeto de banco
        /// </summary>
        /// <param name="dao">Model a ser mapeado</param>
        public static Users UserModelToDTO(UserModel userModel)
        {
            Users dao = new Users
            {
                Birth = userModel.Birth,
                CPF = userModel.CPF,
                Email = userModel.Email,
                FullName = userModel.FullName,
                ID = userModel.ID,
                Password = userModel.Password,
                RG = userModel.RG,
                Sex = userModel.Sex,
                Username = userModel.Username,
				Points = userModel.Points,
				City = userModel.City,
				QtdFriends = userModel.QtdFriends,
				CreditCardNumber = userModel.CreditCardNumber
            };
            return dao;
        }

		/// <summary>
		/// Retorna uma lista de UserModels para determinada lista de Users dada.
		/// </summary>
		/// <param name="listUsers"></param>
		/// <returns></returns>
		public static List<UserModel> ListUserToModel(List<Users> listUsers)
		{
			var listUserModel = new List<UserModel>();
			foreach (var user in listUsers)
			{
				var newModel = UserDTOToModel(user);
				listUserModel.Add(newModel);
			}
			return listUserModel;
		}
        #endregion

        #region Banks
        /// <summary>
        /// Efetua a conversão de um tipo UserBankModel  para um tipo UserBank (DTO)
        /// </summary>
        /// <returns>UserBank com os dados da model</returns>
        public static UserBanks UserBankModelToDTO(UserBankModel bankModel)
        {
            UserBanks dao = new UserBanks
            {
                Account = bankModel.Account,
                Agency = bankModel.Agency,
                BankID = bankModel.BankID,
                ID = bankModel.ID
            };
            return dao;
        }

		public static UserBankModel UserBankToModel(UserBanks dto)
		{
			UserBankModel model = new UserBankModel
			{
				Account = dto.Account,
				Agency = dto.Agency,
				BankID = dto.BankID,
				ID = dto.ID
			};
			return model;
		}
        #endregion

		#region Car

		/// <summary>
		/// Efetua a conversão de um tipo CarDTO para um tipo CarModel
		/// </summary>
		/// <param name="carDTO">DTO a ser convertido</param>
		/// <returns>Resultado da conversão.</returns>
		public static CarModel CarsToModel(Cars carDTO)
		{
			CarModel model = new CarModel
			{
				ArConditioning = carDTO.ArConditioning,
				Color = carDTO.Color,
				Digits = carDTO.Digits,
				ID = carDTO.ID,
				Make = carDTO.Make,
				Modelo = carDTO.Model,
				Pet = carDTO.Pet,
				Radio = carDTO.Radio,
				RENAVAM = carDTO.RENAVAM,
				Smoke = carDTO.Smoke
			};
			return model;
		}

		/// <summary>
		/// Efetua a conversão de um tipo CarModel para um tipo CarDTO
		/// </summary>
		/// <param name="model">Model a ser convertido</param>
		/// <returns>Model convertido</returns>
		public static Cars CarModelToDTO(CarModel model)
		{
			Cars dto = new Cars
			{
				ArConditioning = model.ArConditioning,
				Color = model.Color,
				Digits = model.Digits,
				ID = model.ID,
				Make = model.Make,
				Model = model.Modelo,
				Pet = model.Pet,
				Radio = model.Radio,
				RENAVAM = model.RENAVAM,
				Smoke = model.Smoke
			};
			return dto;
		}

		#endregion

		#region Rides

		/// <summary>
		/// Retorna um tipo Ride DTO a partir do seu model
		/// </summary>
		/// <param name="model">Model a ser convertido</param>
		/// <returns>DTO de Ride</returns>
		public static Rides RidesModelToDTO(RidesModel model)
		{
			Rides dto = new Rides
			{
				Car_ID = model.Car.ID,
				DonorID = model.DonorID,
				DonorName = model.DonorName,
				FromPlace = model.FromPlace,
				GiveUpTolerance = model.GiveUpTolerance,
				ID = model.ID,
				Message = model.Message,
				Price = model.Price,
				Seats = (int)model.Seats,
				TimeFrom = (DateTime)model.TimeFrom,
				ToPlace = model.ToPlace,
				TrunkSize = model.TrunkSize,
				RemainingSeats = model.RemainingSeats,
				Distance = model.Distance
			};
			return dto;
		}

		/// <summary>
		/// Retorna um tipo Model a partir do DTO de Rides
		/// </summary>
		/// <param name="dto">DTO a ser convertido</param>
		/// <returns>RidesModel respectivo</returns>
		public static RidesModel RidesToModel(Rides dto)
		{
			RidesModel model = new RidesModel
			{
				Car = CarsManager.RetrieveCar(dto.Car_ID),
				DonorID = dto.DonorID,
				DonorName = dto.DonorName,
				FromPlace = dto.FromPlace,
				GiveUpTolerance = dto.GiveUpTolerance,
				ID = dto.ID,
				Message = dto.Message,
				Price = dto.Price,
				Seats = dto.Seats,
				TimeFrom = dto.TimeFrom,
				ToPlace = dto.ToPlace,
				TrunkSize = dto.TrunkSize,
				RemainingSeats = dto.RemainingSeats,
				Distance = dto.Distance
			};
			return model;
		}

		/// <summary>
		/// Recebida uma lista de Rides, retorna uma lista de RidesModel
		/// </summary>
		/// <param name="listRides">Lista de rides</param>
		/// <returns>Lista de rides convertida para lista de ridesModel</returns>
		public static List<RidesModel> ListRidesToListModel(List<Rides> listRides)
		{
			var listRidesModel = new List<RidesModel>();
			foreach (var ride in listRides)
			{
				var newModel = Conversor.RidesToModel(ride);
				listRidesModel.Add(newModel);
			}

			return listRidesModel;
		}
		#endregion
	}
}