using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;
using VouJuntoCom.Models;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa a tabela Cars do banco.
	/// </summary>
	public class Cars
	{
		public Guid ID { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string Color { get; set; }
		public string RENAVAM { get; set; }
		public int? Digits { get; set; }
		public bool ArConditioning { get; set; }
		public bool Radio { get; set; }
		public bool Smoke { get; set; }
		public bool Pet { get; set; }
	}

	/// <summary>
	/// Efetua operações de CRUD relacionadas aos veículos
	/// </summary>
	public class CarsManager
	{
		/// <summary>
		/// Retorna uma lista de carros associada a determinado usuário
		/// </summary>
		/// <param name="userID">ID do usuário logado</param>
		/// <param name="error">Flag de controle de erro</param>
		/// <returns>Lista de carros cadastrados pelo usuário</returns>
		public static List<CarModel> RetrieveUserCars(Guid userID, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;

			try
			{
				var listUsersCars = (from userCars in database.UserCars where userCars.UserID == userID select userCars).ToList();
				var listCars = new List<CarModel>();

				foreach (UserCars userCars in listUsersCars)
				{
					var newCar = (from cars in database.Cars where cars.ID == userCars.CarID select cars).First();
					listCars.Add(Conversor.CarsToModel(newCar));
				}
				return listCars;
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return null;
			}
		}

		/// <summary>
		/// Insere o carro associando-o a um usuário
		/// </summary>
		/// <param name="model">CarModel a ser associado e inserido na base</param>
		/// <param name="userID">ID do usuário que inseriu o veículo</param>
		/// <param name="error">Controle de erro</param>
		/// <returns>True se inserido com sucesso ou false caso contrário</returns>
		public static bool InsertCar(CarModel model, Guid userID, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;
			try
			{
				var car = Conversor.CarModelToDTO(model);
				car.ID = Guid.NewGuid();
				database.Cars.Add(car);
				database.UserCars.Add(new UserCars { CarID = car.ID, UserID = userID });
				database.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return false;
			}

		}

		/// <summary>
		/// Remove o carro com ID recebido por parametro da base de veículos
		/// </summary>
		/// <param name="carID">ID do carro a ser removido</param>
		/// <param name="result">Controle de erro</param>
		/// <returns>True se removido com sucesso, False caso contrário</returns>
		public static bool RemoveCar(Guid carID, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;

			try
			{
				var carToRemove = (from car in database.Cars where car.ID == carID select car).First();
				database.Cars.Remove(carToRemove);
				database.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return false;
			}
		}

		/// <summary>
		/// Edita carro passado como parâmetro
		/// </summary>
		/// <param name="model">CarModel para modificação</param>
		/// <param name="result">Controle de erro</param>
		/// <returns>True se removido com sucesso, False caso contrário</returns>
		public static bool EditCar(CarModel model, out ErrorEnum error)
		{
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;

			try
			{
				var carToEdit = database.Cars.First(i => i.ID == model.ID);
				
				//Update
				carToEdit.ArConditioning = model.ArConditioning;
				carToEdit.Color = model.Color;
				carToEdit.Digits = model.Digits;
				carToEdit.Make = model.Make;
				carToEdit.Model = model.Modelo;
				carToEdit.Pet = model.Pet;
				carToEdit.Radio = model.Radio;
				carToEdit.RENAVAM = model.RENAVAM;
				carToEdit.Smoke = model.Smoke;

				database.SaveChanges();

				return true;
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return false;
			}
		}

		/// <summary>
		/// Retorna carro com ID passado por parâmetro
		/// </summary>
		/// <param name="carID">ID do carro a ser pesquisado</param>
		/// <returns>CarModel respectivo</returns>
		public static CarModel RetrieveCar(Guid carID)
		{
			DBConfigurations database = new DBConfigurations();
			var carDTO = (from car in database.Cars where car.ID == carID select car).First();
			return Conversor.CarsToModel(carDTO);
		}
	}
}