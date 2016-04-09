using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace VouJuntoCom.Helpers
{
	/// <summary>
	/// Contem métodos auxiliares globais da solução
	/// </summary>
	public class Helper
	{
		/// <summary>
		/// Recebe uma lista e retorna uma string com seus componentes separados por vírgulas
		/// </summary>
		/// <param name="genericList">Lista genérica para tratamento</param>
		/// <returns>String contendo os elementos separados por vírgulas</returns>
		public static string ToCommaSeparatedString(List<Object> genericList)
		{
			StringBuilder builder = new StringBuilder();
			foreach (var obj in genericList)
			{
				builder.AppendFormat("{0}," , obj);
			}
			//remove a última vírgula
			builder.Remove(builder.Length - 1, 1);
			return builder.ToString();
		}

		public static double Add(double a, double b)
		{
			return a + b;
		}

		public static double Sub(double a, double b)
		{
			return a - b;
		}
	}
}