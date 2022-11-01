using System;
using System.Net;
using Common;

namespace Online;

public class Online
{
    private TcpCom? _com;

    public void AbandonRoom()
    {
        _com!.Stop();
    }

    public bool WaitJoinOpponentRoom(string ipAddress)
    {
        // connect and handshake
        var ipa = IPAddress.Parse(ipAddress);
        _com = TcpCom.AsClient(ipa);
        _com.Start();
        return BombAirfieldProtocol.DoHandShake(_com);
    }

    public string CreateRoom()
    {
        _com = TcpCom.AsServer();
        return _com.ListenOn;
    }

    public bool WaitOpponentToJoin()
    {
        _com!.Start();
        return BombAirfieldProtocol.DoHandShake(_com);
    }

    public void WaitOpponentPlaceAirplane()
    {
        BombAirfieldProtocol.WaitOpponentPlaceAirPlane(_com!);
    }

    public void NotifyLocalReady()
    {
        BombAirfieldProtocol.NotifyLocalReady(_com!);
    }

    public BombResult BombOpponentAirfieldAndWaitResult(Coordinate coordinate)
    {
        return BombAirfieldProtocol.BombOpponentAirfieldAndWaitResult(_com!, coordinate);
    }

    public Coordinate WaitOpponentToBombMyAirfield()
    {
        return BombAirfieldProtocol.WaitOpponentToBombMyAirfield(_com!);
    }

    public void SendBombResultOfMyAirfield(BombResult result)
    {
        BombAirfieldProtocol.SendBombResultOfMyAirfield(_com!, result);
    }

    public bool IsRemoteConnectionLost()
    {
        return _com!.IsLostConnection();
    }
}

public class CanNotJoin : Exception
{
    public CanNotJoin(string? message) : base(message)
    {
    }
}