using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VouJuntoCom.DAO;
using VouJuntoCom.Helpers;
using VouJuntoCom.Models;

namespace VouJuntoCom.Controllers
{
	[Authorize]
	public class CarsController : Controller
	{
		/// <summary>
		/// Action que retorna a lista de veículos do usuário
		/// </summary>
		/// <returns>Página com lista de veículos</returns>
		[HttpGet]
		public ActionResult List()
		{
			ErrorEnum result;
			var userID = User.Identity.Name;
			var search = CarsManager.RetrieveUserCars(new Guid(userID), out result);
			Session["TempCarsList"] = search;
			if (result != ErrorEnum.ExceptionError)
			{
				return View(search);
			}
			else
			{
				ViewBag.ErrorMessage = result;
				//TODO: DEFINIR TRATAMENTO / POPUP DE ERRO
				return null;
			}
		}

		/// <summary>
		/// Action que retorna um formulário para o cadastro de um novo veículo
		/// </summary>
		/// <returns>Página com formulário de cadastro</returns>
		[HttpGet]
		public ActionResult NewCar()
		{
			return View();
		}

		/// <summary>
		/// Insere o carro na lista de carros cadastrados pelo usuário
		/// </summary>
		/// <param name="model">Carro a ser inserido</param>
		/// <returns>Redireciona para lista se cadastrado com sucesso ou apresenta erro</returns>
		[HttpPost]
		public ActionResult NewCar(CarModel model)
		{
			ErrorEnum result;
			var userID = User.Identity.Name;
			var insert = CarsManager.InsertCar(model, new Guid(userID), out result);
			if (insert)
			{
				return RedirectToAction("List");
			}
			else
			{
				ViewBag.ErrorMessage = result;
				//TODO: DEFINIR TRATAMENTO DE ERRO
				return null;
			}
		}

		/// <summary>
		/// Remove o carro com identificador passado por parametro
		/// </summary>
		/// <param name="carID">Guid do carro</param>
		/// <returns>True se ok</returns>
		[HttpPost]
		public bool RemoveCar(Guid carID)
		{
			ErrorEnum result;
			var remove = CarsManager.RemoveCar(carID, out result);
			ViewBag.ErrorMessage = result;
			return remove;
		}

		/// <summary>
		/// Edita o carro com identificador passado por parametro
		/// </summary>
		/// <param name="model">Model do carro com alterações</param>
		/// <returns>null se ok</returns>
		[HttpPost]
		public bool EditCar(CarModel model)
		{
			ErrorEnum result;
			var edit = CarsManager.EditCar(model, out result);
			ViewBag.ErrorMessage = result;
			//TODO: DEFINIR TRATAMENTO DE ERRO
			return edit;
		}

		/// <summary>
		/// Retorna da tela de adição de veículo.
		/// Se lista de veículos continua na sessão, retorna lista pré-carregada.
		/// Caso contrário, recarrega novamente.
		/// </summary>
		/// <returns>Lista de veículos cadastrados</returns>
		[HttpGet]
		public ActionResult BackList()
		{
			var tempList = Session["TempCarsList"];
			if (tempList != null)
			{
				return View("List", tempList);
			}
			else
			{
				return RedirectToAction("List");
			}
		}

	}
}
