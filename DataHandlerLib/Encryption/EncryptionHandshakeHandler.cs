using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataHandlerLib.Encryption
{
    public sealed class EncryptionHandshakeHandler
    {
        private readonly string PrivateKeyString;
        private readonly string PublicKeyString;
        private readonly string ClientSecret;
        private const int KEY_SIZE = 2048;

        public string GetPublicKey() => PublicKeyString;
        public string GetPrivateKey() => PrivateKeyString;
        public string GetClientSecret() => ClientSecret;

        public EncryptionHandshakeHandler()
        {
            RSACryptoServiceProvider CryptoServiceProvider = new RSACryptoServiceProvider(KEY_SIZE);
            RSAParameters PrivateKey = CryptoServiceProvider.ExportParameters(true);
            RSAParameters PublicKey = CryptoServiceProvider.ExportParameters(false);
            PrivateKeyString = GetKeyFromRSAParameters(PrivateKey);
            PublicKeyString = GetKeyFromRSAParameters(PublicKey);
            ClientSecret = Guid.NewGuid().ToString();
        }       

        public string Encrypt(string PublicKey, string text)
        {
            RSACryptoServiceProvider CryptoServiceProvider = new RSACryptoServiceProvider(KEY_SIZE);
            CryptoServiceProvider.ImportParameters(GetRSAParametersFromKey(PublicKey));
            byte[] PlainTextBytes = Encoding.Unicode.GetBytes(text);
            return Convert.ToBase64String(CryptoServiceProvider.Encrypt(PlainTextBytes, true));
        }

        public string Decrypt(string PrivateKey, string text)
        {
            RSACryptoServiceProvider CryptoServiceProvider = new RSACryptoServiceProvider(KEY_SIZE);
            CryptoServiceProvider.ImportParameters(GetRSAParametersFromKey(PrivateKey));
            byte[] CypherBytes = Convert.FromBase64String(text);
            return Encoding.Unicode.GetString(CryptoServiceProvider.Decrypt(CypherBytes, true));
        }

        private string GetKeyFromRSAParameters(RSAParameters Key)
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, Key);
            return sw.ToString();
        }

        private RSAParameters GetRSAParametersFromKey(string Key)
        {
            var sr = new StringReader(Key);
            var xs = new XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)xs.Deserialize(sr);
        }
    }
}
