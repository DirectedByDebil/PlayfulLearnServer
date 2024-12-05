using System;
using System.Collections.Generic;

namespace Server
{

    [Serializable]
    public struct RegistrationFields
    {

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<byte> Password { get; set; }

        public string DateTime { get; set; }
    }
}
