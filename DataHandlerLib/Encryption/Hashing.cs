using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataHandlerLib.Encryption
{
    public class Hashing
    {
        /// <summary>
        /// Returns the sha256 hash of the input text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Sha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hasher = new SHA256Managed();
            byte[] hashed = hasher.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            foreach(byte b in hashed)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
