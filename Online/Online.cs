using System.Net;
using Common;

namespace Online;

public class Online
{
    private ICommunicator? _com;

    public Online()
    {
    }

    public Online(ICommunicator com)
    {
        _com = com;
    }

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
        return BombAnAirplaneProtocol.DoHandShake(_com);
    }
    
    public bool WaitJoinOpponentRoom()
    {
        // connect and handshake
        _com!.Start();
        return BombAnAirplaneProtocol.DoHandShake(_com!);
    }

    public string CreateRoom()
    {
        return _com!.RemoteHandle();
    }

    public bool WaitOpponentToJoin()
    {
        _com!.Start();
        return BombAnAirplaneProtocol.DoHandShake(_com!);
    }

    public void WaitOpponentPlaceAirplane()
    {
        BombAnAirplaneProtocol.WaitOpponentPlaceAirPlane(_com!);
    }

    public void NotifyLocalReady()
    {
        BombAnAirplaneProtocol.NotifyLocalReady(_com!);
    }

    public BombResult BombOpponentAirfieldAndWaitResult(Coordinate coordinate)
    {
        return BombAnAirplaneProtocol.BombOpponentAirfieldAndWaitResult(_com!, coordinate);
    }

    public Coordinate WaitOpponentToBombMyAirfield()
    {
        return BombAnAirplaneProtocol.WaitOpponentToBombMyAirfield(_com!);
    }

    public void SendBombResultOfMyAirfield(BombResult result)
    {
        BombAnAirplaneProtocol.SendBombResultOfMyAirfield(_com!, result);
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