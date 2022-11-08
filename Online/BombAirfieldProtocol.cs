using System.Text.RegularExpressions;
using Common;
using Easy.Logger.Interfaces;

namespace Online;

public class BombAnAirplaneProtocol
{
    private static readonly IEasyLogger Note = Logging.GetLogger("BombAnAirplaneProtocol");

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
        Note.Info($"bomb on opponent airfield at {coordinate.X},{coordinate.Y} is a {result}");
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
                Note.Error($"received unknown bomb result {result}");
                throw new Exception();
            }
        }
    }

    public static Coordinate WaitOpponentToBombMyAirfield(ICommunicator com)
    {
        var where = com.Expect(new Regex(@"(\d,\d)|(-1,-1)"));
        var coordinates = where.Split(',');
        var x = coordinates[0];
        var y = coordinates[1];
        var xN = int.Parse(x);
        var yN = int.Parse(y);
        Note.Info($"opponent bombs me at {xN},{yN}");
        return new Coordinate(xN, yN);
    }

    public static void SendBombResultOfMyAirfield(ICommunicator com, BombResult result)
    {
        com.Write(result.ToString().ToLower());
    }

    public static bool WaitOpponentToSurrender(ICommunicator com)
    {
        var r = com.Expect(new Regex("(yield)|(continue)"));
        if (r == "continue") return false;
        com.Write("end");
        return true;
    }

    public static void WaitNotifyStillStanding(ICommunicator com)
    {
        com.Write("continue");
    }

    public static void WaitSurrenderToOpponent(ICommunicator com)
    {
        com.Write("yield");
        com.Write("end");
    }
}