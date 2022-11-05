using Common;
using Online;
using UserInterface;

namespace BombAnAirplane;

internal static class Program
{
    private static void Main()
    {
        // init ui
        IUserInterface ui = new ReplayUi("", new[]
        {
            new Coordinate(0, 0),
            new Coordinate(1, 1),
            new Coordinate(2, 6),
            new Coordinate(3, 7)
        });
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

        ui.WaitLocalUserPlaceAirplanes(game);

        online.WaitOpponentPlaceAirplane();

        var isMyTurnToBomb = isIBombFirst;
        while (!game.ShouldTerminate())
        {
            if (!isMyTurnToBomb)
            {
                var coordinate = ui.WaitLocalUserChooseBombLocation(game);
                var result = online.BombOpponentAirfieldAndWaitResult(coordinate);
                game.LogBombResultOnOpponentAirfield(coordinate, result);
            }
            else
            {
                ui.DrawAdditionalContent("需等待对方选定炸的位置");
                var coordinate = online.WaitOpponentToBombMyAirfield();
                var result = game.GetBombResultOnMyAirfield(coordinate);
                online.SendBombResultOfMyAirfield(result);
            }

            ui.DrawGameLogic(game);
        }
    }
}