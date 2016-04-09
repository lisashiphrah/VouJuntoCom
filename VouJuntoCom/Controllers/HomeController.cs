using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VouJuntoCom.DAO;
using VouJuntoCom.Helpers;
using VouJuntoCom.Models;

namespace VouJuntoCom.Controllers
{
	[Authorize]
    public class HomeController : Controller
    {
        /// Redireciona para a página inicial
        /// </summary>
        /// <returns></returns>
		[HttpGet]
        public ActionResult Index()
        {
			ErrorEnum result;
			var currentUser = UserManager.RetrieveUser(new Guid(User.Identity.Name), out result);

			//Armazena usuário logado na sessão.
			if (currentUser != null)
			{
				Session["LoggedUser"] = currentUser;
			}
			else
			{
				ViewBag.ErrorMessage = result;
			}

            return View("Index", currentUser);
        }

		/// <summary>
		/// Renderiza página para troca de informações do usuário
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult EditUser()
		{
			ErrorEnum error;
			UserModel actualUser = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			return View("EditUser", actualUser);
		}

		/// <summary>
		/// Atualiza o usuário recebido por parametro
		/// </summary>
		/// <param name="model">Model do usuário a ser atualizado</param>
		/// <returns>Redireciona para tela indice se ok</returns>
		[HttpPost]
		public ActionResult EditUser(UserModel model, string newPassword, string Password)
		{
			ErrorEnum result;

			if (Password.Trim().Length > 0)
			{
				var validate = UserManager.ValidatePassword(Password, model.ID, out result);
				if (result == ErrorEnum.NoErrors)
				{
					if (newPassword.Trim().Length > 0)
					{
						model.Password = newPassword;
					}
					var update = UserManager.UpdateUser(model, out result);
					if (result == ErrorEnum.NoErrors)
					{
						//TODO OK
						Session["LoggedUser"] = model;
						return View("Index", model);
					}
					else
					{
						//TODO ERROS
						ViewBag.ErrorMessage = result;
						return View("EditUser", model);
					}
				}

				else
				{
					//TODO ERROS
					ViewBag.ErrorMessage = result;
					return View("EditUser", model);
				}
			}
			else
			{
				var update = UserManager.UpdateUser(model, out result);
				if (result == ErrorEnum.NoErrors)
				{
					//TODO OK
					Session["LoggedUser"] = model;
					return View("Index", model);
				}
				else
				{
					//TODO ERROS
					ViewBag.ErrorMessage = result;
					return View("EditUser", model);
				}
			}
		}

		/// <summary>
		/// Renderiza tela de encontrar amigos
		/// </summary>
		/// <returns>Tela de encontrar amigos</returns>
		[HttpGet]
		public ActionResult FindFriends()
		{
			ErrorEnum result;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out result);
			if(userModel == null)
			{
				var userID = User.Identity.Name;
				userModel = UserManager.RetrieveUser(new Guid(userID), out result);
				Session["LoggedUser"] = userModel;
			}
			var friends = FriendshipManager.FindFriends(userModel, out result);
			if (result == ErrorEnum.NoErrors)
			{
				return View("FindFriends", friends);
			}
			else
			{
				// TODO: DEFINIR ERRO
				ViewBag.ErrorMessage = result;
				return View("FindFriends");
			}
		}

		/// <summary>
		/// Recupera contatos baseado nos filtros
		/// </summary>
		/// <param name="findName">Filtro de nome</param>
		/// <param name="findCity">Filtro de cidade</param>
		/// <param name="findAge">Filtro de idade</param>
		/// <param name="findEmail">Filtro de email</param>
		/// <returns>Lista de amigos</returns>
		[HttpGet]
		public ActionResult FilterFriends(string findName, string findCity, string findAge, string findEmail)
		{
			ErrorEnum result;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out result);
			var friends = new List<UserModel>();
			try
			{
				friends = FriendshipManager.RetrieveNoFriends(userModel, findCity, Int32.Parse(findAge), findName, findEmail, out result);
			}
			catch(Exception)
			{
				friends = FriendshipManager.RetrieveNoFriends(userModel, findCity, null, findName, findEmail, out result);
			}

