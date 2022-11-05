using Common;

namespace UserInterface;

public interface IUserInterface
{
    void DrawAdditionalContent(string message);
    void DrawGameLogic(GameLogic.GameLogic game);
    Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game);
    void WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game);
    string WaitUserEnterAnIpAddress(string recommended);
}