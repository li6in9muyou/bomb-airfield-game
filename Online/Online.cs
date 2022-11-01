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
        var apis = new Dictionary<string, Func<string, string>>
        {
            { "https://ip6only.me/api/", s => s.Split(",")[1] },
            { "https://api-ipv6.ip.sb/ip", s => s }
        };
        using var c = new HttpClient();
        foreach (var (api, trans) in apis)
            try
            {
                var t = c.GetStringAsync(api);
                t.Wait();
                var ipv6Address = trans(t.Result);
                Console.Out.WriteLine("ipv6Address = {0}", ipv6Address);
                return ipv6Address;
            }
            catch (HttpRequestException)
            {
            }

        throw new Exception("can not get fetch ip address of this machine");
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
        var (r, _) = GetRemoteRoomChannel();
        var ok = r.ReadLine();
        if (ok is not null && ok == "ok")
            return;
        throw new Exception();
    }

    public void NotifyLocalReady()
    {
        var (_, w) = GetRemoteRoomChannel();
        w.WriteLine("ok");
        w.Flush();
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
        Console.Out.WriteLine("where = {0}", where);
        var coordinates = where!.Split(',');
        var x = coordinates[0];
        var y = coordinates[1];
        var xN = int.Parse(x);
        var yN = int.Parse(y);
        Console.Out.WriteLine("x = {0}", xN);
        Console.Out.WriteLine("y = {0}", yN);
        return new Coordinate(xN, yN);
    }

    public void SendBombResultOfMyAirfield(BombResult result)
    {
        var (_, w) = GetRemoteRoomChannel();
        w.WriteLine(result.ToString().ToLower());
        w.Flush();
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