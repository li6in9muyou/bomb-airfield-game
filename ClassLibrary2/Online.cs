using System.Net;
using System.Net.Sockets;
using Common;

namespace Online;

public class Online
{
    private StreamReader? _reader;
    private TcpClient? _remoteRoom;
    private TcpListener? _room;
    private StreamWriter? _writer;

    public void AbandonRoom()
    {
        if (_remoteRoom is not null)
            _remoteRoom.Close();
        else
            Console.Out.WriteLine("_remote_room is null in Online.AbandonRoom()");

        if (_room is not null)
            _room.Stop();
        else
            Console.Out.WriteLine("_room is null in Online.AbandonRoom()");
    }

    private bool DoHandShake()
    {
        var (r, w) = GetRemoteRoomChannel();

        var myNumber = new Random().Next(0, 100);
        Console.Out.WriteLine("myNumber = {0}", myNumber);
        w.WriteLine(myNumber);
        var received = r.ReadLine();
        Console.Out.WriteLine("received = {0}", received);
        var opponentNumber = int.Parse(received!);
        Console.Out.WriteLine("opponent_number = {0}", opponentNumber);
        Console.Out.WriteLine("handshake completed");
        return myNumber > opponentNumber;
    }

    public bool WaitJoinOpponentRoom(string ipAddress)
    {
        // todo: 异常，这个地址连接超时
        // todo: 异常，这个地址不正确
        // connect and handshake
        var ipa = IPAddress.Parse(ipAddress);
        _remoteRoom = new TcpClient();
        _remoteRoom.Connect(ipa, 61234);
    }

    private static string FetchPublicIPv6Address()
    {
        using var c = new HttpClient();
        var t = c.GetStringAsync("https://ip6only.me/api/");
        // todo: 如果这个地址访问不到怎么办，搞多几个这样的api作为后备。
        t.Wait();
        var ipv6Address = t.Result.Split(",")[1];
        Console.Out.WriteLine("ipv6Address = {0}", ipv6Address);
        return ipv6Address;
    }

    public string CreateRoom()
    {
        var ipv6Address = FetchPublicIPv6Address();
        _room = new TcpListener(IPAddress.Parse(ipv6Address), 61234);
        return ipv6Address;
    }

    public bool WaitOpponentToJoin()
    {
        if (_room is null)
        {
            Console.Out.WriteLine("game room is not yet created, call CreateRoom() first");
            throw new Exception();
        }

        _room.Start();
        _remoteRoom = _room.AcceptTcpClient();
        Console.Out.WriteLine("a client is connected");
        _room.Stop();
        Console.Out.WriteLine("stop listening on this port");

        return DoHandShake();
    }

    public void WaitOpponentPlaceAirplane()
    {
    }

    public BombResult BombOpponentAirfieldAndWaitResult(Coordinate coordinate)
    {
        return BombResult.Miss;
    }

    public Coordinate WaitOpponentToBombMyAirfield()
    {
        return new Coordinate(5, 5);
    }

    public void SendBombResultOfMyAirfield(BombResult result)
    {
    }

    public bool IsRemoteConnectionLost()
    {
        return false;
    }

    private ReadAndWrite GetRemoteRoomChannel()
    {
        if (_remoteRoom is null)
        {
            Console.Out.WriteLine("_remote_room is null, cannot get remote room channel now");
            throw new Exception();
        }

        if (_reader is null && _writer is null)
        {
            var s = _remoteRoom.GetStream();
            _reader = new StreamReader(s);
            _writer = new StreamWriter(s);
        }

        return new ReadAndWrite(_reader!, _writer!);
    }

    private record ReadAndWrite(StreamReader Reader, StreamWriter Writer);
}