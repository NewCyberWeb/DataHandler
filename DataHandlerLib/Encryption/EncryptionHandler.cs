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
        private bool HandshakeInitiated;
        private bool HandshakeConfirmed;
        //how it works: create an object, known as the handshake object, this will be a specific request for the server.
        //in this object there will be a public key of the client, then server then uses this key to encrypt his single private key and send it back.
        //now the client and the server both have the same key to encrypt and decrypt.

        public EncryptionHandler(bool server)
        {
            Handshake = new EncryptionHandshakeHandler();
            ServerMode = server;
            if (ServerMode)
            {
                //generate the key
                AesCryptoServiceProvider p = new AesCryptoServiceProvider();
                p.Mode = CipherMode.CBC;
                PrivateKey = Convert.ToBase64String(p.Key);
            }
        }
        #region clientHandshakeMethods
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
        public bool ConfirmHandshakeObject(HandshakeResponseModel response) //on the client
        {
            if (ServerMode) throw new Exception("Calling clientside encryption method from server");
            //check a couple values, if they are not correct end the function
            if (!HandshakeInitiated)
                return false;
            if (response.PublicKey != Handshake.GetPublicKey())
                return false;
            if (response.ClientSecret != Hashing.Sha256(Handshake.GetClientSecret()))
                return false;


            string data = Handshake.Decrypt(Handshake.GetPrivateKey(), response.Data);


            return false;
        }
        #endregion

        #region serverHandshakeMethods
        public HandshakeResponseModel CreateHandshakeResponseObject(HandshakeRequestModel request) //on the server
        {
            if (!ServerMode) throw new Exception("Calling serverside encryption method from client");
            HandshakeInitiated = true;
            return new HandshakeResponseModel
            {
                ClientSecret = Hashing.Sha256(request.ClientSecret),
                PublicKey = request.PublicKey,

            };
        }
        #endregion


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
