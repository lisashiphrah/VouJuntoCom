using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VouJuntoCom.Helpers;
using VouJuntoCom.Models;

namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que representa uma instancia da tabela Friendships no banco de dados.
	/// </summary>
	public class Friendships
	{
		[Key]
		[Column(Order = 0)]
		public Guid User_A { get; set; }
		[Key]
		[Column(Order = 1)]
		public Guid User_B { get; set; }

		public bool Approved { get; set; }
	}

	/// <summary>
	/// Efetua operações de CRUD relacionadas as amizades
	/// </summary>
	public class FriendshipManager
	{
		/// <summary>
		/// Efetua uma pesquisa para indicações de amizade
		/// </summary>
		/// <param name="model">Usuário atualmente logado</param>
		/// <param name="error">Controle de erro</param>
		/// <param name="city">Cidade para filtragem</param>
		/// <param name="years">Anos para filtragem</param>
		/// <param name="name">Nome para filtragem</param>
		/// <param name="email">Email para filtragem</param>
		/// <returns>Lista com indicações de amizade</returns>
		public static List<UserModel> FindFriends(UserModel model, out ErrorEnum error, string city = "", int? years = null, string name = "", string email = "")
		{
			//Efetua busca dos amigos relacionados
			//if (model.QtdFriends > 0)
			//{
			//}

			//Efetua busca caso usuário não possua nenhuma amizade adicionada

			//else
			//{
			return RetrieveNoFriends(model, city, years, name, email, out error);
			//}
		}

		/// <summary>
		/// Efetua a pesquisa de possíveis conexões do usuário a partir dos filtros recebidos
		/// </summary>
		/// <param name="model">Model do usuário logado</param>
		/// <param name="city">Cidade passada como parâmetro na pesquisa</param>
		/// <param name="years">Idade passada como parâmetro na pesquisa</param>
		/// <param name="name">Nome passado como parâmetro na pesquisa</param>
		/// <param name="email">Email passado como parâmetro na pesquisa</param>
		/// <param name="error">Variável de controle</param>
		/// <returns>Lista com as 100 primeiras opções de amizade.</returns>
		public static List<UserModel> RetrieveNoFriends(UserModel model, string city, int? years, string name, string email, out ErrorEnum error)
		{
			var listRelatedFriends = new List<Users>();
			error = ErrorEnum.NoErrors;

			try
			{
				if ((name.Trim().Length > 0) && (email.Trim().Length > 0))
				{
					listRelatedFriends = FriendshipManager.SearchNameEmail(name, email);
				}

				else if ((name.Trim().Length > 0))
				{
					listRelatedFriends = FriendshipManager.SearchName(name);
				}

				else if ((email.Trim().Length > 0))
				{
					listRelatedFriends = FriendshipManager.SearchEmail(email);
				}

				else
				{
					listRelatedFriends = FriendshipManager.SearchNoFilters(city, years, model.City, model.Birth.Year);
				}

				//Remove ele mesmo da lista
				bool containsItem = listRelatedFriends.Any(item => item.ID == model.ID);
				if (containsItem)
				{
					listRelatedFriends.Remove(
												(from friend in listRelatedFriends
												 where friend.ID == model.ID
												 select friend).First()
											);
				}

				//Remove os que já são amigos da lista
				var listAux = new List<Users>();
				foreach (var friend in listRelatedFriends)
				{
					if (!FriendshipManager.AreFriends(model.ID, friend.ID))
					{
						listAux.Add(friend);
					}
				}

				return Conversor.ListUserToModel(listAux);
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return null;
			}
		}

		/// <summary>
		/// Se não definiu nada, ou se definiu só cidade ou idade, busca pelos dois
		/// </summary>
		/// <param name="city">Cidade para pesquisa</param>
		/// <param name="years">Idade para pesquisa</param>
		/// <param name="modelCity">Cidade do usuário</param>
		/// <param name="modelYear">Ano de nascimento do usuário</param>
		/// <returns>Lista com resultados encontrados</returns>
		private static List<Users> SearchNoFilters(string city, int? years, string modelCity, int modelYear)
		{
			DBConfigurations database = new DBConfigurations();
			var listRelatedFriends = new List<Users>();

			//Se não definiu filtro algum e não tem nenhum amigo, sistema procura 
			//usuário da mesma cidade com até 10 anos de diferença
			if (city.Trim().Length == 0)
			{
				city = modelCity;
			}
			if (years == null)
			{
				years = 10;
			}

			listRelatedFriends = (from user in database.Users
								  where
									  ((user.City == city) &&
									  ((user.Birth.Year - modelYear) <= years)) &&
									  ((user.Birth.Year - modelYear) >= -(years))
								  select user).Take(20).ToList();

			return listRelatedFriends;
		}

		/// <summary>
		/// Se definiu apenas email, pesquisa por este
		/// </summary>
		/// <param name="email">Email para pesquisa</param>
		/// <returns>Lista com resultados encontrados</returns>
		private static List<Users> SearchEmail(string email)
		{
			DBConfigurations database = new DBConfigurations();
			var listRelatedFriends = new List<Users>();

			listRelatedFriends = (from user in database.Users
								  where (user.Email.Contains(email))
								  select user).Take(20).ToList();

			return listRelatedFriends;
		}

		/// <summary>
		/// Se defininiu apenas nome, pesquisa por este
		/// </summary>
		/// <param name="name">Nome solicitado para pesquisa</param>
		/// <returns>Lista com resultados encontrados</returns>
		private static List<Users> SearchName(string name)
		{
			DBConfigurations database = new DBConfigurations();
			var listRelatedFriends = new List<Users>();

			listRelatedFriends = (from user in database.Users
								  where (user.FullName.Contains(name))
								  select user).Take(20).ToList();
			return listRelatedFriends;
		}

		/// <summary>
		/// Se definiu nome e email, pesquisa por estes 2
		/// </summary>
		/// <param name="name">Nome para pesquisa</param>
		/// <param name="email">Email para pesquisa</param>
		/// <returns>Lista com resutados encontrados</returns>
		private static List<Users> SearchNameEmail(string name, string email)
		{
			DBConfigurations database = new DBConfigurations();
			var listRelatedFriends = new List<Users>();

			listRelatedFriends = (from user in database.Users
								  where
									  (user.FullName.Contains(name)) &&
									  (user.Email.Contains(email))
								  select user).Take(20).ToList();
			return listRelatedFriends;
		}

		/// <summary>
		/// Verifica se existe amizade entre dois usuários, considerando convites já enviados
		/// </summary>
		/// <param name="user_A">Usuário A para pesquisa</param>
		/// <param name="user_B">Usuário B para pesquisa</param>
		/// <returns>True se existe ao menos o envio de solicitação, false se nem isso existir.</returns>
		private static bool AreFriends(Guid user_A, Guid user_B)
		{
			DBConfigurations database = new DBConfigurations();

			var containsItem = (from friends in database.Friendships
								where
									(friends.User_A == user_A && friends.User_B == user_B) ||
									(friends.User_A == user_B && friends.User_B == user_A)
								select friends).Count();
			if (containsItem == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Verifica se existe amizade entre dois usuários já aceitos
		/// </summary>
		/// <param name="user_A">Usuário A para pesquisa</param>
		/// <param name="user_B">Usuário B para pesquisa</param>
		/// <returns>True se existe a confirmação de amizade, false se não existir.</returns>
		private static bool AreFriendsAccepted(Guid user_A, Guid user_B)
		{
			DBConfigurations database = new DBConfigurations();

			var containsItem = (from friends in database.Friendships
								where
									(friends.User_A == user_A && friends.User_B == user_B && friends.Approved == true) ||
									(friends.User_A == user_B && friends.User_B == user_A && friends.Approved == true)
								select friends).Count();
			if (containsItem == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Envia um convite de amizade do usuário userModel para o usuário com ID da guid recebida
		/// </summary>
		/// <param name="userModel">Usuário logado (usuário que enviou o convite)</param>
		/// <param name="friendID">ID do usuário a receber solicitação de amizade</param>
		/// <param name="error">Controle de erro</param>
		/// <returns>True se enviou solicitação com sucesso ou false</returns>
		public static bool InviteFriend(UserModel userModel, Guid friendID, out ErrorEnum error)
		{
			//Verifica se convite já não foi enviado
			DBConfigurations database = new DBConfigurations();
			error = ErrorEnum.NoErrors;
			try
			{
				//Se convite não existir, envia
				if (!FriendshipManager.AreFriends(userModel.ID, friendID))
				{
					var newFriendship = new Friendships
					{
						User_A = userModel.ID,
						User_B = friendID,
						Approved = false
					};
					database.Friendships.Add(newFriendship);
					database.SaveChanges();
					return true;
				}
				//Se existir, retorna falso
				else
				{
					error = ErrorEnum.ResendSolicitation;
					return false;
				}
			}
			catch (Exception)
			{
				error = ErrorEnum.ExceptionError;
				return false;
			}
		}

		/// <summary>
		/// Recupera todas as solicitações de amizades enviadas para o usuário
		/// </summary>
		/// <param name="userID">ID do usuário a ser pesquisado</param>
		/// <returns>Lista com todas as solicitações de amizades pendentes</returns>
		public static List<UserModel> GetFriendsRequests(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var listFriendsRequests = (from request in database.Friendships
									   where ((request.User_B == userID) &&
									   (request.Approved == false))
									   select request).ToList();

			var listUsersRequest = new List<UserModel>();
			foreach (var request in listFriendsRequests)
			{
				var newUser = UserManager.RetrieveUserLight(request.User_A);
				listUsersRequest.Add(newUser);
			}
			return listUsersRequest;
		}

		/// <summary>
		/// Aceita solicitação de amizade entre dois usuários
		/// </summary>
		/// <param name="userID">Usuário que aceitou solicitação</param>
		/// <param name="friendID">Usuário que enviou solicitação</param>
		/// <param name="result">Controle de erro</param>
		/// <returns>True se aceitou com sucesso</returns>
		public static bool AcceptFriend(Guid userID, Guid friendID, out ErrorEnum result)
		{
			DBConfigurations database = new DBConfigurations();
			result = ErrorEnum.NoErrors;
			try
			{
				var friendshipSolicitation = database.Friendships.First(request =>
											(request.User_A == friendID &&
											request.User_B == userID &&
											request.Approved == false));

				friendshipSolicitation.Approved = true;

				Friendships newFriend = new Friendships
				{
					User_A = userID,
					User_B = friendID,
					Approved = true
				};

				database.Friendships.Add(newFriend);
				database.SaveChanges();

				return true;
			}
			catch (Exception)
			{
				result = ErrorEnum.ExceptionError;
				return false;
			}
		}

		/// <summary>
		/// Retorna uma lista com todos os amigos de determinado usuário
		/// </summary>
		/// <param name="userID">ID do usuário a ser efetuada a pesquisa</param>
		/// <returns>Lista com todos os amigos do usuário</returns>
		public static List<UserModel> RetrieveFriends(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var friendships = FriendshipManager.RetrieveFriendsID(userID);

			var listFriends = (from user in database.Users where friendships.Contains(user.ID) select user).ToList();

			var listFriendsModel = new List<UserModel>();
			foreach (var friend in listFriends)
			{
				listFriendsModel.Add(Conversor.UserDTOToModel(friend));
			}
			return listFriendsModel;
		}

		/// <summary>
		/// Retorna apenas os IDS de todos os contatos de determinado usuário
		/// </summary>
		/// <param name="userID">ID do usuário a ter a pesquisa efetuada</param>
		/// <returns>Lista com IDS</returns>
		public static List<Guid> RetrieveFriendsID(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var friendships = (from friend in database.Friendships
							   where (friend.User_A == userID) &&
							   (friend.Approved == true)
							   select friend.User_B).ToList();
			return friendships;
		}

		/// <summary>
		/// Retorna um dictionary contendo como informação uma lista de amigos e se a amizade existe ou o convite de amizade não foi
		/// confirmado
		/// </summary>
		/// <param name="userID">ID do usuário a ter a pesquisa efetuada</param>
		/// <returns>Dictionary com relacionamentos</returns>
		public static Dictionary<UserModel, bool> RetrieveFriendsRelations(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			var friendships = (from friend in database.Friendships
							   where (friend.User_A == userID)
							   select friend.User_B).ToList();

			var listFriends = (from user in database.Users where friendships.Contains(user.ID) select user).ToList();

			var dictionarymodel = new Dictionary<UserModel, bool>();
			foreach (var friend in listFriends)
			{
				dictionarymodel.Add(Conversor.UserDTOToModel(friend), FriendshipManager.AreFriendsAccepted(userID, friend.ID));
			}
			return dictionarymodel;
		}

		/// <summary>
		/// Remove uma relação de amizade entre dois contatos
		/// </summary>
		/// <param name="userID">ID de um contato</param>
		/// <param name="friendID">ID do outro contato</param>
		/// <returns>True se efetuou a remoção</returns>
		public static bool RemoveFriendship(Guid userID, Guid friendID)
		{
			DBConfigurations database = new DBConfigurations();

			var relation_A = database.Friendships.First(request =>
											(request.User_A == friendID &&
											request.User_B == userID));
			var relation_B = database.Friendships.First(request =>
											(request.User_A == userID &&
											request.User_B == friendID));
			database.Friendships.Remove(relation_A);
			database.Friendships.Remove(relation_B);
			database.SaveChanges();
			return true;
		}
	}
}