using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VouJuntoCom.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Redireciona para a página inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
