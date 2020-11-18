using DataHandlerLib.Encryption;
using DataHandlerLib.Encryption.Models;
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
            EncryptionHandler server = new EncryptionHandler(true);
            EncryptionHandler client = new EncryptionHandler(false);

            var hObject = client.CreateHandshakeObject(); //client to server
            var hResponseObject = server.CreateHandshakeResponseObject(hObject); //server to client       
            bool hSuccess = client.ConfirmHandshakeObject(hResponseObject, out HandshakeResponseModel response); //client to server
            if (hSuccess)
            {
                bool serverConfirm = server.ConfirmHandshakeObject(response, out HandshakeResponseModel final); //server to client
                if(final == null && serverConfirm)
                {
                    Console.WriteLine("Handshake Complete.");
                }
            }
        }
    }
}
