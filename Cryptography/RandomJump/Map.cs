using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptographic
{
    /// <summary>
    /// <DEVELOPER> Anderson Simioni </DEVELOPER>
    /// Maped char jumps
    /// </summary>
    class Map
    {
        /// <summary>
        /// Use this var to jump,
        /// Example: ( var crypted = Jump[YourCharToCrypt]; )
        /// </summary>
        public Dictionary<char, char> Jump { get; }

        /// <summary>
        /// Reverse a jump, decrypt char.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public char reverseJump(char to) 
        {
            var from = Jump.Where(i => i.Value == to);

            if (from == null)
                throw new Exception("'From' not found!");

            return from.First().Key;
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="jumps"></param>
        [JsonConstructor]
        private Map(Dictionary<char, char> jump) 
        {
            if (jump == null)
                throw new Exception("Jumps is null!");

            this.Jump = jump;
        }

        /// <summary>
        /// Create a new random jump map
        /// </summary>
        /// <returns></returns>
        public static Map createNewRandomMap() 
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var jumps = new Dictionary<char, char>();
            var notUsedToJumps = new List<char>();                  

            for (char i = char.MinValue; i < char.MaxValue; i++)
                notUsedToJumps.Add(i);

            for (char i = char.MinValue; i < char.MaxValue; i++)
            {
                var randomToId = (int)(notUsedToJumps.Count() * random.NextDouble());

                jumps.Add(i, notUsedToJumps[randomToId]);
                notUsedToJumps.Remove(notUsedToJumps[randomToId]);
            }

            return new Map(jumps);
        }
    }
}
