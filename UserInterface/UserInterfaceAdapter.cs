using Common;
using Database;
using GameLogic;
using Newtonsoft.Json;

namespace UserInterface;

public class Ap
{
    public string Direction { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class ApRoot
{
    public List<Ap> Aps { get; set; } = new();
}

public class CoordRoot
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class UserInterfaceAdapter : IUserInterface
{
    public void DrawLocalUserLost()
    {
        DrawAdditionalContent("you lose!");
    }

    public void DrawLocalUserWon()
    {
        DrawAdditionalContent("you win!");
    }

    public bool WaitLocalUserDecideWhetherToContinue()
    {
        throw new NotImplementedException();
    }

    public void WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game)
    {
        while (true)
        {
            //未完，没有返回值，没有通知前端
            //获取数据
            var jsonAp = UiCache.WaitAirplanesPlacement();
            var apRoot = JsonConvert.DeserializeObject<ApRoot>(jsonAp);
            var aps = new AirplanePlacement[apRoot!.Aps.Count];
            var i = 0;
            foreach (var ap in apRoot.Aps)
            {
                var airPlanePlacement = new AirplanePlacement
                {
                    Direction = ap.Direction,
                    HeadCoord = new Coordinate(ap.X, ap.Y)
                };
                aps[i++] = airPlanePlacement;
            }

            //判断数据是否合理
            bool isPPR = game.IsPlanesPlacementReasonable(aps);
            if (isPPR)
            {
                foreach (var ap in apRoot.Aps)
                {
                    game.SetAirplane(ap.X, ap.Y, ap.Direction);
                }

                break;
            }
        }
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        var ipAddress = UiCache.WaitIpAddress();
        return ipAddress;
    }

    public void DrawAdditionalContent(string message)
    {
        UIServer.SendMsg("stateMsg", message);
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
        //发送轰炸位置、结果
        GameStateSnapShot gSSS = game.CaptureCurrentGameState();
        var br = gSSS.MyAirfieldWasBombedAt;
        var mbr = gSSS.BombResultsOnOpponentAirfield;
        String JsonData = "[";
        foreach (var b in br)
        {
            var BL = new { x = b.Item1.X, y = b.Item1.Y, result = b.Item2.ToString() };
            JsonData += JsonConvert.SerializeObject(BL) + ",";
        }

        foreach (var mb in mbr)
        {
            var BL = new { x = mb.Item1.X+10, y = mb.Item1.Y , result = mb.Item2.ToString() };
            JsonData += JsonConvert.SerializeObject(BL) + ",";
        }
        Console.WriteLine("x:"+JsonData);
        JsonData=JsonData.Substring(0, JsonData.Length - 1);
        JsonData += "]";
        UIServer.SendMsg("bombResult", JsonData);
    }

    public Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game)
    {
        var jsonCoord = UiCache.WaitBombLocation();
        var coordRoot = JsonConvert.DeserializeObject<CoordRoot>(jsonCoord);
        var coordinate = new Coordinate(coordRoot!.X, coordRoot.Y);
        return coordinate;
    }
}