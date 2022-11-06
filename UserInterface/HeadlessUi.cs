using Common;
using Easy.Logger.Interfaces;
using GameLogic;

namespace UserInterface;

public class HeadlessUi : IUserInterface
{
    private readonly Stack<Coordinate> _bombLocations;
    private readonly string _ipAddress;
    private readonly IEasyLogger _note;

    public HeadlessUi(string ipAddress, IEnumerable<Coordinate> bombLocations)
    {
        _ipAddress = ipAddress;
        _bombLocations = new Stack<Coordinate>(bombLocations);
        _note = Logging.GetLogger("GameMainLoop");
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
        AirplanePlacement[] ap =
        {
            new()
            {
                Direction = "d",
                HeadCoord = new Coordinate(9, 9)
            },
            new()
            {
                Direction = "l",
                HeadCoord = new Coordinate(0, 0)
            },
            new()
            {
                Direction = "u",
                HeadCoord = new Coordinate(3, 3)
            }
        };
        foreach (var p in ap)
            game.SetAirplane(p.HeadCoord.X, p.HeadCoord.Y, p.Direction);
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