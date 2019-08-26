using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.Utilities.PasswordGenerator
{
    public class GenerateRandomPassword
    {
        public static string Generate()
        {
            Random random = new Random();

            string[] characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "k", "z", "y" };
            int[] numbers = new int[] { 32, 15, 14, 647, 245, 2134, 64235, 14135 };

            string password = "";

            int indexOfCharacters = random.Next(0, characters.Length);
            password += characters[indexOfCharacters];

            int indexOfNumbers = random.Next(0, numbers.Length);
            password += numbers[indexOfNumbers];

            indexOfCharacters = random.Next(0, characters.Length);
            password += characters[indexOfCharacters];

            indexOfNumbers = random.Next(0, numbers.Length);
            password += numbers[indexOfNumbers];

            return password;
        }
    }
}
