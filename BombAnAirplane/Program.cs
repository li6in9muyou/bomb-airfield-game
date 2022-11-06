﻿using Common;
using Online;
using UserInterface;

namespace BombAnAirplane;

internal static class Program
{
    private static void Main()
    {
        var note = Logging.GetLogger("游戏主循环");
        note.Debug("你好世界");
        // 各子系统初始化
        IUserInterface ui = new ReplayUi("", new[]
        {
            new Coordinate(0, 0),
            new Coordinate(1, 1),
            new Coordinate(2, 6),
            new Coordinate(3, 7)
        });
        ICommunicator communicator = new MockCommunicator();
        var online = new Online.Online(communicator);

        // 询问玩家如何联机
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
            // 我要加入别人的房间
            isIBombFirst = online.WaitJoinOpponentRoom(ipAddress);
        }

        // 我是不是先手
        Console.Out.WriteLine("isIBombFirst = {0}", isIBombFirst);

        // 游戏主循环
        do
        {
            var game = new GameLogic.GameLogic();
            ui.WaitLocalUserPlaceAirplanes(game);
            online.WaitOpponentPlaceAirplane();
            var isMyTurnToBomb = isIBombFirst;
            while (true)
            {
                if (isMyTurnToBomb)
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
                isMyTurnToBomb = !isMyTurnToBomb;

                if (game.MyAirfieldIsDoomed())
                {
                    online.WaitSurrenderToOpponent();
                    ui.DrawLocalUserLost();
                    break;
                }

                online.WaitNotifyStillStanding();
                if (!online.WaitOpponentToSurrender()) continue;
                ui.DrawLocalUserWon();
                break;
            }
        } while (ui.WaitLocalUserDecideWhetherToContinue());
    }
}