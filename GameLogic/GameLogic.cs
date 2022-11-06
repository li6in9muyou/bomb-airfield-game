using Common;
using Database;

namespace GameLogic;

public class GameLogic
{
    public GameStateSnapShot CaptureCurrentGameState()
    {
        List<Tuple<Coordinate, BombResult>> BombResultsOnOpponentAirfield = new List<Tuple<Coordinate, BombResult>>();

        List<Tuple<Coordinate, BombResult>> MyAirfieldWasBombedAt = new List<Tuple<Coordinate, BombResult>>();
        List<Airplane> MyAirplanes = new List<Airplane>();

        //获取敌方机场，实例化一个单例
        OpponentAirfield opponentAirfield = OpponentAirfield.getOpponentAirfield();
        //从单例中获取数据结构,这个数据结构中存有敌方机场的状态
        Dictionary<int, BombResult> BombResults = opponentAirfield.getOpponent();
        foreach (int Key in BombResults.Keys)
        {
            Coordinate C = new Coordinate(Key / 10, Key % 10);
            BombResult B = BombResults[Key];
            Tuple<Coordinate, BombResult> tuple = new(C, B);
            BombResultsOnOpponentAirfield.Add(tuple);
        }
        //获取本方飞机，实例化AirplanePlace单例
        AirplanePlace airplanePlace = AirplanePlace.GetAirplanePlace();
        for (int i = 0; i < 3; i++)
        {
            if (airplanePlace.GetAirplane(i)!=null)
            MyAirplanes.Add(airplanePlace.GetAirplane(i));
        }

        //获取本方机场信息：坐标，状态 实例化BeBombed单例
        BeBombed beBombed = BeBombed.GetBeBombed();
        //从单例中获取存有本方机场状态的数据结构
        Dictionary<int, BombResult> Beb = beBombed.GetBeb();
        foreach (int Key in Beb.Keys)
        {
            Coordinate C=new Coordinate(Key / 10, Key % 10);
            BombResult B = Beb[Key];
            Tuple<Coordinate, BombResult> tuple = new(C, B);
            MyAirfieldWasBombedAt.Add(tuple);
        }

        GameStateSnapShot gameState = new GameStateSnapShot(BombResultsOnOpponentAirfield.ToArray(),MyAirfieldWasBombedAt.ToArray(),MyAirplanes.ToArray());
        return gameState;
    }

