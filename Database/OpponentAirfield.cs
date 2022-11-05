using Common;

namespace Database;


public class OpponentAirfield
{
    //坐标+状态，由于设定了key=x*10+y，一个key对应一个坐标(仅限于10*10格子)，则可以让value成为状态
    Dictionary<int,BombResult> opponent = new Dictionary<int,BombResult>();
    //这里约定BombResult状态为：Miss,Hit,Destroyed
    //静态私有成员变量
    private static OpponentAirfield opponentAirfield=null;

    public Boolean AddCoordinate(int x,int y,BombResult state){

        int key = x * 10 + y;
        bool flag = this.opponent.ContainsKey(key);
        if (!flag) this.opponent.Add(key, state);

        return flag;
    }
    public Dictionary<int,BombResult> getOpponent()
    {
        return opponent;
    }

    //私有构造函数
    private OpponentAirfield() { }
    //静态公有工厂方法，返回唯一实例
    public static OpponentAirfield getOpponentAirfield()
    {
        if (opponentAirfield == null)
            opponentAirfield = new OpponentAirfield();
        return opponentAirfield;
    }












}
