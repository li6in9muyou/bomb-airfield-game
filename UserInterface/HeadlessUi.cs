using Common;
using Easy.Logger.Interfaces;
using GameLogic;

namespace UserInterface;

public class HeadlessUi : IUserInterface
{
    private readonly IEnumerable<AirplanePlacement> _airplanePlacements;
    private readonly Stack<Coordinate> _bombLocations;
    private readonly string _ipAddress;
    private readonly IEasyLogger _note;

    public HeadlessUi(
        string ipAddress,
        IEnumerable<Coordinate> bombLocations,
        IEnumerable<AirplanePlacement> airplanePlacements
    )
    {
        _ipAddress = ipAddress;
        _airplanePlacements = airplanePlacements;
        _bombLocations = new Stack<Coordinate>(bombLocations.Reverse());
        _note = Logging.GetLogger("MOCK--HeadlessUi");
    }

    public bool WaitLocalUserDecideWhetherToContinue()
    {
        return false;
    }

    public void DrawLocalUserLost()
    {
        _note.Info("local user has lost");
    }

    public void DrawLocalUserWon()
    {
        _note.Info("local user has won");
    }

    public void DrawAdditionalContent(string message)
    {
        _note.Info($"show {message} to local user");
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
        _note.Info("current game state is rendered");
    }

    public Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game)
    {
        var coordinate = _bombLocations.Pop();
        _note.Info($"local user bombs {coordinate.X},{coordinate.Y}");
        return coordinate;
    }

    public void WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game)
    {
        foreach (var p in _airplanePlacements)
        {
            var good = game.SetAirplane(p.HeadCoord.X, p.HeadCoord.Y, p.Direction);
            if (!good)
                _note.Error($"can not set airplane at {p.HeadCoord.X},{p.HeadCoord.Y} heading {p.Direction}");
        }
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        _note.Info($"local enters an ip addr of {_ipAddress}");
        return _ipAddress;
    }

    public bool WaitLocalUserDecideWhetherContinue()
    {
        _note.Info("local refuse to continue");
        return false;
    }
}