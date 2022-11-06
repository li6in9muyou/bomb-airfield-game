using Common;

namespace BombAnAirplaneTests;

public class GameLogicTests
{
    [Fact]
    public void ShouldTerminateGameAfterLoggingThreeDestroyBombResult()
    {
        // 登记了三次对方机场的摧毁之后就应该结束游戏
        var game = new GameLogic.GameLogic();
        game.LogBombResultOnOpponentAirfield(
            new Coordinate(0, 0),
            BombResult.Destroyed
        );
        game.LogBombResultOnOpponentAirfield(
            new Coordinate(9, 9),
            BombResult.Destroyed
        );
        game.LogBombResultOnOpponentAirfield(
            new Coordinate(9, 2),
            BombResult.Destroyed
        );
        Assert.True(game.MyAirfieldIsDoomed());
    }

    [Fact]
    public void ShouldTerminateGameAfterThreeAirplaneWereHitAtHead()
    {
        // 本方三个飞机的飞机头被炸之后就应该结束游戏
        var firstHead = new Coordinate(2, 2);
        var secondHead = new Coordinate(6, 3);
        var thirdHead = new Coordinate(8, 7);

        var game = new GameLogic.GameLogic();
        game.SetAirplane(firstHead.X, firstHead.Y, "u");
        game.SetAirplane(secondHead.X, secondHead.Y, "d");
        game.SetAirplane(thirdHead.X, thirdHead.Y, "r");
        Assert.Equal(
            BombResult.Destroyed,
            game.GetBombResultOnMyAirfield(firstHead)
        );
        Assert.Equal(
            BombResult.Destroyed,
            game.GetBombResultOnMyAirfield(secondHead)
        );
        Assert.Equal(
            BombResult.Destroyed,
            game.GetBombResultOnMyAirfield(thirdHead)
        );
        Assert.True(game.MyAirfieldIsDoomed());
    }

    [Fact]
    public void ShouldReturnCorrectBombResult()
    {
        // 正确地返回本方机场挨炸的结果
        var firstHead = new Coordinate(2, 2);
        var secondHead = new Coordinate(6, 3);
        var thirdHead = new Coordinate(8, 7);

        var game = new GameLogic.GameLogic();
        game.SetAirplane(firstHead.X, firstHead.Y, "u");
        game.SetAirplane(secondHead.X, secondHead.Y, "d");
        game.SetAirplane(thirdHead.X, thirdHead.Y, "r");

        Tuple<Coordinate, BombResult>[] coordinateAndExpectedResult =
        {
            new(new Coordinate(0, 3), BombResult.Hit),
            new(new Coordinate(1, 3), BombResult.Hit),
            new(new Coordinate(2, 3), BombResult.Hit),
            new(new Coordinate(3, 3), BombResult.Hit),
            new(new Coordinate(4, 3), BombResult.Hit),
            new(new Coordinate(5, 3), BombResult.Miss),
            new(new Coordinate(5, 4), BombResult.Miss),
            new(new Coordinate(5, 5), BombResult.Miss),
            new(new Coordinate(5, 6), BombResult.Hit),
            new(new Coordinate(5, 7), BombResult.Hit),
            new(new Coordinate(5, 8), BombResult.Hit),
            new(new Coordinate(5, 9), BombResult.Hit),
            new(firstHead, BombResult.Destroyed),
            new(secondHead, BombResult.Destroyed),
            new(thirdHead, BombResult.Destroyed)
        };
        foreach (var cb in coordinateAndExpectedResult)
        {
            var (coord, expected) = cb;
            Assert.Equal(
                expected,
                game.GetBombResultOnMyAirfield(coord)
            );
        }
    }
}