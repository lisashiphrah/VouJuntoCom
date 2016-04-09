using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VouJuntoCom.Helpers
{
	/// <summary>
	/// Contém todas as mensagem de erro do sistema.
	/// </summary>
	public enum ErrorEnum
	{
		NoErrors,

		[DescriptionAttribute("O sistema está apresentando erros. Por favor, tente novamente.")]
		ExceptionError,

		[DescriptionAttribute("CPF já existente na base de dados.")]
		ExistentCPF,

		[DescriptionAttribute("Nome de usuário indisponível.")]
		ExistentUsername,

		[DescriptionAttribute("Usuário ou senha incorretos.")]
		InvalidUsername,

		[DescriptionAttribute("Senha incorreta.")]
		InvalidPassword,

		[DescriptionAttribute("Solicitação de amizade já foi enviada.")]
		ResendSolicitation,

		[DescriptionAttribute("Solicitação de carona já foi enviada.")]
		ResendRideSolicitation,
	}

}