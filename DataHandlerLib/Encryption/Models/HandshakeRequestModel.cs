using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandlerLib.Encryption.Models
{
    public sealed class HandshakeRequestModel
    {
        public string PublicKey { get; set; }
        public string ClientSecret { get; set; }
    }
}