			if (result == ErrorEnum.NoErrors)
			{
				return View("FindFriends", friends);
			}
			else
			{
				//TODO TRATAR ERRO
				ViewBag.ErrorMessage = result;
				return View("FindFriends", null); 
			}

		}

		/// <summary>
		/// Retorna a imagem associada a determinado usuário
		/// </summary>
		/// <param name="userID">ID do usuário</param>
		/// <returns>Imagem associada a este</returns>
		[HttpGet]
		public FileContentResult GetImage(string userID)
		{
			var fileContentResult = ImagesManager.RetrieveImage(new Guid(userID));
			return fileContentResult;
		}

		/// <summary>
		/// Adiciona o amigo passado como parametro
		/// </summary>
		/// <param name="friendID">ID do contato a ser adicionado</param>
		/// <returns>True se adicionou com sucesso, false se não</returns>
		[HttpPost]
		public bool AddFriend(string friendID)
		{
			ErrorEnum result;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out result);
			var sendInvitation = FriendshipManager.InviteFriend(userModel, new Guid(friendID), out result);
			if (sendInvitation)
			{
				return true;
			}
			else
			{
				ViewBag.ErrorMessage = result;
				return false;
			}
		}

		/// <summary>
		/// Retorna todas as solicitações de amizade pendentes
		/// </summary>
		/// <param name="userID">Usuário que possui as solicitações de amizade</param>
		/// <returns>Lista com as solicitações</returns>
		[HttpGet]
		public ActionResult OpenFriendsSolicitations(string userID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(userID), out error);
			}

			return View("FriendsRequests", userModel.FriendsRequests);
		}

		[HttpGet]
		public ActionResult OpenReservations(string userID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(userID), out error);
			}
			return View("OpenReservations", userModel.RidesRequests);
		}

		[HttpPost]
		public void CancelRidePresence(string rideID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			
			RidesManager.CancelRidePresence(new Guid(User.Identity.Name), new Guid(rideID), userModel.FullName);
			Session["LoggedUser"] = userModel;
		}

		[HttpPost]
		public void AcceptReservation(string userID, string rideID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(userID), out error);
			}
			RidesRequestManager.AcceptReservation(new Guid(userID), new Guid(rideID), userModel.FullName);
			Session["LoggedUser"] = userModel;
		}

		[HttpPost]
		public void RejectReservation(string userID, string rideID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(userID), out error);
			}
			RidesRequestManager.RejectReservation(new Guid(userID), new Guid(rideID), userModel.FullName);
			Session["LoggedUser"] = userModel;
		}

		/// <summary>
		/// Renderiza tela de notificações
		/// </summary>
		/// <param name="userID">ID do usuário que possui a notificação</param>
		/// <returns>View com lista de notificações</returns>
		public ActionResult OpenNotifications(string userID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(userID), out error);
			}
			NotificationsManager.RemoveNotifications(new Guid(userID));
			return View("Notifications", userModel.ListNotifications);
		}

		/// <summary>
		/// Aceita um convite de amizade pendente
		/// </summary>
		/// <param name="userID">ID do usuário a ser aceito</param>
		/// <returns>True se adicionou false se não</returns>
		[HttpPost]
		public bool AcceptFriend(string userID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(userID), out error);
			}

			bool accepted = FriendshipManager.AcceptFriend(userModel.ID, new Guid(userID), out error);
			if (error == ErrorEnum.NoErrors)
			{
				NotificationsManager.AddFriendAcceptedNotification(new Guid(userID), userModel.FullName);
				return true;
			}
			else
			{
				ViewBag.ErrorMessage = error;
				return false;
			}
		}

		/// <summary>
		/// Retorna view com todos os contatos do usuário
		/// </summary>
		/// <returns>View com todos os contatos do usuário</returns>
		[HttpGet]
		public ActionResult MyFriends()
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			}

			return View("Friends", FriendshipManager.RetrieveFriendsRelations(new Guid(User.Identity.Name)));
		}

		/// <summary>
		/// Remove determinado contato da lista de contatos do usuário
		/// </summary>
		/// <param name="friendID">ID do contato a ser removido</param>
		/// <returns>True se removeu com sucesso, falso caso contrário</returns>
		[HttpPost]
		public bool RemoveFriend(Guid friendID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			}

			bool removed = FriendshipManager.RemoveFriendship(new Guid(User.Identity.Name), friendID);
			return removed;
		}

		[HttpPost]
		public ActionResult RemoveAccount()
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			}
			bool ok = UserManager.RemoveAccount(new Guid(User.Identity.Name));
			if (ok)
			{
				FormsAuthentication.SignOut();
				return RedirectToAction("Index");
			}
			else
			{
				//TODO: ERRO
				return RedirectToAction("Index");
			}
		}

		/// <summary>
		/// Efetua o envio de uma mensagem a um usuário
		/// </summary>
		/// <param name="receiverID">ID do recebedor da mensagem</param>
		/// <param name="receiverName">Nome do recebedor da mensagem</param>
		/// <param name="message">Mensagem</param>
		/// <returns>View de retorno e confirmação de envio</returns>
		[HttpPost]
		public void SendMessage(string ID, string FullName, string message)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			}
			MessagesManager.SendMessage(userModel.ID.ToString(), userModel.FullName, FullName, ID, message);
		}

		/// <summary>
		/// Abre view para envio de nova mensagem
		/// </summary>
		/// <param name="userID">ID do recebedor da mensagem</param>
		/// <returns>View para inserção de mensagem</returns>
		[HttpGet]
		public ActionResult NewMessage(string userID)
		{
			ErrorEnum error;
			var userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			if (userModel == null)
			{
				userModel = UserManager.RetrieveUser(new Guid(User.Identity.Name), out error);
			}
			var user = UserManager.RetrieveUserLight(new Guid(userID));
			return View("NewMessage", user);
		}

		[HttpGet]
		public void MarkAsRead(string messageID)
		{
			MessagesManager.MarkAsRead(new Guid(messageID));
		}

		[HttpGet]
		public void RemoveMessage(string messageID)
		{
			MessagesManager.RemoveMessage(new Guid(messageID));
		}
	}
}
