using Online;
using UserInterface;

namespace BombAnAirplane;

internal static class Program
{
    private static void Main()
    {
        // init ui
        IUserInterface ui = new ConsoleUi();
        // init online
        ICommunicator communicator = new MockCommunicator();
        var online = new Online.Online(communicator);
        // init game
        var game = new GameLogic.GameLogic();

        // 获取一个IP
        var ipAddress = ui.WaitUserEnterAnIpAddress("localhost");

        bool isIBombFirst;
        if (ipAddress == "")
        {
            // 我是房主
            var remoteHandle = online.CreateRoom();
            ui.DrawAdditionalContent("本房间的IP地址是：" + remoteHandle);
            isIBombFirst = online.WaitOpponentToJoin();
        }
        else
        {
            // 我要加入别的房间
            isIBombFirst = online.WaitJoinOpponentRoom(ipAddress);
        }

        // 我是不是先手
        Console.Out.WriteLine("isIBombFirst = {0}", isIBombFirst);
        
        var placements =  ui.WaitLocalUserPlaceAirplanes(game);
        foreach (var placement in placements)
        {
            game.setAirplane(placement.HeadCoord.X, placement.HeadCoord.Y, placement.Direction);
        }
        online.WaitOpponentPlaceAirplane();

        if (isIBombFirst)
        {
            var coordinate =  ui.WaitLocalUserChooseBombLocation(game);
            var result =online.BombOpponentAirfieldAndWaitResult(coordinate);
            game.LogBombResultOnOpponentAirfield(coordinate, result);
        }
    }
}