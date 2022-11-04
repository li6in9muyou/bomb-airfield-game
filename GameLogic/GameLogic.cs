namespace GameLogic;

using Common;
using Database;
public class GameLogic
{

    public Boolean setAirplanePlace(Coordinate coordinate, String direction)//摆飞机
    {
        int x = coordinate.X; int y = coordinate.Y;
        //传入飞机头，方向，经过算法(排除机头坐标重复，飞机重叠，坐标超载等情况)，返回bool值，true成功--》传入数据类中，false失败--》需要再次输入(逻辑在界面类实现)
        bool flag = true;
        AirplanePlace airplanePlace = AirplanePlace.getAirplanePlace();//获取存有飞机坐标的类，机场类
        if (x < 0 || x > 9 || y < 0 || y > 9) return false;//任何坐标越界直接返回
                                                                       

        switch (direction)//1、坐标超载
        {//x→增，y↓增
            //上，下，左，右
            case "u":
                if ((y + 3) > 9 || (x + 2) > 9 || (x - 2) < 0) flag = false;
                break;
            case "d":
                if ((y - 3) < 0 || (x + 2) > 9 || (x - 2) < 0) flag = false;
                break;
            case "l":
                if ((x + 3) > 9 || (y + 1) > 9 || (y - 1) < 0) flag = false;
                break;
            case "r":
                if ((x - 3) < 0 || (y - 1) < 0 || (y + 1) > 9) flag = false;
                break;
        }
        if (!flag) return false;//如果flag满足上述任何一个越界条件，flag=false，则返回
        //如不满足，则检测飞机重叠情况，由于采用了dictionary数据结构，只需要拿飞机坐标调用其中的方法judgeCoor()检测有没有在head||body中，ture表示重叠
        bool f=airplanePlace.judgeCoor(x, y);
        switch (direction)
        {//上，下，左，右
            case "u":
                f = f || airplanePlace.judgeCoor(x, y + 1);
                f = f || airplanePlace.judgeCoor(x - 1, y + 1);
                f = f || airplanePlace.judgeCoor(x - 2, y + 1);
                f = f || airplanePlace.judgeCoor(x + 1, y + 1);
                f = f || airplanePlace.judgeCoor(x + 2, y + 1);

                f = f || airplanePlace.judgeCoor(x, y + 2);

                f = f || airplanePlace.judgeCoor(x, y + 3);
                f = f || airplanePlace.judgeCoor(x - 1, y + 3);
                f = f || airplanePlace.judgeCoor(x + 1, y + 3);
                break;
            case "d":
                f = f || airplanePlace.judgeCoor(x, y - 1);
                f = f || airplanePlace.judgeCoor(x - 1, y - 1);
                f = f || airplanePlace.judgeCoor(x - 2, y - 1);
                f = f || airplanePlace.judgeCoor(x + 1, y - 1);
                f = f || airplanePlace.judgeCoor(x + 2, y - 1);

                f = f || airplanePlace.judgeCoor(x, y - 2);

                f = f || airplanePlace.judgeCoor(x, y - 3);
                f = f || airplanePlace.judgeCoor(x - 1, y - 3);
                f = f || airplanePlace.judgeCoor(x + 1, y - 3);
                break;
            case "l":
                f = f || airplanePlace.judgeCoor(x + 1, y);
                f = f || airplanePlace.judgeCoor(x + 1, y - 1);
                f = f || airplanePlace.judgeCoor(x + 1, y - 2);
                f = f || airplanePlace.judgeCoor(x + 1, y + 1);
                f = f || airplanePlace.judgeCoor(x + 1, y + 2);

                f = f || airplanePlace.judgeCoor(x + 2, y);

                f = f || airplanePlace.judgeCoor(x + 3, y);
                f = f || airplanePlace.judgeCoor(x + 3, y - 1);
                f = f || airplanePlace.judgeCoor(x + 3, y + 1);
                break;
            case "r":
                f = f || airplanePlace.judgeCoor(x - 1, y);
                f = f || airplanePlace.judgeCoor(x - 1, y - 1);
                f = f || airplanePlace.judgeCoor(x - 1, y - 2);
                f = f || airplanePlace.judgeCoor(x - 1, y + 1);
                f = f || airplanePlace.judgeCoor(x - 1, y + 2);

                f = f || airplanePlace.judgeCoor(x - 2, y);

                f = f || airplanePlace.judgeCoor(x - 3, y);
                f = f || airplanePlace.judgeCoor(x - 3, y - 1);
                f = f || airplanePlace.judgeCoor(x - 3, y + 1);
                break;
        }
        if(f)return false;//如果有任何一个重叠，f=ture，则说明重叠，返回false

        //经过多轮判断，说明已经通过了，则将其丢入
        flag=airplanePlace.AddAirplane(x, y, direction);
        return flag;
    }

    public void LogBombResultOnOpponentAirfield(Coordinate coordinate, BombResult result)//炸敌方结果存储在OpponentAirfield表中
    {
        int x = coordinate.X; int y = coordinate.Y;
        OpponentAirfield opponentAirfield = OpponentAirfield.getOpponentAirfield();
        opponentAirfield.AddCoordinate(x,y,result);

    }
        public bool ShouldTerminate()//判断游戏是否结束
    {//即判断三个飞机头坐标是否在BeBombed表中
        AirplanePlace airplanePlace = AirplanePlace.getAirplanePlace();
        BeBombed beBombed = BeBombed.getBeBombed();
        int[] x = airplanePlace.getX();
        int[] y = airplanePlace.getY();
        bool[]  flag=new bool[x.Length];
        for(int i = 0; i < x.Length; i++)
        {
            flag[i] = beBombed.judge(x[i], y[i]);
        }

        return flag[0] && flag[1] && flag[2];
    }
    public BombResult GetBombResultOnMyAirfieldd(Coordinate coordinate)//被炸判断
    {
        int x = coordinate.X; int y = coordinate.Y;
        //对当前坐标轰炸，获取本方机场AirplanePlace类,以便确定,获取被炸表，以便添加
        AirplanePlace airplanePlace=AirplanePlace.getAirplanePlace();
        BeBombed beBombed = BeBombed.getBeBombed();
        bool flag = airplanePlace.judgeHead(x, y);

        if (flag) {
            beBombed.AddBeBombed(x, y, BombResult.Destroyed);
            return BombResult.Destroyed; 
        }
        bool flag2 = airplanePlace.judgebody(x, y);
        if (flag2)
        {
            beBombed.AddBeBombed(x, y, BombResult.Hit);
            return BombResult.Hit;
        }

        beBombed.AddBeBombed(x, y, BombResult.Miss);
        return BombResult.Miss;
    }




}