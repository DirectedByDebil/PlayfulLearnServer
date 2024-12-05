using System;

namespace Server
{

    [Serializable]
    public struct IdKeys
    {

        public byte[] Salt { get; set; }

        public string DateTime { get; set; }
    }
}