using GameLogic;
using ClassLibrary1;

namespace UserInterface
{
    public class UserInterfaceAdater
    {
        UserInterfaceAdater() { }

        void DrawAdditionalContent() { }

        void DrawGameLogic(GameLogic.GameLogic game) { }

        Coordinate WaitLocalUserChooseBombLocation() { }

        AirplanePlacement[] WaitLocaluserPlaceAirplanes() { }

        string WaitInputIPAddress() { }
    }
}