using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VouJuntoCom.Helpers;

namespace VouJuntoCom.DAO
{
	public class Images
	{
		[Key]
		public Guid UserID { get; set; }
		public string ContentType { get; set; }
		public byte[] ImageContent { get; set; }
	}

	public class ImagesManager
	{
		/// <summary>
		/// Armazena imagem selecionada pelo usuário no cadastro.
		/// </summary>
		/// <param name="fileImage">Imagem selecionada</param>
		/// <param name="userID">ID do usuário que a cadastrou</param>
		public static void SaveImage(HttpPostedFileBase fileImage, Guid userID)
		{
			DBConfigurations database = new DBConfigurations();
			try
			{
				var imageData = new byte[fileImage.ContentLength];
				fileImage.InputStream.Read(imageData, 0, fileImage.ContentLength);
				Images img = new Images
				{
					UserID = userID,
					ImageContent = imageData,
					ContentType = fileImage.ContentType
				};

				database.Images.Add(img);
				database.SaveChanges();
			}
			catch (Exception)
			{
				//TODO ?
			}

		}

		/// <summary>
		/// Recupera imagem salva pelo usuário
		/// </summary>
		/// <param name="userID">ID do usuário</param>
		/// <returns></returns>
		public static FileContentResult RetrieveImage(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();

			try
			{
				var img = (from image in database.Images where image.UserID == userID select image).First();
				if (img != null)
				{
					return new FileContentResult(img.ImageContent, img.ContentType);
				}
				else
				{
					return null;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void RemoveImage(Guid userID)
		{
			DBConfigurations database = new DBConfigurations();

			try
			{
				var img = (from image in database.Images where image.UserID == userID select image).First();
				if (img != null)
				{
					database.Images.Remove(img);
					database.SaveChanges();
				}
			}
			catch (Exception)
			{
				// TODO
			}
		}
	}
}