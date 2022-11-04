using Common;

namespace Database;

public class BeBombed
{
    private static BeBombed beBombed; //静态私有成员变量

    //原来用的是HashSet数据结构，但在判断是否在其中的方法时，.contain()根据的是地址判断的
    //更新后：给10*10的坐标赋一个key，key=x*10+y，采用dictionary<int,BombResult>,来存坐标，状态
    //此外，dictionary和JAVA 中的Hashmap是一样的，都是<key,value>键值对
    private readonly Dictionary<int, BombResult> Beb = new();

    //私有构造函数
    private BeBombed()
    {
    }

    //添加某个坐标进BeBombed表中
    public bool AddBeBombed(int x, int y, BombResult state)
    {
        var key = x * 10 + y;

        var flag = Beb.ContainsKey(key);
        if (!flag) Beb.Add(key, state);

        return flag;
    }

    public bool judge(int x, int y)
    {
        var key = x * 10 + y;
        var flag = Beb.ContainsKey(key);
        return flag;
    }

    public Dictionary<int, BombResult> getBeb()
    {
        return Beb;
    }

    //静态公有工厂方法，返回唯一实例
    public static BeBombed getBeBombed()
    {
        if (beBombed == null)
            beBombed = new BeBombed();
        return beBombed;
    }
}