    public bool IsPlanesPlacementReasonable(AirplanePlacement[] aps)
    {
        if (aps.Length != 3) return false;
        //表示相对位置的数组
        int[][] refer = new int[4][];
        refer[0] = new int[] {0,-19,-9,1,11,21,2,-7,3,13};//上
        refer[1] = new int[] {0,-21,-11,-1,9,19,-2,-13,-3,7};//下
        refer[2] = new int[] {0,8,9,10,11,12,20,29,30,31 };//左
        refer[3] = new int[] {0,-12,-11,-10,-9,-8,-20,-31,-30,-29};//右
        //表示位置是否被占用
        bool[] isoccupied = new bool[100];
        for (int i = 0; i < 100; i++) isoccupied[i] = false;
        //遍历三个飞机
        for (int i = 0; i < 3; i++)
        {
            Coordinate c = aps[i].HeadCoord;
            String d=aps[i].Direction;//u d l r
            int id = c.X * 10 + c.Y;
            int[]? r=null;
            if (d.Equals("u")) r = refer[0];
            if (d.Equals("d")) r = refer[1];
            if (d.Equals("l")) r = refer[2];
            if (d.Equals("r")) r = refer[3];
            //遍历这个飞机的所有点
            if(r!=null)
            for (int j = 0; j < 10; j++)
            {
                //当前点的id
                int buf_id = id;
                buf_id += r[j];
                //出界错误
                if (buf_id < 0 || buf_id > 99)
                {
                    Console.WriteLine(i+","+j+"出界错误:"+buf_id/10+","+buf_id%10);
                    return false;
                }
                //重叠错误
                if (isoccupied[buf_id])
                {
                    Console.WriteLine(i+","+j+"重叠错误:"+buf_id/10+","+buf_id%10);
                    return false;
                }
                else//无错误，占点
                {
                    isoccupied[buf_id] = true;
                }
            }
        }
        return true;
    }
    public bool SetAirplane(int x, int y, string direction) //摆飞机
    {
        //传入飞机头，方向，经过算法(排除机头坐标重复，飞机重叠，坐标超载等情况)，返回bool值，true成功--》传入数据类中，false失败--》需要再次输入(逻辑在界面类实现)
        var flag = true;
        var airplanePlace = AirplanePlace.GetAirplanePlace(); //获取存有
        //飞机坐标的类，机场类
        if (x < 0 || x > 9 || y < 0 || y > 9) return false; //任何坐标越界直接返回


        switch (direction) //1、坐标超载
        {
            //x→增，y↓增
            //上，下，左，右
            case "u":
                if (y + 3 > 9 || x + 2 > 9 || x - 2 < 0) flag = false;
                break;
            case "d":
                if (y - 3 < 0 || x + 2 > 9 || x - 2 < 0) flag = false;
                break;
            case "l":
                if (x + 3 > 9 || y + 1 > 9 || y - 1 < 0) flag = false;
                break;
            case "r":
                if (x - 3 < 0 || y - 1 < 0 || y + 1 > 9) flag = false;
                break;
        }

        if (!flag) return false; //如果flag满足上述任何一个越界条件，flag=false，则返回
        //如不满足，则检测飞机重叠情况，由于采用了dictionary数据结构，只需要拿飞机坐标调用其中的方法JudgeCoord()检测有没有在head||body中，ture表示重叠
        var f = airplanePlace.JudgeCoord(x, y);
        switch (direction)
        {
            //上，下，左，右
            case "u":
                f = f || airplanePlace.JudgeCoord(x, y + 1);
                f = f || airplanePlace.JudgeCoord(x - 1, y + 1);
                f = f || airplanePlace.JudgeCoord(x - 2, y + 1);
                f = f || airplanePlace.JudgeCoord(x + 1, y + 1);
                f = f || airplanePlace.JudgeCoord(x + 2, y + 1);

                f = f || airplanePlace.JudgeCoord(x, y + 2);

                f = f || airplanePlace.JudgeCoord(x, y + 3);
                f = f || airplanePlace.JudgeCoord(x - 1, y + 3);
                f = f || airplanePlace.JudgeCoord(x + 1, y + 3);
                break;
            case "d":
                f = f || airplanePlace.JudgeCoord(x, y - 1);
                f = f || airplanePlace.JudgeCoord(x - 1, y - 1);
                f = f || airplanePlace.JudgeCoord(x - 2, y - 1);
                f = f || airplanePlace.JudgeCoord(x + 1, y - 1);
                f = f || airplanePlace.JudgeCoord(x + 2, y - 1);

                f = f || airplanePlace.JudgeCoord(x, y - 2);

                f = f || airplanePlace.JudgeCoord(x, y - 3);
                f = f || airplanePlace.JudgeCoord(x - 1, y - 3);
                f = f || airplanePlace.JudgeCoord(x + 1, y - 3);
                break;
            case "l":
                f = f || airplanePlace.JudgeCoord(x + 1, y);
                f = f || airplanePlace.JudgeCoord(x + 1, y - 1);
                f = f || airplanePlace.JudgeCoord(x + 1, y - 2);
                f = f || airplanePlace.JudgeCoord(x + 1, y + 1);
                f = f || airplanePlace.JudgeCoord(x + 1, y + 2);

                f = f || airplanePlace.JudgeCoord(x + 2, y);

                f = f || airplanePlace.JudgeCoord(x + 3, y);
                f = f || airplanePlace.JudgeCoord(x + 3, y - 1);
                f = f || airplanePlace.JudgeCoord(x + 3, y + 1);
                break;
            case "r":
                f = f || airplanePlace.JudgeCoord(x - 1, y);
                f = f || airplanePlace.JudgeCoord(x - 1, y - 1);
                f = f || airplanePlace.JudgeCoord(x - 1, y - 2);
                f = f || airplanePlace.JudgeCoord(x - 1, y + 1);
                f = f || airplanePlace.JudgeCoord(x - 1, y + 2);

                f = f || airplanePlace.JudgeCoord(x - 2, y);

                f = f || airplanePlace.JudgeCoord(x - 3, y);
                f = f || airplanePlace.JudgeCoord(x - 3, y - 1);
                f = f || airplanePlace.JudgeCoord(x - 3, y + 1);
                break;
        }

        if (f) return false; //如果有任何一个重叠，f=ture，则说明重叠，返回false

        //经过多轮判断，说明已经通过了，则将其丢入
        flag = airplanePlace.AddAirplane(x, y, direction);
        return flag;
    }

    public void LogBombResultOnOpponentAirfield(Coordinate coordinate, BombResult result) //炸敌方结果存储在OpponentAirfield表中
    {
        var x = coordinate.X;
        var y = coordinate.Y;
        var opponentAirfield = OpponentAirfield.getOpponentAirfield();
        opponentAirfield.AddCoordinate(x, y, result);
    }

    public bool MyAirfieldIsDoomed() //判断游戏是否结束
    {
        //即判断三个飞机头坐标是否在BeBombed表中
        var airplanePlace = AirplanePlace.GetAirplanePlace();
        var beBombed = BeBombed.GetBeBombed();
        var x = airplanePlace.GetX();
        var y = airplanePlace.GetY();
        var flag = new bool[x.Length];
        for (var i = 0; i < x.Length; i++) flag[i] = beBombed.Judge(x[i], y[i]);

        return flag[0] && flag[1] && flag[2];
    }

    public BombResult GetBombResultOnMyAirfield(Coordinate coordinate) //被炸判断
    {
        var x = coordinate.X;
        var y = coordinate.Y;
        //对当前坐标轰炸，获取本方机场AirplanePlace类,以便确定,获取被炸表，以便添加
        var airplanePlace = AirplanePlace.GetAirplanePlace();
        var beBombed = BeBombed.GetBeBombed();
        var flag = airplanePlace.JudgeHead(x, y);

        if (flag)
        {
            beBombed.AddBeBombed(x, y, BombResult.Destroyed);
            return BombResult.Destroyed;
        }

        var flag2 = airplanePlace.JudgeBody(x, y);
        if (flag2)
        {
            beBombed.AddBeBombed(x, y, BombResult.Hit);
            return BombResult.Hit;
        }

        beBombed.AddBeBombed(x, y, BombResult.Miss);
        return BombResult.Miss;
    }
}