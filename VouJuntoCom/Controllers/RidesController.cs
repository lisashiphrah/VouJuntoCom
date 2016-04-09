using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VouJuntoCom.DAO;
using VouJuntoCom.Helpers;
using VouJuntoCom.Models;

namespace VouJuntoCom.Controllers
{
    public class RidesController : Controller
    {
		/// <summary>
		/// Renderiza view de criação de nova carona
		/// </summary>
		/// <returns>View de criação de carona</returns>
		[HttpGet]
		public ActionResult CreateRide()
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			var newRide = new RidesModel
			{
				DonorID = userModel.ID,
				DonorName = userModel.FullName
			};
			return View("CreateRide", newRide);
		}

		/// <summary>
		/// Post para criação de uma nova carona
		/// </summary>
		/// <param name="ride">Model carona com dados preenchidos</param>
		/// <param name="timeTo">Horário de saida da carona</param>
		/// <param name="timeFrom">Horário de chegada da carona</param>
		/// <param name="dateTo">Data de saida da carona</param>
		/// <param name="dateFrom">Data de chegada da carona</param>
		/// <param name="selectedCar">Veículo selecionado</param>
		/// <param name="steps">Caminho armazenado no mapa</param>
		/// <param name="price">Preço total da carona</param>
		/// <returns>View com lista de caronas cadastradas.</returns>
		[HttpPost]
		public ActionResult CreateRide(RidesModel ride, string timeFrom, string dateFrom, string selectedCar, string steps, string price, string distance)
		{
			ErrorEnum error;

			//Mantem sessão
			var userModel  = UserManager.RetrieveUser(ride.DonorID, out error);
			
			var datetimeFrom = new DateTime(Int32.Parse(dateFrom.Substring(6, 4)),
									Int32.Parse(dateFrom.Substring(3, 2)), Int32.Parse(dateFrom.Substring(0, 2)),
									Int32.Parse(timeFrom.Substring(0, 2)), Int32.Parse(timeFrom.Substring(3, 2)), 0);

			ride.Car = new CarModel { ID = new Guid(selectedCar) };
			ride.Price = Convert.ToDecimal(price);
			ride.Direction = new DirectionsModel(steps);
			ride.TimeFrom = datetimeFrom;
			ride.Distance = float.Parse(distance, CultureInfo.InvariantCulture);

			var guid = RidesManager.CreateRide(ride, out error);
			//TODO: Mensagem de Criação
			ViewBag.InsertedRide = guid;
			return RedirectToAction("Index","Home");
		}

		/// <summary>
		/// Renderiza tela de visualização de caronas publicadas
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ViewRides()
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			
			return View(RidesManager.RetrieveFriendsRides((UserManager.RetrieveUser(new Guid(User.Identity.Name), out error)).ID, null, null));
		}

		/// <summary>
		/// Filtra as caronas pela data inicial e final recebidas
		/// </summary>
		/// <param name="txtDataInicial">Data inicial para pesquisa</param>
		/// <param name="txtDataFinal">Data final para pesquisa</param>
		/// <returns>Lista com todas as caronas dentro do intervalo solicitado</returns>
		[HttpGet]
		public ActionResult ViewRidesFilter(string txtDataInicial, string txtDataFinal)
		{
			ErrorEnum error;
			var userModel  = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			

			int dayInit = Int32.Parse(txtDataInicial.Substring(0, 2));
			int monthInit = Int32.Parse(txtDataInicial.Substring(3, 2));
			int yearInit = Int32.Parse(txtDataInicial.Substring(6, 4));

			int dayEnd = Int32.Parse(txtDataFinal.Substring(0, 2));
			int monthEnd = Int32.Parse(txtDataFinal.Substring(3, 2));
			int yearEnd = Int32.Parse(txtDataFinal.Substring(6, 4));

			var initDate = new DateTime(yearInit, monthInit, dayInit);
			var endDate = new DateTime(yearEnd, monthEnd, dayEnd);

			return View("ViewRides", RidesManager.RetrieveFriendsRides((UserManager.RetrieveUser(new Guid(User.Identity.Name), out error)).ID, initDate, endDate));
		}

		/// <summary>
		/// Retorna view com informações da carona cadastrada
		/// </summary>
		/// <param name="selectedRide"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult GetInfoRide(string selectedRide)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			

			return View("InfoRide",RidesManager.RetrieveRide(new Guid(selectedRide)));
		}

		/// <summary>
		/// Efetua a aceitação de um usuário a uma carona
		/// </summary>
		/// <param name="rideID"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult AcceptRide(string ID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			
			var ok = RidesManager.AcceptRide(new Guid(User.Identity.Name), new Guid(ID), userModel.FullName);
			if (ok)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				//TODO: se deu problema, mantém na tela
				return RedirectToAction(ID);
			}
		}

		/// <summary>
		/// Solicita a reserva de uma carona
		/// </summary>
		/// <param name="driverID">ID do motorista</param>
		/// <param name="rideID">ID da carona</param>
		/// <returns>Mensagem de retorno</returns>
		[HttpPost]
		public string RequestRide(string driverID, string rideID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			
			RidesManager.RequestRideReservation(userModel.ID, new Guid(driverID), new Guid(rideID), out error);
			if (error == ErrorEnum.NoErrors)
			{
				return "Reserva de carona solicitada com sucesso.";
			}
			else
			{
				return EnumUtils.ValueOf(error);
			}
		}

		/// <summary>
		/// Solicita o cancelamento de uma reserva de carona
		/// </summary>
		/// <param name="driverID">ID do motorista da carona</param>
		/// <param name="rideID">ID da carona a ser cancelada</param>
		/// <returns>Mensagem de sucesso ou erro</returns>
		[HttpPost]
		public string CancelRequest(string driverID, string rideID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			
			RidesRequestManager.CancelRideReservation(userModel.ID, new Guid(driverID), new Guid(rideID), out error);
			if (error == ErrorEnum.NoErrors)
			{
				return "Reserva de carona cancelada com sucesso.";
			}
			else
			{
				return EnumUtils.ValueOf(error);
			}
		}

		/// <summary>
		/// Retorna view de gerencia de caronas já selecionadas
		/// </summary>
		/// <returns>View com caronas já selecionadas</returns>
		[HttpGet]
		public ActionResult ManageRides()
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			return View(userModel);
		}

		[HttpPost]
		public ActionResult CancelDonorRide(string rideId)
		{
			RidesManager.CancelDonorRide(new Guid(rideId));
			return RedirectToAction("ManageRides");
		}
    }
}
