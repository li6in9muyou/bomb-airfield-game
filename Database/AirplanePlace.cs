namespace Database;

public class AirplanePlace
{
    private static AirplanePlace _airplanePlace; //静态私有成员变量
    private readonly Dictionary<int, int[]> _body = new();
    private readonly string[] _direction = new string[3];

    //存头坐标集合，身体坐标集合，仅用来(本方挨炸环节、绘制图像环节)，快速判断并返回炸的结果 
    //通过新加入飞机(AddAirplane)时调用 私有 方法mergeHead(i)，mergeBody(i)来填充
    private readonly Dictionary<int, int[]> _head = new();

    private readonly int[] _x = new int[3];
    private readonly int[] _y = new int[3];
    private Airplane _airplane1;
    private Airplane _airplane2;
    private Airplane _airplane3;

    //私有构造函数
    private AirplanePlace()
    {
    }

    //检测一个坐标是否在head/body中
    public bool JudgeCoord(int x, int y)
    {
        var key = x * 10 + y;
        var flag = _head.ContainsKey(key);
        var flag2 = _body.ContainsKey(key);
        return flag || flag2; //只要有其中一个在，就返回ture，意思是已经重叠
    }

    public bool JudgeHead(int x, int y)
    {
        var key = x * 10 + y;
        return _head.ContainsKey(key);
    }

    public bool JudgeBody(int x, int y)
    {
        var key = x * 10 + y;
        return _body.ContainsKey(key);
    }


    public bool AddAirplane(int x, int y, string d)
    {
        //存1，2，3，这里逻辑有问题，但是人和程序其中一个能跑就行
        if (_airplane1 == null)
        {
            _airplane1 = new Airplane(x, y, d);
            MergeAirplane();
            MergeHead(0);
            MergeBody(0);
        }
        else if (_airplane2 == null)
        {
            _airplane2 = new Airplane(x, y, d);
            MergeAirplane();
            MergeHead(1);
            MergeBody(1);
        }
        else if (_airplane3 == null)
        {
            _airplane3 = new Airplane(x, y, d);
            MergeAirplane();
            MergeHead(2);
            MergeBody(2);
        }
        else
        {
            return false;
        }

        return true;
    }

    public void MergeAirplane()
    {
        if (_airplane1 != null)
        {
            _x[0] = _airplane1.X;
            _y[0] = _airplane1.Y;
            _direction[0] = _airplane1.Direction;
        }

        if (_airplane2 != null)
        {
            _x[1] = _airplane2.X;
            _y[1] = _airplane2.Y;
            _direction[1] = _airplane2.Direction;
        }

        if (_airplane3 != null)
        {
            _x[2] = _airplane3.X;
            _y[2] = _airplane3.Y;
            _direction[2] = _airplane3.Direction;
        }
    }

    private void MergeHead(int i)
    {
        MergeAirplane(); //再调用一遍
        //存三个飞机头坐标到Dictionary_head中

        var key = _x[i] * 10 + _y[i];
        var value = new int[2];
        value[0] = _x[i];
        value[1] = _y[i];

        var flag = _head.ContainsKey(key); //这个key，value是否已经被存到Dictionary中，没有存返回flase
        if (!flag) _head.Add(key, value); //没有被存那就现在存
    }

    //至少调用3*9=27次，这里将存入mergeBody的操作提取出来，节省代码空间
    private void Addbody(int x, int y)
    {
        var key = x * 10 + y;
        int[] value = { x, y };
        var flag = _body.ContainsKey(key);
        if (!flag) _body.Add(key, value);
    }

    private void MergeBody(int i)
    {
        //根据机头，方向，算出飞机身体坐标并存在Dictionary_body中

        MergeAirplane(); //再调用一遍
        //机头x坐标，y坐标，方向
        var x = _x[i];
        var y = _y[i];
        var d = _direction[i];
        switch (d)
        {
            //上，下，左，右
            case "u":
                Addbody(x, y + 1);
                Addbody(x - 1, y + 1);
                Addbody(x - 2, y + 1);
                Addbody(x + 1, y + 1);
                Addbody(x + 2, y + 1);

                Addbody(x, y + 2);

                Addbody(x, y + 3);
                Addbody(x - 1, y + 3);
                Addbody(x + 1, y + 3);
                break;
            case "d":
                Addbody(x, y - 1);
                Addbody(x - 1, y - 1);
                Addbody(x - 2, y - 1);
                Addbody(x + 1, y - 1);
                Addbody(x + 2, y - 1);

                Addbody(x, y - 2);

                Addbody(x, y - 3);
                Addbody(x - 1, y - 3);
                Addbody(x + 1, y - 3);
                break;
            case "l":
                Addbody(x + 1, y);
                Addbody(x + 1, y - 1);
                Addbody(x + 1, y - 2);
                Addbody(x + 1, y + 1);
                Addbody(x + 1, y + 2);

                Addbody(x + 2, y);

                Addbody(x + 3, y);
                Addbody(x + 3, y - 1);
                Addbody(x + 3, y + 1);
                break;
            case "r":
                Addbody(x - 1, y);
                Addbody(x - 1, y - 1);
                Addbody(x - 1, y - 2);
                Addbody(x - 1, y + 1);
                Addbody(x - 1, y + 2);

                Addbody(x - 2, y);

                Addbody(x - 3, y);
                Addbody(x - 3, y - 1);
                Addbody(x - 3, y + 1);
                break;
        }
    }

    //这个写了不一定要用，或者说最好不要用到
    public Airplane GetAirplane1()
    {
        return _airplane1;
    }

    public Airplane GetAirplane2()
    {
        return _airplane2;
    }

    public Airplane GetAirplane3()
    {
        return _airplane3;
    }

    public int[] GetX()
    {
        return _x;
    }

    public int[] GetY()
    {
        return _y;
    }

    public string[] GetDirection()
    {
        return _direction;
    }

    //静态公有工厂方法，返回唯一实例
    public static AirplanePlace GetAirplanePlace()
    {
        if (_airplanePlace == null)
            _airplanePlace = new AirplanePlace();
        return _airplanePlace;
    }
}