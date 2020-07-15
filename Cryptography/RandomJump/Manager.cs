using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptographic
{
    /// <summary>
    /// 
    /// </summary>
    public static class Manager
    {
        private static Dictionary<string, Map> Maps;

        private static string MapsPath = "RandomJumpMapCollection\\Maps";

        /// <summary>
        /// Save all jump maps in collection on server's file
        /// </summary>
        private static void saveJumpMapsCollection() 
        {
            if (Maps == null)
                return;

            var serialized = JsonConvert.SerializeObject(Maps);

            try
            {
                ServerDirectory.Directories.createDirectory(MapsPath.Replace("\\Maps", ""));
                ServerDirectory.Files.writeText(MapsPath, serialized);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when save jump maps table, ({ex.Message})");
            }
        }

        /// <summary>
        /// Load saved jump maps on server's file
        /// </summary>
        private static void loadJumpMapsCollectionFromFile() 
        {
            if (Maps == null)
            {
                if (ServerDirectory.Files.fileExist(MapsPath))
                {
                    try
                    {
                        var readed = ServerDirectory.Files.readText(MapsPath);
                        var desirialized = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Map>>(readed);

                        Maps = desirialized;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error ocurred when read collection file ({ex.Message})");
                    }
                }
                else
                    Maps = new Dictionary<string, Map>();
            }
        }

        private static void generateJumpMap(string mapKey) 
        {
            loadJumpMapsCollectionFromFile();

            if (!Maps.ContainsKey(mapKey))
                Maps.Add(mapKey, Map.createNewRandomMap());

            saveJumpMapsCollection();
        }

        /// <summary>
        /// Map key is ("text.len-text.chars.sum-charPosition-password")
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string getMapKey(string text, int charPosition, string password) 
        {
            if (string.IsNullOrEmpty(text))
                throw new Exception("Text is empty or null!");

            if (string.IsNullOrEmpty(password))
                throw new Exception("Password is empty or null!");

            if (charPosition < 0)
                throw new Exception("Invalid char position!");

            var len = text.Length;

            var mapKey = $"{len}-{charPosition}-{password}";
            mapKey = Base64.convertToBase64(mapKey);

            generateJumpMap(mapKey);

            return mapKey;
        }

        /// <summary>
        /// Get Jump to char value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static char getJumpChar(string mapKey, char fromChar) 
        {
            if (!Maps.ContainsKey(mapKey))
                throw new Exception("Map not found!");

            return Maps[mapKey].Jump[fromChar];
        }

        /// <summary>
        /// Get Jump from char value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static char getReverseJumpChar(string mapKey, char toChar)
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
                var mapKey = getMapKey(text, i, password);
                crypted += getJumpChar(mapKey, charArray[i]);
            }

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
            var charArray = text.ToCharArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                var mapKey = getMapKey(text, i, password);
                decrypted += getReverseJumpChar(mapKey, charArray[i]);
            }

            return decrypted;
        }
    }
}
