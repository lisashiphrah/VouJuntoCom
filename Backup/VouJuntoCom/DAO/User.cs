using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VouJuntoCom.Models;
using VouJuntoCom.Helpers;
using System.Text;
      
namespace VouJuntoCom.DAO
{
	/// <summary>
	/// Classe que mapeia a tabela User do banco.
	/// </summary>
	public class User
	{
		public Guid ID { get; set; }
		public Guid UserBankID { get; set; }
        //public int Image { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Birth { get; set; }  
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
		public bool Notifications { get; set; }

	}

    /// <summary>
    /// Efetua operações de CRUD relacionadas ao usuário
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Adiciona uma nova instancia de usuário no banco de dados se esta não existir.
        /// Verifica se nome de usuário e CPF já estão em uso antes de adicionar.
        /// </summary>
        /// <param name="user">UserModel a ser adicionado</param>
        /// <returns>ID do usuário adicionado, ou uma Guid vazia em caso de erro de inserção.</returns>
        public static Guid? InsertUser(UserModel user, out string message)
        {
            message = "";

            if (!UserManager.IsUserLoginExist(user.Username))
            {
                if (!UserManager.CPFUserExist(user.CPF))
                {
                    try
                    {
                        //Encripta a senha
                        string encryptPass = Security.Encrypt(user.Password);
                        user.Password = encryptPass;

                        //Define informações do banco
                        UserBank userBankDAO = user.BankAccount.ToDAO();
                        userBankDAO.ID = Guid.NewGuid();

                        //Define configurações do usuário
                        User userDAO = user.ToDAO();
                        userDAO.ID = Guid.NewGuid();
                        userDAO.UserBankID = userBankDAO.ID;

                        return UserManager.AddUser(userDAO, userBankDAO);
                       
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                else
                {
                    message = "CPF já existente na base de dados.";
                    return null;
                }
            }
            else
            {
                message = "Nome de usuário indisponível.";
                return null;
            }
        }

        private static Guid AddUser(User userDAO, UserBank userBankDAO)
        {
            DBConfigurations database = new DBConfigurations();
            database.User.Add(userDAO);
            database.UserBank.Add(userBankDAO);
            database.SaveChanges();
            return userDAO.ID;
        }

        /// <summary>
        /// Verifica se a ID selecionada para login não existe na base de dados
        /// </summary>
        /// <param name="loginID">Nome de usuário selecionado</param>
        /// <returns>True se usuário existir na base, False se não existir</returns>
        public static bool IsUserLoginExist(string userID)
        {
            DBConfigurations database = new DBConfigurations();
            bool exists = (from user in database.User where user.Username == userID select user).Any();
            return exists;
        }

        /// <summary>
        /// Verifica se o CPF selecionado não existe na base de dados
        /// </summary>
        /// <param name="userCPF">CPF do usuário selecionado</param>
        /// <returns>True se o CPF existir e false se não existir</returns>
        public static bool CPFUserExist(string userCPF)
        {
            DBConfigurations database = new DBConfigurations();
            bool exists = (from user in database.User where user.CPF == userCPF select user).Any();
            return exists;
        }

        /// <summary>
        /// Efetua o login do usuário e senha caso estes existam
        /// </summary>
        /// <param name="user">Usuário a ser logado</param>
        /// <param name="password">Senha inserida</param>
        /// <param name="message">Mensagem de erro para o controle</param>
        /// <returns></returns>
        public static UserModel LoginUser(string username, string password, out string message)
        {
            DBConfigurations database = new DBConfigurations();
            message = "";

            var encryptPassword = Security.Encrypt(password);
            var userDTO = (from user in database.User where user.Username == username && user.Password == encryptPassword select user).First();
            if (userDTO != null)
            {
                var userModel = Converter.UserToUserModel(userDTO);
                return userModel;
            }
            message = "Usuário ou senha incorretos.";
            return null;
        }

        public static IEnumerable<CarModel> RetrieveCars(Guid userID)
        {
        }

        public static UserBankModel RetrieveAccount(Guid userID)
        {
        }
    }
}
