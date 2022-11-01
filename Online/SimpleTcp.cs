using System.Net;
using System.Net.Sockets;

namespace Online;

public class SimpleTcp
{
    public static ICommunicator AsListener()
    {
        return new SimpleTcpServer();
    }


    private class SimpleTcpServer : ICommunicator
    {
        private readonly TcpListener _server;
        private TcpClient? _remote;

        public readonly IPAddress ListeningOn;

        public SimpleTcpServer()
        {
            var ipv6Address = Utility.FetchPublicIPv6Address();
            ListeningOn = IPAddress.Parse(ipv6Address);
            _server = new TcpListener(ListeningOn, 61234);
        }

        public bool IsLostConnection()
        {
            throw new NotImplementedException();
        }

        public string Read()
        {
            if (_remote is null)
            {
                _server.Start();
                _remote = _server.AcceptTcpClient();
                _server.Stop();
                _protocol = new BombAirfieldProtocol(_remote.GetStream());
            }
            throw new NotImplementedException();
        }

        public string Write()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            _server.Stop();
        }
    }
}