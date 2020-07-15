using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptographic.RandomJumpMap
{
    /// <summary>
    /// 
    /// </summary>
    public static class Manager
    {
        private static Dictionary<string, JumpMap> Maps;

        private static void generateJumpMap(string mapKey) 
        {
            if (Maps == null)
                Maps = new Dictionary<string, JumpMap>();

            if (!Maps.ContainsKey(mapKey))
                Maps.Add(mapKey, JumpMap.createNewRandomMap());
        }

        /// <summary>
        /// Map key is ("text.len-text.chars.sum-charPosition-password")
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string getMapKey(int length, int charPosition, string password) 
        {
            if (length < 0)
                throw new Exception("Text is empty or null!");

            if (string.IsNullOrEmpty(password))
                throw new Exception("Password is empty or null!");

            if (charPosition < 0)
                throw new Exception("Invalid char position!");

            var mapKey = $"{length}-{charPosition}-{password}";
            mapKey = Base64.crypt(mapKey);

            generateJumpMap(mapKey);

            return mapKey;
        }

        /// <summary>
        /// Get Jump to char value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static int getJumpChar(string mapKey, int fromChar) 
        {
            if (!Maps.ContainsKey(mapKey))
                throw new Exception("Map not found!");

            if (!Maps.ContainsKey(mapKey))
                Maps.Add(mapKey, JumpMap.createNewRandomMap());

            return Maps[mapKey].Jumps[fromChar];
        }

        /// <summary>
        /// Get Jump from char value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static int getReverseJumpChar(string mapKey, int toChar)
        {
            if (!Maps.ContainsKey(mapKey))
                throw new Exception("Map not found!");

            return Maps[mapKey].reverseJump(toChar);
        }

        /// <summary>
        /// Return crypted text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string crypt(string text, string password) 
        {
            var crypted = "";
            var charArray = text.ToCharArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                var mapKey = getMapKey(text.Length, i, password);

                crypted += getJumpChar(mapKey, charArray[i]) + (i == (charArray.Length - 1) ? ("") : ("x"));
            }

            crypted = Base64.crypt(crypted);

            return crypted;
        }

        /// <summary>
        /// Return decrypted text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string decrypt(string text, string password)
        {
            var decrypted = "";          
            text = Base64.decrypt(text);
            var charArray = text.Split('x').Select(i => int.Parse(i)).ToArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                var mapKey = getMapKey(charArray.Length, i, password);

                decrypted += (char)getReverseJumpChar(mapKey, charArray[i]);
            }

            return decrypted;
        }
    }
}
