using Common;
using Database;

namespace GameLogic;

public class GameLogic
{
    public bool setAirplane(int x, int y, string direction) //摆飞机
    {
        //传入飞机头，方向，经过算法(排除机头坐标重复，飞机重叠，坐标超载等情况)，返回bool值，true成功--》传入数据类中，false失败--》需要再次输入(逻辑在界面类实现)
        var flag = true;
        var airplanePlace = AirplanePlace.getAirplanePlace(); //获取存有
        //飞机坐标的类
        airplanePlace.merge(); //把飞机中的数据提取出来

        switch (direction)
        {
            //x→增，y↓增
            case "u":
                if (x + 2 > 9 || x - 2 < 0) flag = false;
                if (y < 0 || y + 3 > 9) flag = false;
                break;
            case "d":

                break;
            case "l":

                break;
            case "r":

                break;
        }


        return flag;
    }

    public bool ShouldTerminate()
    {
        throw new NotImplementedException();
    }

    public void LogBombResultOnOpponentAirfield(Coordinate coordinate, BombResult result)
    {
        throw new NotImplementedException();
    }

    public BombResult GetBombResultOnMyAirfield(Coordinate coordinate)
    {
        throw new NotImplementedException();
    }
}