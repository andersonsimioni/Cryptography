using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace hasbots.Classes.Cryptographic.RandomKeyMap
{
    /// <summary>
    /// Manages maps of random keys and their values,
    /// for load and save maps use Base64 crypt/decrypt data string
    /// </summary>
    class Map
    {
        /// <summary>
        /// Keys and data dictionary
        /// </summary>
        public Dictionary<string, Key> DIC { get; }

        /// <summary>
        /// Create a new key, this new key not exist in DIC
        /// </summary>
        /// <param name="data">data for link with the new key</param>
        /// <returns>new key name</returns>
        public string addKey(string data)
        {
            var key = Key.createNewRandomKey(data);

            while (DIC.ContainsKey(key.Name))
                key = Key.createNewRandomKey(data);

            DIC.Add(key.Name, key);

            return key.Name;
        }

        /// <summary>
        /// Find in DIC a key with name equal key var
        /// </summary>
        /// <param name="key">name of key to find</param>
        /// <returns>key data value</returns>
        public string getData(string key)
        {
            if (!DIC.ContainsKey(key))
                throw new Exception("Key doesnt exist!");

            return DIC[key].Value;
        }

        /// <summary>
        /// Save the DIC in a file to load another time
        /// </summary>
        /// <param name="file">path where load file</param>
        public void save(string file)
        {
            //if (DIC == null)
            //    throw new Exception("Null map");

            //var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            //try
            //{
            //    ServerDirectory.Files.writeText(file, serialized);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("File problem: " + ex.Message);
            //}
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dic">DIC to set in the new Map class</param>
        [JsonConstructor]
        private Map(Dictionary<string, Key> dic = null)
        {
            if (dic == null)
                this.DIC = new Dictionary<string, Key>();
            else
                this.DIC = dic;
        }

        /// <summary>
        /// Load exist DIC from file
        /// </summary>
        /// <param name="file">path from load file</param>
        /// <returns>Map loaded from file</returns>
        public static Map loadFromFile(string file)
        {
            //if (!ServerDirectory.Files.fileExist(file))
            //    throw new Exception("File not exist!");

            //var readedData = ServerDirectory.Files.readText(file);

            try
            {
                //var deserialized = JsonConvert.DeserializeObject<Map>(readedData);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid file format: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Create a new empty key Map
        /// </summary>
        /// <returns>new empty key map</returns>
        public static Map createNew()
        {
            return new Map();
        }
    }
}
