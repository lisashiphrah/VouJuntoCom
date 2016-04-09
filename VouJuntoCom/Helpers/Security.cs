using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace VouJuntoCom.Helpers
{
	/// <summary>
	/// Responsável pela segurança dos dados.
	/// Possui métodos para criptografar e decriptar dados com base no algorítmo de criptografia Rijndael.
	/// </summary>
	public class Security
	{
		// Definição de algumas constantes
		private static string passPhrase = "Pas5pr@se";
		private static string saltValue = "s@1tValue";
		private static string hashAlgorithm = "SHA1";
		private static string initVector = "@1B2c3D4e5F6g7H8";
		private static int passwordIterations = 2;
		private static int keySize = 256;

		/// <summary>
		/// Criptografa texto recebido por parâmetro com base nas constantes declaradas
		/// </summary>
		/// <param name="plainText">Texto a ser criptografado</param>
		/// <returns>Texto criptografado</returns>
		public static string Encrypt(string plainText)
		{
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			// Criação da senha
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
			byte[] keyBytes = password.GetBytes(keySize / 8);

			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

			//Criptografa
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptoStream.FlushFinalBlock();

			// Converte os dados encriptados de um memory stream para um array de bytes.
			byte[] cipherTextBytes = memoryStream.ToArray();

			memoryStream.Close();
			cryptoStream.Close();

			// Converte os dados criptografados para um base64-encoded string.
			string cipherText = Convert.ToBase64String(cipherTextBytes);
			return cipherText;
		}

		/// <summary>
		/// Decripta string passada por parametro
		/// </summary>
		/// <param name="cipherText">Texto criptografado</param>
		/// <returns>Texto decriptado</returns>
		public static string Decrypt(string cipherText)								
		{
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
			byte[] keyBytes = password.GetBytes(keySize / 8);

			RijndaelManaged symmetricKey = new RijndaelManaged();

			symmetricKey.Mode = CipherMode.CBC;

			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
															 keyBytes,
															 initVectorBytes);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

			// Define cryptographic stream (always use Read mode for encryption).
			CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

			memoryStream.Close();
			cryptoStream.Close();

			string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
			return plainText;
		}
	}
}