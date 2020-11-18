using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandlerLib.Encryption.Models
{
    public class SecureDataObject<T>
    {
        public string ClientSecret { get; set; }
        public string IV { get; set; }
        public T DataObject { get; set; }
    }
}
