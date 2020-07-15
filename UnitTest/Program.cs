using System;
using Cryptographic;

namespace UnitTest
{
    class Program
    {
        static void testBase64() 
        {
            Console.WriteLine("-> Base64 <-");
            for (int i = 1; i <= 5; i++)
                Console.WriteLine($"    [Level {i}]: 'Hello world' = {Base64.crypt("Hello world", i)}");
            Console.WriteLine("____________\n\n\n");
        }

        static void testRandomJumpMap()
        {
            Console.WriteLine("-> RandomJumpMap <-");

            var saveStringAux = "";
            var manager = Cryptographic.RandomJumpMap.Manager.create(
                //save method
                (data) => 
                {
                    saveStringAux = data;
                    return true;
                },
                //load mehtod
                () => 
                { 
                    return saveStringAux; 
                });

            var pass = "123";
            var data = "Hellow world";
            var cryptedData = manager.crypt(data, pass);
            var decryptedData = manager.decrypt(cryptedData, pass);

            Console.WriteLine($"Password = {pass}\n");
            Console.WriteLine($"    '{data}' = {cryptedData}\n");
            Console.WriteLine($"    {cryptedData} = '{decryptedData}'");
            Console.WriteLine("___________________\n\n\n");
        }

        static void testRandomKeyMap() 
        {
            Console.WriteLine("-> RandomJumpMap <-");
            var data = "Hellow world";
            var cryptedData = "";
            var decryptedData = "";

            Console.WriteLine($"    '{data}' = {cryptedData}");
            Console.WriteLine($"    {cryptedData} = '{decryptedData}'");
            Console.WriteLine("___________________\n\n\n");
        }

        static void Main(string[] args)
        {
            Console.Title = ("---UNIT TEST---");

            testBase64();
            testRandomJumpMap();
            testRandomKeyMap();

            Console.ReadLine();
        }
    }
}
