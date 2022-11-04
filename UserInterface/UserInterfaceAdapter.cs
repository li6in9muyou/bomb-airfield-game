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
        return aps;
    }

    public string WaitUserEnterAnIpAddress(string recommended)
    {
        String ipAddress = UiCache.WaitIpAddress();
        return ipAddress;
        throw new NotImplementedException();
    }

    public void DrawAdditionalContent(string message)
    {
    }

    public void DrawGameLogic(GameLogic.GameLogic game)
    {
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