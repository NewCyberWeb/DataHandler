using DataHandlerLib.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            EncryptionHandshakeHandler h = new EncryptionHandshakeHandler();
            string enc = h.Encrypt(h.GetPublicKey(), "test");
            string dec = h.Decrypt(h.GetPrivateKey(), enc);

            Console.WriteLine(dec);
        }
    }
}
