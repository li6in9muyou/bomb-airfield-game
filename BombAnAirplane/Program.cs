using Common;
using Online;
using UserInterface;

namespace BombAnAirplane;

internal static class Program
{
    private static void Main()
    {
        var note = Logging.GetLogger("GameMainLoop");
        note.Debug("game has started");
        // 各子系统初始化
        UIServer.Init();
        IUserInterface ui = new UserInterfaceAdapter();
        ICommunicator communicator = new MockReceiving(new[]
        {
            "999",
            "ok",
            "3,3",
            "continue",
            "destroyed",
            "continue",
            "0,3",
            "continue",
            "destroyed",
            "continue",
            "5,5",
            "continue",
            "destroy",
            "yield",
            "end"
        });
        var online = new Online.Online(communicator);

        // 询问玩家如何联机
        var ipAddress = ui.WaitUserEnterAnIpAddress("localhost");

        bool isIBombFirst;
        if (ipAddress == "")
        {
            // 我是房主
            note.Debug("local creates a room");
            var remoteHandle = online.CreateRoom();
            ui.DrawAdditionalContent("本房间的IP地址是：" + remoteHandle);
            isIBombFirst = online.WaitOpponentToJoin();
        }
        else
        {
            // 我要加入别人的房间
            note.Debug("local joins a room");
            isIBombFirst = online.WaitJoinOpponentRoom(ipAddress);
        }

        // 我是不是先手
        note.Debug($"hand shake result: isIBombFirst = {isIBombFirst}");

        // 游戏主循环
        do
        {
            var game = new GameLogic.GameLogic();
            ui.WaitLocalUserPlaceAirplanes(game);
            note.Debug("local finishes placing airplanes");
            online.WaitOpponentPlaceAirplane();
            note.Debug("remote finishes placing airplanes");
            var isMyTurnToBomb = isIBombFirst;
            while (true)
            {
                if (isMyTurnToBomb)
                {
                    note.Debug("local is going to bomb");
                    ui.DrawAdditionalContent("请输入你要炸的位置");
                    var coordinate = ui.WaitLocalUserChooseBombLocation(game);
                    var result = online.BombOpponentAirfieldAndWaitResult(coordinate);
                    game.LogBombResultOnOpponentAirfield(coordinate, result);
                }
                else
                {
                    note.Debug("remote is going to bomb");
                    ui.DrawAdditionalContent("需等待对方选定炸的位置");
                    var coordinate = online.WaitOpponentToBombMyAirfield();
                    var result = game.GetBombResultOnMyAirfield(coordinate);
                    online.SendBombResultOfMyAirfield(result);
                }

                ui.DrawGameLogic(game);
                isMyTurnToBomb = !isMyTurnToBomb;

                if (game.MyAirfieldIsDoomed())
                {
                    note.Debug("local's airfield is doomed");
                    online.WaitSurrenderToOpponent();
                    ui.DrawLocalUserLost();

                    note.Debug("this game is over");
                    break;
                }

                note.Debug("local not yet yields");
                online.WaitNotifyStillStanding();
                if (!online.WaitOpponentToSurrender())
                {
                    note.Debug("remote not yet yields");
                    continue;
                }

                note.Debug("local has won");
                ui.DrawLocalUserWon();
                note.Debug("this game is over");
                break;
            }
        } while (ui.WaitLocalUserDecideWhetherToContinue());
    }
}