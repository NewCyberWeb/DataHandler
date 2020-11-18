using DataHandlerLib.Encryption.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataHandlerLib.Encryption
{
    /// <summary>
    /// Encryption handler for data transport, object should have one instance per client.
    /// </summary>
    public class EncryptionHandler
    {
        private EncryptionHandshakeHandler Handshake;
        private string PrivateKey;
        private readonly bool ServerMode;

        public bool HandshakeConfirmed { get; private set; }
        public bool HandshakeInitiated { get; private set; }

        public EncryptionHandler(bool server)
        {
            Handshake = new EncryptionHandshakeHandler();
            HandshakeConfirmed = false;
            HandshakeInitiated = false;
            ServerMode = server;
            if (ServerMode)
            {
                //generate the key
                AesCryptoServiceProvider p = new AesCryptoServiceProvider();
                p.Mode = CipherMode.CBC;
                PrivateKey = Convert.ToBase64String(p.Key);
            }
        }

        public HandshakeRequestModel CreateHandshakeObject() //from the client side.
        {
            if (ServerMode) throw new Exception("Calling clientside encryption method from server");

            HandshakeInitiated = true;
            return new HandshakeRequestModel
            {
                PublicKey = Handshake.GetPublicKey(),
                ClientSecret = Handshake.GetClientSecret()
            };
        }
        public bool ConfirmHandshakeObject(HandshakeResponseModel response, out HandshakeResponseModel output) //on the client or te server
        {
            string match = "CONFIRMED_HANDSHAKE";
            output = null;
            //check a couple values, if they are not correct end the function
            if (!HandshakeInitiated)
                return false;
            if (response.PublicKey == Handshake.GetPublicKey())
                return false;
            

            if (ServerMode)
            {               
                string message = Handshake.Decrypt(Handshake.GetPrivateKey(), response.Data);
                if(message == match)             
                    return HandshakeConfirmed = true;           
            } 
            else
            {
                if (response.ClientSecret != Hashing.Sha256(Handshake.GetClientSecret()))
                    return false;
                PrivateKey = Handshake.Decrypt(Handshake.GetPrivateKey(), response.Data);
                HandshakeConfirmed = true;
                output = new HandshakeResponseModel
                {
                    ClientSecret = Handshake.GetClientSecret(),
                    PublicKey = Handshake.GetPublicKey(),
                    Data = Handshake.Encrypt(response.PublicKey, match)
                };
                return true;
            }
            return false;
        }

        public HandshakeResponseModel CreateHandshakeResponseObject(HandshakeRequestModel request) //on the server
        {
            if (!ServerMode) throw new Exception("Calling serverside encryption method from client.");
            HandshakeInitiated = true;
            return new HandshakeResponseModel
            {
                ClientSecret = Hashing.Sha256(request.ClientSecret),
                PublicKey = Handshake.GetPublicKey(),
                Data = Handshake.Encrypt(request.PublicKey, PrivateKey)
            };
        }


        public bool InHandshakeMode()
        {
            return HandshakeConfirmed && HandshakeInitiated;
        }

        public void Encrypt(string text)
        {
            if (!HandshakeInitiated || !HandshakeConfirmed) throw new Exception("Handshake not completed, no encryption possible.");
        }

        public void Decrypt(string text)
        {
            if (!HandshakeInitiated || !HandshakeConfirmed) throw new Exception("Handshake not completed, no decryption possible.");
        }
    }
}
