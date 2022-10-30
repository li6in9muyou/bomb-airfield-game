using GameLogic;

namespace Online
{
    public class Online
    {
        Online() { }

        void WaitOpponentPlaceAirplane() { }

        BombResult BombOpponentAirfieldAndWaitResult(Coordinate coordinate) { }
        
        Coordinate WaitOpponentToBombMyAirfield() { }
        
        void SendBombResultOfMyAirfield(BombResult result) { }

        bool IsRemoteConnectionLost() { }
    }
}