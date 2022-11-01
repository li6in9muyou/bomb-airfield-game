using System.Text.RegularExpressions;
using Common;

namespace Online;

public class BombAirfieldProtocol
{
    public static bool DoHandShake(ICommunicator com)
    {
        var myNumber = new Random().Next(0, 100);
        com.Write(myNumber.ToString());
        var received = com.Expect(new Regex(@"\d{1,2}"));
        var opponentNumber = int.Parse(received);
        return myNumber > opponentNumber;
    }

    public static void WaitOpponentPlaceAirPlane(ICommunicator com)
    {
        com.Expect(new Regex("ok"));
    }

    public static void NotifyLocalReady(ICommunicator com)
    {
        com.Write("ok");
    }

    public static BombResult BombOpponentAirfieldAndWaitResult(ICommunicator com, Coordinate coordinate)
    {
        var where = coordinate.X + "," + coordinate.Y;
        com.Write(where);
        var result = com.Expect(new Regex(@"(hit)|(miss)|(destroy)"));
        switch (result)
        {
            case "hit":
            {
                return BombResult.Hit;
            }
            case "miss":
            {
                return BombResult.Miss;
            }
            case "destroy":
            {
                return BombResult.Destroyed;
            }
            default:
            {
                Console.Out.WriteLine("result = {0}", result);
                throw new Exception();
            }
        }
    }

    public static Coordinate WaitOpponentToBombMyAirfield(ICommunicator com)
    {
        var where = com.Expect(new Regex(@"(\d,\d)|(-1,-1)"));
        Console.Out.WriteLine("where = {0}", where);
        var coordinates = where.Split(',');
        var x = coordinates[0];
        var y = coordinates[1];
        var xN = int.Parse(x);
        var yN = int.Parse(y);
        Console.Out.WriteLine("x = {0}", xN);
        Console.Out.WriteLine("y = {0}", yN);
        return new Coordinate(xN, yN);
    }

    public static void SendBombResultOfMyAirfield(ICommunicator com, BombResult result)
    {
        com.Write(result.ToString().ToLower());
    }
}