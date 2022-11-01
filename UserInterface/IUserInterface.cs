using Common;
using GameLogic;

namespace UserInterface;

public interface IUserInterface
{
    void DrawAdditionalContent(string message);
    void DrawGameLogic(GameLogic.GameLogic game);
    Coordinate WaitLocalUserChooseBombLocation();
    AirplanePlacement[] WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game);
    string WaitUserEnterAnIpAddress(string recommended);
}