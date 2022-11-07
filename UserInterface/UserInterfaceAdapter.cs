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
    public List<Ap> Aps { get; set; }
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
        {//未完，没有返回值，没有通知前端
            //获取数据
            var jsonAp = UiCache.WaitAirplanesPlacement();
            var apRoot = JsonConvert.DeserializeObject<ApRoot>(jsonAp);
            var aps = new AirplanePlacement[apRoot.Aps.Count];
            var i = 0;
            foreach (var ap in apRoot.Aps)
            {
                var airPlanePlacement = new AirplanePlacement();
                airPlanePlacement.Direction = ap.Direction;
                var coordinate = new Coordinate(ap.X, ap.Y);
                airPlanePlacement.HeadCoord = coordinate;
                aps[i++] = airPlanePlacement;
            }
            //判断数据是否合理
            bool isPPR=game.IsPlanesPlacementReasonable(aps);
            if (isPPR)
            {
                foreach (var ap in apRoot.Aps)
                {
                    game.SetAirplane(ap.X,ap.Y,ap.Direction);
                }
                break;
            }else continue;
        }
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        var ipAddress = UiCache.WaitIpAddress();
        return ipAddress;
    }

    public void DrawAdditionalContent(string message)
    {
        UIServer.SendMsg("stateMsg",message);
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
        //发送轰炸位置、结果
        GameStateSnapShot gSSS = game.CaptureCurrentGameState();
        var br = gSSS.BombResultsOnOpponentAirfield;
        String JsonData = "[";
        foreach (var b in br)
        {
            var BL = new { x = b.Item1.X, y = b.Item1.Y, result = b.Item2.ToString() };
            JsonData += JsonConvert.SerializeObject(BL) + ",";
        }
        JsonData += "]";
        UIServer.SendMsg("bombResult",JsonData);
    }

    public Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game)
    {
        var jsonCoord = UiCache.WaitBombLocation();
        var coordRoot = JsonConvert.DeserializeObject<CoordRoot>(jsonCoord);
        var coordinate = new Coordinate(coordRoot.X, coordRoot.Y);
        return coordinate;
    }

    public AirplanePlacement[] WaitLocalUserPlaceAirplanes()
    {
        return Array.Empty<AirplanePlacement>();
    }
}