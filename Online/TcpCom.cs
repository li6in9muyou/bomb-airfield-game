using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Common;
using Easy.Logger.Interfaces;

namespace Online;

public class TcpCom : ICommunicator
{
    private readonly IEasyLogger _note;
    private IPAddress? _ipAddr;
    private StreamReader? _reader;
    private TcpClient? _remote;
    private TcpListener? _server;
    private StreamWriter? _writer;

    public TcpCom()
    {
        _note = Logging.GetLogger("TcpCom");
    }

    public string RemoteHandle()
    {
        if (_ipAddr != null) return _ipAddr.ToString();
        var publicIPv6Address = Utility.FetchPublicIPv6Address();
        _ipAddr = IPAddress.Parse(publicIPv6Address);
        return publicIPv6Address;
    }

    public bool IsLostConnection()
    {
        return false;
    }

    public string Read()
    {
        var t = _reader!.ReadLine();
        _note.Debug($"TcpCom reads: {t}");
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
        _note.Debug($"TcpCom writes: {message}");
    }

    public void Start()
    {
        _server = new TcpListener(IPAddress.Parse(RemoteHandle()), 61234);
        _server.Start();
        _remote = _server.AcceptTcpClient();
        var s = _remote.GetStream();
        _reader = new StreamReader(s);
        _writer = new StreamWriter(s);
    }

    public void Start(string remoteHandle)
    {
        _ipAddr = IPAddress.Parse(remoteHandle);
        _remote = new TcpClient();
        var lastErrorMessage = "";
        for (var retryNum = 0; retryNum < 3; retryNum++)
        {
            if (retryNum > 0) _note.Debug($"\nretryNum = {retryNum}");

            try
            {
                _remote.Connect(_ipAddr, 61234);
                var s = _remote.GetStream();
                _reader = new StreamReader(s);
                _writer = new StreamWriter(s);
                return;
            }
            catch (SocketException e)
            {
                _note.Error("failed");
                _note.Error(e.Message);
                lastErrorMessage = e.Message;
            }
        }

        _note.Info($"give up joining room {_ipAddr}");
        throw new CanNotJoin(lastErrorMessage);
    }

    public void Stop()
    {
        _remote?.Close();
        _server?.Stop();
    }
}