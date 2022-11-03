using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Online;

public class TcpCom : ICommunicator
{
    private IPAddress? _ipAddr;
    private StreamReader? _reader;
    private TcpClient? _remote;
    private TcpListener? _server;
    private StreamWriter? _writer;

    private TcpCom()
    {
    }

    public string RemoteHandle()
    {
        return _ipAddr!.ToString();
    }

    public bool IsLostConnection()
    {
        return false;
    }

    public string Read()
    {
        var t = _reader!.ReadLine();
        Console.Out.WriteLine("TcpCom reads: {0}", t);
        if (t is null) throw new Exception();

        return t;
    }

    public string Expect(Regex expected)
    {
        var t = Read();
        if (!expected.IsMatch(t)) throw new ExpectMismatch(expected, t);
        return t;
    }

    public void Write(string message)
    {
        _writer!.WriteLine(message);
        _writer.Flush();
        Console.Out.WriteLine("TcpCom writes: {0}", message);
    }

    public void Start()
    {
        if (_server is not null)
        {
            _server.Start();
            _remote = _server.AcceptTcpClient();
        }
        else
        {
            var lastErrorMessage = "";
            for (var retryNum = 0; retryNum < 3; retryNum++)
            {
                if (retryNum > 0) Console.Out.WriteLine("\nretryNum = {0}", retryNum);

                try
                {
                    _remote!.Connect(_ipAddr!, 61234);
                    break;
                }
                catch (SocketException e)
                {
                    Console.Out.WriteLine("failed");
                    Console.WriteLine(e.Message);
                    lastErrorMessage = e.Message;
                }
            }

            Console.Out.WriteLine("give up joining room {0}", _ipAddr);
            throw new CanNotJoin(lastErrorMessage);
        }

        var s = _remote.GetStream();
        _reader = new StreamReader(s);
        _writer = new StreamWriter(s);
    }

    public void Stop()
    {
        _remote?.Close();
        _server?.Stop();
    }

    public static TcpCom AsServer()
    {
        var ipAddress = IPAddress.Parse(
            Utility.FetchPublicIPv6Address()
        );
        return new TcpCom
        {
            _server = new TcpListener(ipAddress, 61234),
            _ipAddr = ipAddress
        };
    }

    public static TcpCom AsClient(IPAddress ipAddress)
    {
        return new TcpCom
        {
            _remote = new TcpClient(),
            _ipAddr = ipAddress
        };
    }
}