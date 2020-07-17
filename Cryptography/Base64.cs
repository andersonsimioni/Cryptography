using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    /// <summary>
    /// <DEVELOPER> Anderson Simioni </DEVELOPER>
    /// This class encode/decode strings from base64
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Encode string to base64
        /// </summary>
        /// <param name="data">string data convert to base64</param>
        /// <param name="level">conversions number</param>
        /// <returns>data in base64</returns>
        public static string crypt(string data, int level = 1) 
        {
            if (level < 0)
                throw new Exception("Invalid level!");
            if (string.IsNullOrEmpty(data))
                throw new Exception("Invalid data!");

            var cryptedData = data;

            for (int i = 0; i < level; i++)
                cryptedData = Convert.ToBase64String(UnicodeEncoding.UTF8.GetBytes(cryptedData));

            return cryptedData;
        }

        /// <summary>
        /// Decode string from base64
        /// </summary>
        /// <param name="data">crypeted data to get from base 64</param>
        /// <param name="level">crypted data level</param>
        /// <returns>decrypetd data</returns>
        public static string decrypt(string data, int level = 1) 
        {
            if (level < 0)
                throw new Exception("Invalid level!");
            if (string.IsNullOrEmpty(data))
                throw new Exception("Invalid data!");

            var decryptedData = data;

            for (int i = 0; i < level; i++)
                decryptedData = UnicodeEncoding.UTF8.GetString(Convert.FromBase64String(decryptedData));

            return decryptedData;
        }
    }
}