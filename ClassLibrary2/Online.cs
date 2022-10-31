using Common;

namespace Online;

public class Online
{
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
}