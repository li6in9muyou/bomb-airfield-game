using Common;
using GameLogic;

namespace UserInterface;

public class ReplayUi : IUserInterface
{
    private readonly Stack<Coordinate> _bombLocations;
    private readonly string _ipAddress;

    public ReplayUi(string ipAddress, IEnumerable<Coordinate> bombLocations)
    {
        _ipAddress = ipAddress;
        _bombLocations = new Stack<Coordinate>(bombLocations);
    }

    public void DrawAdditionalContent(string message)
    {
        Console.Out.WriteLine("message = {0}", message);
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
        Console.Out.WriteLine("game = {0}", game);
    }

    public Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game)
    {
        Console.Out.WriteLine(@"bomb where? \d,\d");
        Console.Out.WriteLine("mocking input");
        var coordinate = _bombLocations.Pop();
        Console.Out.WriteLine("coordinate = {0}", coordinate);
        return coordinate;
    }

    public AirplanePlacement[] WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game)
    {
        return new[]
        {
            new AirplanePlacement
            {
                Direction = "d",
                HeadCoord = new Coordinate(9, 9)
            },
            new AirplanePlacement
            {
                Direction = "l",
                HeadCoord = new Coordinate(0, 0)
            },
            new AirplanePlacement
            {
                Direction = "u",
                HeadCoord = new Coordinate(3, 3)
            }
        };
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        Console.Out.WriteLine("mocking input");
        Console.Out.WriteLine("_ipAddress = {0}", _ipAddress);
        return _ipAddress;
    }
}