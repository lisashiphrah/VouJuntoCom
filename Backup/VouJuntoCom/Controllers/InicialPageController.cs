using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using VouJuntoCom.Models;
using VouJuntoCom.Helpers;
using VouJuntoCom.DAO;
using System.Net;

namespace VouJuntoCom.Controllers
{
    public class InicialPageController : Controller
    {
		DBConfigurations database = new DBConfigurations();

		/// <summary>
		/// Renderiza Home
		/// </summary>
		/// <returns>Tela Home</returns>
		[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

		/// <summary>
		/// Renderiza Sobre nós
		/// </summary>
		/// <returns>Tela Sobre nós</returns>
		[HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }

		/// <summary>
		/// Renderiza Contato
		/// </summary>
		/// <returns>Tela de Contato</returns>
		[HttpGet]
		public ActionResult Contact()
		{
			return View();
		}

        /// <summary>
        /// Renderiza tela de cadastro de usuário
        /// </summary>
        /// <returns>Tela de Cadastro de Usuário</returns>
        [HttpGet]
        public ActionResult NewUser()
        {
            return View();
        }

        /// <summary>
        /// Envia email escrito na página de contato
        /// </summary>
        /// <param name="nome">Nome do usuário</param>
        /// <param name="email">Email do usuário</param>
        /// <param name="mensagem">Mensagem escrita pelo usuário</param>
        /// <returns>View de contato com mensagem de sucesso ou erro</returns>
        [HttpPost]
        public ActionResult SendEmail(string nome, string email, string telefone, string mensagem)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Mensagem recebida via VouJunto.com.\n\nRemetente: {0}\nEmail: {1}\nTelefone: {3}\nMensagem: {2}", nome, email, mensagem, telefone);

            var fromAddress = new MailAddress("voujuntocom@gmail.com", "[VouJunto.com CONTATO]");
            var toAddress = new MailAddress("voujuntocom@gmail.com");
            var fromPassword = "a1234@45";
            var subject = "[VouJunto.com CONTATO]";
            var body = builder.ToString();

            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
               
                smtp.Send(message);
                ViewData["result"] = "Enviado";
                ViewData["message"] = "Mensagem enviada com sucesso! Obrigado pelo seu contato.";
            }

            //Captura possiveis erros no envio do email
            catch (Exception ex)
            {
                ViewData["result"] = "Erro";
                ViewData["message"] = "Problemas no envio do email. Por favor, confira as informações inseridas e tente novamente.";
            }
            return View("Contact");
      }

        /// <summary>
        /// Efetua a validação e cadastro de um novo usuário no sistema
        /// </summary>
        /// <param name="user">Model do usuário com os dados para a inserção</param>
        /// <returns>Home do usuário ou mantém-se na mesma tela levando consigo uma mensagem de erro</returns>
        [HttpPost]
        public ActionResult CreateUser(UserModel user)
        {
            string message;
            var insert = UserManager.InsertUser(user, out message);
            if (insert != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("messageError", message);
            return null;
        }

        /// <summary>
        /// Efetua o login de um usuário no sistema
        /// </summary>
        /// <param name="user">usuário a ser logado</param>
        /// <param name="password">senha inserida</param>
        /// <returns>Tela de login, ou erro caso ocorram problemas de autenticação.</returns>
        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            string message;
            var model = UserManager.LoginUser(user, password, out message);
            if (model != null)
            {
            }
            else
            {
            }
        }
    
    }
}
