using Common;

namespace Online;

public class Online
{
    private readonly ICommunicator _com;

    public Online() : this(new TcpCom())
    {
    }

    public Online(ICommunicator com)
    {
        _com = com;
    }

    public void AbandonRoom()
    {
        _com.Stop();
    }

    public bool WaitJoinOpponentRoom(string remoteHandle)
    {
        // connect and handshake
        _com.Start(remoteHandle);
        return BombAnAirplaneProtocol.DoHandShake(_com);
    }

    public string CreateRoom()
    {
        return _com.RemoteHandle();
    }

    public bool WaitOpponentToJoin()
    {
        _com.Start();
        return BombAnAirplaneProtocol.DoHandShake(_com);
    }

    public void WaitSurrenderToOpponent()
    {
        BombAnAirplaneProtocol.WaitSurrenderToOpponent(_com);
    }

    public void WaitNotifyStillStanding()
    {
        BombAnAirplaneProtocol.WaitNotifyStillStanding(_com);
    }

    public bool WaitOpponentToSurrender()
    {
        return BombAnAirplaneProtocol.WaitOpponentToSurrender(_com);
    }

    public void WaitOpponentPlaceAirplane()
    {
        BombAnAirplaneProtocol.WaitOpponentPlaceAirPlane(_com);
    }

    public void NotifyLocalReady()
    {
        BombAnAirplaneProtocol.NotifyLocalReady(_com);
    }

    public BombResult BombOpponentAirfieldAndWaitResult(Coordinate coordinate)
    {
        return BombAnAirplaneProtocol.BombOpponentAirfieldAndWaitResult(_com, coordinate);
    }

    public Coordinate WaitOpponentToBombMyAirfield()
    {
        return BombAnAirplaneProtocol.WaitOpponentToBombMyAirfield(_com);
    }

    public void SendBombResultOfMyAirfield(BombResult result)
    {
        BombAnAirplaneProtocol.SendBombResultOfMyAirfield(_com, result);
    }

    public bool IsRemoteConnectionLost()
    {
        return _com.IsLostConnection();
    }
}

public class CanNotJoin : Exception
{
    public CanNotJoin(string? message) : base(message)
    {
    }
}