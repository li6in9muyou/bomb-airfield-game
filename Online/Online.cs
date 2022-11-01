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

    private static bool DoHandShake((StreamReader r, StreamWriter w) readAndWrite)
    {
        var (r, w) = readAndWrite;

        var myNumber = new Random().Next(0, 100);
        Console.Out.WriteLine("myNumber = {0}", myNumber);
        w.WriteLine(myNumber);
        w.Flush();
        var received = r.ReadLine();
        Console.Out.WriteLine("received = {0}", received);
        var opponentNumber = int.Parse(received!);
        Console.Out.WriteLine("opponent_number = {0}", opponentNumber);
        Console.Out.WriteLine("handshake completed");
        return myNumber > opponentNumber;
    }

    public bool WaitJoinOpponentRoom(string ipAddress)
    {
        // connect and handshake
        var ipa = IPAddress.Parse(ipAddress);
        _remoteRoom = new TcpClient();

        var lastErrorMessage = "";
        for (var retryNum = 0; retryNum < 3; retryNum++)
        {
            if (retryNum > 0) Console.Out.WriteLine("\nretryNum = {0}", retryNum);

            try
            {
                _remoteRoom.Connect(ipa, 61234);
                return DoHandShake(GetRemoteRoomChannel());
            }
            catch (SocketException e)
            {
                Console.Out.WriteLine("failed");
                Console.WriteLine(e.Message);
                lastErrorMessage = e.Message;
            }
        }

        Console.Out.WriteLine("give up joining room {0}", ipa);
        throw new CanNotJoin(lastErrorMessage);
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

        return DoHandShake(GetRemoteRoomChannel());
    }

    public void WaitOpponentPlaceAirplane()
    {
    }

    public BombResult BombOpponentAirfieldAndWaitResult(Coordinate coordinate)
    {
        var (r, w) = GetRemoteRoomChannel();
        var where = coordinate.X + "," + coordinate.Y;
        w.WriteLine(where);
        w.Flush();
        var result = r.ReadLine();
        switch (result)
        {
            case "hit":
            {
                return BombResult.Hit;
            }
            case "miss":
            {
                return BombResult.Miss;
            }
            case "destroy":
            {
                return BombResult.Destroyed;
            }
            default:
            {
                Console.Out.WriteLine("result = {0}", result);
                throw new Exception();
            }
        }
    }

    public Coordinate WaitOpponentToBombMyAirfield()
    {
        var (r, _) = GetRemoteRoomChannel();
        var where = r.ReadLine();
        var coordinates = where!.Split(',');
        var x = coordinates[0];
        var y = coordinates[1];
        var xN = int.Parse(x);
        var yN = int.Parse(y);
        return new Coordinate(xN, yN);
    }

    public void SendBombResultOfMyAirfield(BombResult result)
    {
    }

    public bool IsRemoteConnectionLost()
    {
        return false;
    }

    private (StreamReader, StreamWriter) GetRemoteRoomChannel()
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

        return (_reader!, _writer!);
    }
}

public class CanNotJoin : Exception
{
    public CanNotJoin(string? message) : base(message)
    {
    }
}