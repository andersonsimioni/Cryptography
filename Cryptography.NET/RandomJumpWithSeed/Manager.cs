using Cryptography.RandomJumpMapWithSeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography.RandomJumpWithSeed
{
    /// <summary>
    /// <DEVELOPER> Anderson Simioni </DEVELOPER>
    /// Manages random jump maps so that when you want
    /// to encrypt or decrypt data, also when you need
    /// to create, load and save a map.
    /// </summary>
    public class Manager
    {
        private readonly int BaseSeed;
        private readonly Dictionary<int, JumpMap> Maps;

        /// <summary>
        /// Check if all seeds maps exist and if not, create
        /// </summary>
        /// <param name="seeds"></param>
        private void ValidateMapWithSeedSequence(int[] seeds) 
        {
            var notFounds = seeds
                .Distinct()
                .Where(s => Maps.ContainsKey(s) == false)
                .ToArray();

            if (notFounds == null)
                return;

            foreach (var seed in notFounds)
                Maps.Add(seed, JumpMap.createNewRandomMap(seed));         
        }

        /// <summary>
        /// Generate random jump map with seed 
        /// crypted word with password
        /// </summary>
        /// <param name="data"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Crypt(string data, string password) 
        {
            var dataIntChars = data.ToCharArray().Select(c => (int)c).ToArray();
            var jmpSequence = GetMapsSequence(password);

            ValidateMapWithSeedSequence(jmpSequence);

            foreach (var seed in jmpSequence)
                for (int i = 0; i < dataIntChars.Length; i++)
                    dataIntChars[i] = Maps[seed].Jumps[dataIntChars[i]];

            var cryptedData = "";
            foreach (var item in dataIntChars)
                cryptedData += $"x{item}";
            
            return Base64.crypt(cryptedData);
        }

        /// <summary>
        /// Get reverse jumps sequence to decrypt data and return
        /// </summary>
        /// <param name="data"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Decrypt(string data, string password) 
        {
            var decode = Base64.decrypt(data);
            var values = decode.Split('x');

            try
            {
                var jumpSequence = GetMapsSequence(password);
                var intCharArray = values
                    .Where(c => string.IsNullOrEmpty(c) == false)
                    .Select(v => int.Parse(v))
                    .ToArray();

                ValidateMapWithSeedSequence(jumpSequence);

                for (int i = jumpSequence.Length - 1; i >= 0; i--)
                {
                    var mapSeed = jumpSequence[i];
                    intCharArray = intCharArray.Select(c => Maps[mapSeed].reverseJump(c)).ToArray();
                }

                var decrypted = "";
                foreach (var item in intCharArray)
                    decrypted += (char)item;

                return decrypted;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid format, " + ex.Message);
            }
        }

        /// <summary>
        /// Return array of int and each item is a jump map index
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private int[] GetMapsSequence(string password) 
        {
            return password.ToCharArray().Select(c => BaseSeed + (int)c).ToArray();
        }

        /// <summary>
        /// Extract seed from word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static int CalculateWordSeed(string word) 
        {
            return word.ToCharArray().Select(c => (int)c).Sum();
        }

        /// <summary>
        /// Create new random jump map with 
        /// calculated base seed from password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Manager Create(string password) 
        {
            var seed = CalculateWordSeed(password);
            return new Manager(seed);
        }

        private Manager(int baseSeed) 
        {
            this.BaseSeed = baseSeed;
            this.Maps = new Dictionary<int, JumpMap>();
        }
    }
}
