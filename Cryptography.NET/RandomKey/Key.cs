using Cryptography;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace Cryptography.RandomKey
{
    /// <summary>
    /// <DEVELOPER> [Anderson Simioni 15/07/2020] </DEVELOPER>
    /// Random map key that will be a random name 
    /// generated and store values.
    /// </summary>
    public class Key
    {
        [JsonProperty("Name")]
        private readonly string Name;

        [JsonProperty("Password")]
        private readonly string Password;

        [JsonProperty("Data")]
        private readonly string Data;

        [JsonProperty("RegisterDate")]
        private readonly DateTime RegisterDate;

        [JsonProperty("ExpirationDate")]
        private readonly DateTime ExpirationDate;

        //Simple getter
        public string getName() 
        {
            return Name;
        }

        public string getCryptPassword() 
        {
            return Password;
        }

        public string getCryptData() 
        {
            return Data;
        }

        public DateTime getRegisterDate() 
        {
            return RegisterDate;
        }

        public DateTime getExpDate() 
        {
            return ExpirationDate;
        }

        /// <summary>
        /// Try acess key data with password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public (bool result, string data) getData(string password = "") 
        {
            var accessAccepted = 
                string.IsNullOrEmpty(this.Password) || 
                (this.Password == Base64.crypt(password));

            if (ExpirationDate != null)
                if ((ExpirationDate - DateTime.Now).TotalSeconds <= 0)
                    return (false, "Expired");

            if (accessAccepted) 
            {
                var decryptedData = Base64.decrypt(Data);
                return (true, decryptedData);
            }

            return (false, "Access denied");
        }

        /// <summary>
        /// Create new random name, it not exist on names list
        /// </summary>
        /// <param name="length"></param>
        /// <param name="existingNames"></param>
        /// <returns></returns>
        private static string getNewRandomName(int length, params string[] existingNames) 
        {
            if (length <= 0)
                throw new Exception("Invalid length");

            var newName = string.Empty;
            var randomGen = new Random(DateTime.Now.Millisecond);

            while (newName == string.Empty || existingNames.Contains(newName))
            {
                newName = "";

                for (int i = 0; i < length; i++)
                    newName += (int)((char)randomGen.Next(char.MinValue, char.MaxValue)) + (i == length - 1 ? "" : "x");

                newName = Base64.crypt(newName);
            }

            return newName;
        }

        /// <summary>
        /// Create new random key with new name and
        /// save data and password, set expiration date 
        /// to null for not have
        /// </summary>
        /// <param name="existingNames"></param>
        /// <param name="data"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Key create(string[] existingNames, string data, DateTime expirationDate, string password = "") 
        {     
            var cryptedData = Base64.crypt(data);
            var cryptedPassword = Base64.crypt(password);
            var name = getNewRandomName(data.Length, existingNames);

            return new Key(name, cryptedPassword, cryptedData, DateTime.Now, expirationDate);
        }

        [JsonConstructor]
        public Key(string Name, string Password, string Data, DateTime RegisterDate, DateTime ExpirationDate) 
        {
            if (string.IsNullOrEmpty(Name))
                throw new Exception("Name is empty or null!");

            this.Name = Name;
            this.Password = Password;
            this.Data = Data;
            this.RegisterDate = RegisterDate;
            this.ExpirationDate = ExpirationDate;
        }
    }
}
