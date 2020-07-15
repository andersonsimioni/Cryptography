using System;
using Cryptographic;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = ("---UNIT TEST---");

            Console.WriteLine("-> Base64 <-");
            for (int i = 1; i <= 5; i++)
                Console.WriteLine($"    [Level {i}]: 'Hello world' = {Base64.crypt("Hello world", i)}");
            Console.WriteLine("____________\n\n\n");

            Console.WriteLine("-> RandomJumpMap <-");
            var data = "Hellow world";
            var cryptedData = Cryptographic.RandomJumpMap.Manager.crypt(data, "123");
            var decryptedData = Cryptographic.RandomJumpMap.Manager.decrypt(cryptedData, "123");

            Console.WriteLine($"    '{data}' = {cryptedData}");
            Console.WriteLine($"    {cryptedData} = '{decryptedData}'");
            Console.WriteLine("___________________\n\n\n");

            Console.ReadLine();
        }
    }
}
