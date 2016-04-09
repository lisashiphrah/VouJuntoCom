using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VouJuntoCom.Helpers
{
	/// <summary>
	/// Recupera o valor da descrição de um enum.
	/// </summary>
	public class EnumUtils
	{
		/// <summary>
		/// Retorna a descrição correspondete a determinado valor de enum.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ValueOf(Enum value)
		{
			if (value != null)
			{
				FieldInfo fi = value.GetType().GetField(value.ToString());
				DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (attributes.Length > 0)
				{
					return attributes[0].Description;
				}
				else
				{
					return value.ToString();
				}
			}
			return null;
		}
	}
}