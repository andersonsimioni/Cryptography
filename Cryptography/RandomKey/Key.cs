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
    /// This class represent a key with data linked
    /// </summary>
    class Key
    {
        private readonly static char[] printables = ("qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890").ToCharArray();

        /// <summary>
        /// Name for call this key
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// data value of this key
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Date of creation this key
        /// </summary>
        public DateTime CreationDate { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">data for link to this key</param>
        [Newtonsoft.Json.JsonConstructor]
        public Key(string name, string value, DateTime creationDate) 
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Null or empty key value!");

            if (string.IsNullOrEmpty(name))
                throw new Exception("Name is null or empty!");

            if (creationDate == null)
                throw new Exception("Creation date is invalid!");

            this.Name = name;
            this.Value = value;
            this.CreationDate = creationDate;
        }

        /// <summary>
        /// Create a new random key
        /// </summary>
        /// <param name="data">key value</param>
        /// <returns>new random key</returns>
        public static Key createNewRandomKey(string data) 
        {
            var random = new Random((int)DateTime.Now.Ticks);

            string key = "";
            var strong = data.Length * 2 + (int)(random.NextDouble() * data.Length);

            while (key.Length < strong)
                key += printables[(int)(random.NextDouble() * printables.Length)];

            return new Key(key, data, DateTime.Now);
        }
    }
}
