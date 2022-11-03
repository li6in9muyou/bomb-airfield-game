using Common;
using GameLogic;

namespace UserInterface;

public class UserInterfaceAdapter : IUserInterface
{
    public AirplanePlacement[] WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game)
    {
        throw new NotImplementedException();
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        throw new NotImplementedException();
    }

    public void DrawAdditionalContent(string message)
    {
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
    }

    public Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game)
    {
        return Coordinate.Void();
    }

    public AirplanePlacement[] WaitLocalUserPlaceAirplanes()
    {
        return Array.Empty<AirplanePlacement>();
    }

    public string WaitInputIpAddress()
    {
        return "";
    }
}