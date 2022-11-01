using System.Net;

namespace Online;

public class SimpleTcp
{
    public static ICommunicator AsListener()
    {
        return new SimpleTcpServer();
    }


    private class SimpleTcpServer : ICommunicator
    {
        public IPAddress ListeningOn = IPAddress.None;

        public bool IsLostConnection()
        {
            throw new NotImplementedException();
        }

        public string Read()
        {
            throw new NotImplementedException();
        }

        public string Write()
        {
            throw new NotImplementedException();
        }
    }
}