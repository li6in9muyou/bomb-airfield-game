using Common;
using GameLogic;
using Newtonsoft.Json;
using System.Threading;

namespace UserInterface;
public class Ap
{
    public string direction { get; set; }
    public int x { get; set; }
    public int y { get; set; }
}
public class ApRoot
{
    public List <Ap > Aps { get; set; }
}
public class CoorRoot
{
    public int x { get; set; }
    public int y { get; set; }
}
public class UserInterfaceAdapter : IUserInterface
{
    public void WaitLocalUserPlaceAirplanes(GameLogic.GameLogic game)
    {
        //未设置GameLogic状态
        String JsonAP = UiCache.WaitAirplanesPlacement();
        ApRoot apRoot = JsonConvert.DeserializeObject<ApRoot>(JsonAP);
        AirplanePlacement[] aps = new AirplanePlacement[apRoot.Aps.Count];
        int i = 0;
        foreach (var ap in apRoot.Aps)
        {
            var airPlanePlacement = new AirplanePlacement();
            airPlanePlacement.Direction = ap.direction;
            var coordinate = new Coordinate(ap.x, ap.y);
            airPlanePlacement.HeadCoord = coordinate;
            aps[i++] = airPlanePlacement;
        }
        //isAirplanePlacementReasonable
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        String ipAddress = UiCache.WaitIpAddress();
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
        String JsonCoor = UiCache.WaitBombLocation();
        CoorRoot coorRoot = JsonConvert.DeserializeObject<CoorRoot>(JsonCoor);
        Coordinate coordinate = new Coordinate(coorRoot.x, coorRoot.y);
        return coordinate;
    }

    public AirplanePlacement[] WaitLocalUserPlaceAirplanes()
    {
        return Array.Empty<AirplanePlacement>();
    }
}