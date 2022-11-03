using Common;
using GameLogic;

namespace UserInterface;

public class ConsoleUi : IUserInterface
{
    public void DrawAdditionalContent(string message)
    {
        Console.Out.WriteLine("message = {0}", message);
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
        Console.Out.WriteLine("game = {0}", game);
    }

    public Coordinate WaitLocalUserChooseBombLocation()
    {
        Console.Out.WriteLine(@"bomb where? \d,\d");
        var where = Console.ReadLine();
        var coordinates = where!.Split(',');
        var x = coordinates[0];
        var y = coordinates[1];
        var xN = int.Parse(x);
        var yN = int.Parse(y);
        Console.Out.WriteLine("x = {0}", xN);
        Console.Out.WriteLine("y = {0}", yN);
        return new Coordinate(xN, yN);
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
        Console.Out.WriteLine(@"to join room, enter an ip addr");
        return Console.ReadLine()!;
    }
}