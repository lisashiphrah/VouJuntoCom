using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VouJuntoCom.DAO;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.Controllers
{
    public class HistoricController : Controller
    {
		/// <summary>
		/// Renderiza tela inicial de histórico
		/// </summary>
		/// <returns>View de histórico</returns>
        [HttpGet]
        public ActionResult Index()
        {
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			TempData["Ano"] = DateTime.Now.Year;
			return View(UserManager.RetrieveUserHistory(userModel.ID, DateTime.Now.Year));
        }

		/// <summary>
		/// Busca informações a respeito do ano indicado para pesquisa
		/// </summary>
		/// <param name="year">Ano indicado para pesquisa</param>
		/// <returns>View de histórico para o ano selecionado</returns>
		[HttpGet]
		public ActionResult Refresh(string year)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			TempData["Ano"] = Int32.Parse(year);
			return View("Index", UserManager.RetrieveUserHistory(userModel.ID, Int32.Parse(year)));
		}

		/// <summary>
		/// Renderiza tela de simulador
		/// </summary>
		/// <returns>View de simulador</returns>
		[HttpGet]
		public ActionResult Simulator()
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			return View(UserManager.RetrieveUserHistory(userModel.ID, DateTime.Now.Year));
		}
    }
}
