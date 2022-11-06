using Common;
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
        throw new NotImplementedException();
    }

    public void DrawLocalUserWon()
    {
        throw new NotImplementedException();
    }

    public bool WaitLocalUserDecideWhetherToContinue()
    {
        throw new NotImplementedException();
    }

    public void WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game)
    {
        //未设置GameLogic状态
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
        //isAirplanePlacementReasonable
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
        var BL = new { x = 1, y = 1, result = 1 };
        String JsonData = JsonConvert.SerializeObject(BL);
        UIServer.SendMsg("bombResult",JsonData);
    }

    public Coordinate WaitLocalUserChooseBombLocation(GameLogic.GameLogic game)
    {
        //未设置GameLogic状态
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