using System;
using System.Linq;

namespace CoreApp
{
    public class RandomStringGenerator
    {
        /// <summary>
        /// Random String generator with help of https://stackoverflow.com/a/1344242
        /// </summary>
        /// <param name="gen">The Random generator</param>
        /// <param name="length">String Length</param>
        /// <returns>Random String</returns>
        public static string RandomString(Random gen, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[gen.Next(s.Length)]).ToArray());
        }
    }
}
