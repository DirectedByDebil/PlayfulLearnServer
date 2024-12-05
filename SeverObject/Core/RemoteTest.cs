using System;

namespace Core
{

    public sealed class RemoteTest : MarshalByRefObject
    {

        public int TestIndex { get; private set; }


        public RemoteTest(int testIndex)
        {

            TestIndex = testIndex;
        }
    }
}
