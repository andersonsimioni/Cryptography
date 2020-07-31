using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptography.RandomJumpMap
{
    /// <summary>
    /// <DEVELOPER> Anderson Simioni </DEVELOPER>
    /// Manages random jump maps so that when you want
    /// to encrypt or decrypt data, also when you need
    /// to create, load and save a map.
    /// [---ALERT---]: For load and save maps use Base64 crypt/decrypt data string
    /// </summary>
    public class Manager
    {
        private Dictionary<string, JumpMap> Maps;
        private readonly Func<string, bool> Save;
        private readonly Func<string> Load;
        private bool Modified = false;

        /// <summary>
        /// Execute save function if not null
        /// </summary>
        public (bool result, string info) callSaveMethod()
        {
            try
            {
                if (Save != null)
                {
                    var json = JsonConvert.SerializeObject(Maps);
                    var cryptedJson = Base64.crypt(json);

                    return (Save(cryptedJson), "Save sucess!!");
                }

                return (false, "Save method not found");
            }
            catch (Exception ex)
            {
                return (false, $"[{ex.StackTrace}]: save fail, info: {ex.Message}");
            }
        }

        /// <summary>
        /// Execute load function if not null
        /// </summary>
        public (bool result, string info) callLoadMethod()
        {
            try
            {
                if (Load != null)
                {
                    var cryptedJson = Load();
                    var decryptedJson = Base64.decrypt(cryptedJson);
                    var maps = JsonConvert.DeserializeObject<Dictionary<string, JumpMap>>(decryptedJson);

                    return (true, "Load sucess!");
                }

                return (false, "Load method not found");
            }
            catch (Exception ex)
            {
                return (false, $"[{ex.StackTrace}]: load fail, info: {ex.Message}");
            }
        }

        /// <summary>
        /// Create new randomJumpMap and Save
        /// </summary>
        /// <param name="mapKey"></param>
        private void generateJumpMap(string mapKey) 
        {
            if (Maps == null)
                Maps = new Dictionary<string, JumpMap>();

            if (!Maps.ContainsKey(mapKey))
            {
                Maps.Add(mapKey, JumpMap.createNewRandomMap());
                Modified = true;
            }  
        }

        /// <summary>
        /// Map key is ("text.len-text.chars.sum-charPosition-password")
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string getMapKey(int length, int charPosition, string password) 
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
        private int getJumpChar(string mapKey, int fromChar) 
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
        private int getReverseJumpChar(string mapKey, int toChar)
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
        public string crypt(string text, string password) 
        {
            callLoadMethod();

            var crypted = "";
            var charArray = text.ToCharArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                var mapKey = getMapKey(text.Length, i, password);
                crypted += getJumpChar(mapKey, charArray[i]) + (i == (charArray.Length - 1) ? ("") : ("x"));
            }

            crypted = Base64.crypt(crypted);

            if (Modified)
            {
                callSaveMethod();
                Modified = false;
            }

            return crypted;
        }

        /// <summary>
        /// Return decrypted text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string decrypt(string text, string password)
        {
            callLoadMethod();

            var decrypted = "";          
            text = Base64.decrypt(text);
            var charArray = text.Split('x').Select(i => int.Parse(i)).ToArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                var mapKey = getMapKey(charArray.Length, i, password);
                decrypted += (char)getReverseJumpChar(mapKey, charArray[i]);
            }

            if (Modified)
            {
                callSaveMethod();
                Modified = false;
            }

            return decrypted;
        }

        public static Manager create(Func<string, bool> saveMethod, Func<string> loadMethod) 
        {
            return new Manager(saveMethod, loadMethod);
        }

        public Manager(Func<string, bool> saveMethod, Func<string> loadMethod) 
        {
            if (saveMethod == null)
                throw new Exception("Save method is null");
            if (loadMethod == null)
                throw new Exception("Load method is null");

            this.Maps = new Dictionary<string, JumpMap>();
            this.Save = saveMethod;
            this.Load = loadMethod;
        }
    }
}
