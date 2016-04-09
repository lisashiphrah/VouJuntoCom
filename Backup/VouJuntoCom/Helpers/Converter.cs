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
    public class Converter
    {
        #region User
        /// <summary>
        /// Efetua a conversão de um tipo User (DTO) para um tipo UserModel
        /// </summary>
        /// <param name="userDTO">DTO do usuário</param>
        /// <returns>Model com os dados da DTO</returns>
        public static UserModel UserToUserModel(User userDTO)
        {
            var userModel = new UserModel
            {
                Birth = userDTO.Birth,
                CPF = userDTO.CPF,
                Email = userDTO.Email,
                FullName = userDTO.FullName,
                ID = userDTO.ID,
                Notifications = userDTO.Notifications,
                Password = userDTO.Password,
                RG = userDTO.RG,
                Sex = userDTO.Sex,
                Username = userDTO.Username
            };
            return userModel;
        }


        /// <summary>
        /// Mapeia propriedades de um objeto UserModel para objeto de banco
        /// </summary>
        /// <param name="dao">Model a ser mapeado</param>
        public static User UserModelToUser(UserModel userModel)
        {
            User dao = new User
            {
                Birth = userModel.Birth,
                CPF = userModel.CPF,
                Email = userModel.Email,
                FullName = userModel.FullName,
                ID = userModel.ID,
                Notifications = userModel.Notifications,
                Password = userModel.Password,
                RG = userModel.RG,
                Sex = userModel.Sex,
                Username = userModel.Username
            };
            return dao;
        }

        #endregion

        #region Banks
        /// <summary>
        /// Efetua a conversão de um tipo UserBankModel  para um tipo UserBank (DTO)
        /// </summary>
        /// <returns>UserBank com os dados da model</returns>
        public static UserBank UserBankModelToUserBank(UserBankModel bankModel)
        {
            UserBank dao = new UserBank
            {
                Account = bankModel.Account,
                Agency = bankModel.Agency,
                BankID = bankModel.BankID,
                ID = bankModel.ID
            };
            return dao;
        }
        #endregion
    }
}