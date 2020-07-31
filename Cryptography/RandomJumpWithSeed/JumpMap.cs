using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptography.RandomJumpMapWithSeed
{
    /// <summary>
    /// <DEVELOPER> Anderson Simioni </DEVELOPER>
    /// Maped char jumps
    /// </summary>
    class JumpMap
    {
        /// <summary>
        /// Use this var to jump,
        /// Example: ( var crypted = Jump[YourCharToCrypt]; )
        /// </summary>
        [JsonProperty("Jumps")]
        public readonly Dictionary<int, int> Jumps;

        /// <summary>
        /// Reverse a jump, decrypt char.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public int reverseJump(int to) 
        {
            if (to < 0)
                throw new Exception("to is smaller than zero!");

            var from = Jumps.Where(i => i.Value == to);

            if (from == null)
                throw new Exception("'From' not found!");

            return from.First().Key;
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="jumps"></param>
        [JsonConstructor]
        private JumpMap(Dictionary<int, int> jump) 
        {
            if (jump == null)
                throw new Exception("Jumps is null!");

            this.Jumps = jump;
        }

        /// <summary>
        /// Create a new random jump map
        /// </summary>
        /// <returns></returns>
        public static JumpMap createNewRandomMap(int seed) 
        {
            var random = new Random(seed);
            var jumps = new Dictionary<int, int>();
            var notUsedToJumps = new List<int>();

            for (char i = char.MinValue; i < char.MaxValue; i++)
                notUsedToJumps.Add((int)i);           

            for (char i = char.MinValue; i < char.MaxValue; i++)
            {
                var randomToId = (int)(notUsedToJumps.Count() * random.NextDouble());

                jumps.Add(i, notUsedToJumps[randomToId]);
                notUsedToJumps.Remove(notUsedToJumps[randomToId]);
            }

            return new JumpMap(jumps);
        }
    }
}
