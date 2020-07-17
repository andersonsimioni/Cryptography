using Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cryptography.RandomKey
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
        private Dictionary<string, Key> RainbowTable;
        private readonly Func<string, bool> Save;
        private readonly Func<string> Load;

        /// <summary>
        /// Execute save function if not null
        /// </summary>
        public (bool result, string info) callSaveMethod()
        {
            try
            {
                if (Save != null)
                {
                    var json = JsonConvert.SerializeObject(RainbowTable);
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
                    var maps = JsonConvert.DeserializeObject<Dictionary<string, Key>>(decryptedJson);

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
        /// Create new random key and set data, password, exp...
        /// </summary>
        /// <param name="data"></param>
        /// <param name="expiration"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string crypt(string data, DateTime expiration, string password = "") 
        {
            callLoadMethod();

            var existingNames = RainbowTable.Select(i => i.Key).ToArray();
            var newKey = Key.create(existingNames, data, expiration, password);

            RainbowTable.Add(newKey.getName(), newKey);
            callSaveMethod();

            return newKey.getName();
        }

        /// <summary>
        /// Return decrypted key data
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public (bool result, string data) decrypt(string keyName, string password = "") 
        {
            callLoadMethod();

            if (RainbowTable.ContainsKey(keyName) == false)
                throw new Exception("Invalid key");

            var key = RainbowTable[keyName];
            var data = key.getData(password);

            return data;
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

            this.RainbowTable = new Dictionary<string, Key>();
            this.Save = saveMethod;
            this.Load = loadMethod;
        }
    }
}
