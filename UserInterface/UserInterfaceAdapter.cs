using Common;
using GameLogic;

namespace UserInterface;

public class UserInterfaceAdapter
{
    public void DrawAdditionalContent(string message)
    {
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
    }

    public Coordinate WaitLocalUserChooseBombLocation()
    {
        return Coordinate.Void();
    }

    public AirplanePlacement[] WaitLocaluserPlaceAirplanes()
    {
        return Array.Empty<AirplanePlacement>();
    }

    public string WaitInputIpAddress()
    {
        return "";
    }
}