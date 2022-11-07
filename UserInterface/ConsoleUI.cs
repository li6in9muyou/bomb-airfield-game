using Common;
using Easy.Logger.Interfaces;

namespace UserInterface;

public class ConsoleUi : IUserInterface
{
    private readonly IEasyLogger _note;

    public ConsoleUi()
    {
        _note = Logging.GetLogger("ConsoleUi");
    }

    public void DrawLocalUserLost()
    {
        Console.Out.WriteLine("local user has lost");
    }

    public void DrawLocalUserWon()
    {
        Console.Out.WriteLine("local user has won");
    }

    public bool WaitLocalUserDecideWhetherToContinue()
    {
        Console.Out.WriteLine(@"keep playing? y|n");
        return Console.ReadLine() == "y";
    }

    public void DrawAdditionalContent(string message)
    {
        Console.Out.WriteLine("\nADDITIONAL CONTENT");
        Console.Out.WriteLine(message);
        Console.Out.WriteLine("END");
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
        var state = game.CaptureCurrentGameState();
        Console.Out.WriteLine("\n\nCURRENT GAME STATE");

        Console.Out.WriteLine("your airplanes:");
        foreach (var airplane in state.MyAirplanes)
            Console.Out.WriteLine($"{airplane.Direction} ${airplane.X},${airplane.Y}");

        Console.Out.WriteLine("your airfield:");
        foreach (var (coordinate, bombResult) in state.MyAirfieldWasBombedAt)
            Console.Out.WriteLine($"{coordinate.X},{coordinate.Y} is {bombResult}");

        Console.Out.WriteLine("opponent airfield:");
        foreach (var (coordinate, bombResult) in state.BombResultsOnOpponentAirfield)
            Console.Out.WriteLine($"{coordinate.X},{coordinate.Y} is {bombResult}");

        Console.Out.WriteLine("END");
        Console.Out.WriteLine();
        Console.Out.WriteLine();
    }

    public Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game)
    {
        Console.Out.WriteLine(@"bomb where? \d,\d");
        var where = Console.ReadLine();
        var coordinates = where!.Split(',');
        var x = coordinates[0];
        var y = coordinates[1];
        var xN = int.Parse(x);
        var yN = int.Parse(y);
        _note.Info($"user input coord {xN},{yN}");
        return new Coordinate(xN, yN);
    }

    public void WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game)
    {
        var suggestions = new Stack<string>(new[] { "u,2,2", "d,6,3", "r,8,7" }.Reverse());
        for (var i = 0; i < 3; i++)
        {
            bool validPlacement;
            do
            {
                Console.Out.WriteLine(
                    @"enter airplane placement ^[udlr],\d,\d$"
                    + $" press enter to accept {suggestions.Peek()}"
                );
                var text = Console.ReadLine();
                if (text == "") text = suggestions.Pop();
                _note.Info($"user input: {text}");
                var a = text!.Split(',');
                validPlacement = game.SetAirplane(int.Parse(a[1]), int.Parse(a[2]), a[0]);

                if (!validPlacement)
                    Console.Out.WriteLine("invalid placement, try again...");
                else
                    Console.Out.WriteLine("good placement!!!");
            } while (!validPlacement);
        }
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        Console.Out.WriteLine(@"to join room, enter an ip addr");
        var ipAddr = Console.ReadLine();
        _note.Info($"user input \"{ipAddr}\"");
        return ipAddr!;
    }
}