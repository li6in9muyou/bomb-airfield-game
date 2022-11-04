using System.Xml.Serialization;

namespace Database;

public class AirplanePlace
{
    private Airplane Airplane1 = null;
    private Airplane Airplane2 = null;
    private Airplane Airplane3 = null;

    private int[] X = new int[3];
    private int[] Y = new int[3];
    private String[] direction = new string[3];

    //存头坐标集合，身体坐标集合，仅用来(本方挨炸环节、绘制图像环节)，快速判断并返回炸的结果 
    //通过新加入飞机(AddAirplane)时调用 私有 方法mergeHead(i)，mergeBody(i)来填充
    private Dictionary<int, int[]> head = new Dictionary<int, int[]>();
    private Dictionary<int, int[]> body = new Dictionary<int, int[]>();

    //检测一个坐标是否在head/body中
    public Boolean judgeCoor(int x,int y)
    {
        int key = x * 10 + y;
        bool flag = this.head.ContainsKey(key);
        bool flag2 = this.body.ContainsKey(key);
        return flag || flag2;//只要有其中一个在，就返回ture，意思是已经重叠
    }
    public Boolean judgeHead(int x,int y)
    {
        int key=x * 10 + y;
        return this.head.ContainsKey(key);
    }
    public Boolean judgebody(int x,int y)
    {
        int key=x * 10 + y;
        return this.body.ContainsKey(key);
    }


    public Boolean AddAirplane(int x, int y, String d)
    {//存1，2，3，这里逻辑有问题，但是人和程序其中一个能跑就行
        if (Airplane1 == null) { Airplane1 = new Airplane(x, y, d); mergeAirplane();mergeHead(0);mergeBody(0);}
        else if (Airplane2 == null) { Airplane2 = new Airplane(x, y, d); mergeAirplane(); mergeHead(1); mergeBody(1); }
        else if (Airplane3 == null) { Airplane3 = new Airplane(x, y, d); mergeAirplane(); mergeHead(2); mergeBody(2); }
        else return false;

        return true;
    }
    public void mergeAirplane()
    {
        if (Airplane1 != null)
        {
            X[0] = Airplane1.X; Y[0] = Airplane1.Y; direction[0] = Airplane1.Direction;
        }
        if (Airplane2 != null)
        {
            X[1] = Airplane2.X; Y[1] = Airplane2.Y; direction[1] = Airplane2.Direction;
        }
        if (Airplane3 != null)
        {
            X[2] = Airplane3.X; Y[2] = Airplane3.Y; direction[2] = Airplane3.Direction;

        }
    }
    private void mergeHead(int i)
    {   
        this.mergeAirplane();//再调用一遍
        //存三个飞机头坐标到Dictionary_head中
    
        int key = X[i] * 10 + Y[i];
        int[] value =new int[2];
        value[0] = X[i]; value[1] = Y[i];

        bool flag = this.head.ContainsKey(key);//这个key，value是否已经被存到Dictionary中，没有存返回flase
        if(!flag)this.head.Add(key, value);//没有被存那就现在存


    }

    //至少调用3*9=27次，这里将存入mergeBody的操作提取出来，节省代码空间
    private void  addbody(int x,int y)
    {
        int key = x * 10 + y;
        int[] value = { x, y };
        bool flag= this.body.ContainsKey(key);
        if(!flag)this.body.Add(key, value);
    }
    private void mergeBody(int i)
    {   //根据机头，方向，算出飞机身体坐标并存在Dictionary_body中

        this.mergeAirplane();//再调用一遍
        //机头x坐标，y坐标，方向
        int x = X[i];int y = Y[i];String d = direction[i];
            switch (d)
            {//上，下，左，右
                case "u":
                    addbody(x, y + 1);
                    addbody(x -1, y + 1);
                    addbody(x - 2, y + 1);
                    addbody(x + 1, y + 1);
                    addbody(x + 2, y + 1);

                    addbody(x, y + 2);

                    addbody(x, y + 3);
                    addbody(x - 1, y + 3);
                    addbody(x + 1, y + 3);
                    break;
                case "d":
                    addbody(x, y - 1);
                    addbody(x-1, y - 1);
                    addbody(x-2, y - 1);
                    addbody(x+1, y - 1);
                    addbody(x+2, y - 1);

                    addbody(x, y - 2);

                    addbody(x, y - 3);
                    addbody(x-1, y - 3);
                    addbody(x+1, y - 3);
                    break;
                case "l":
                    addbody(x + 1, y);
                    addbody(x + 1, y-1);
                    addbody(x + 1, y-2);
                    addbody(x + 1, y+1);
                    addbody(x + 1, y+2);

                    addbody(x + 2, y);

                    addbody(x + 3, y);
                    addbody(x + 3, y - 1);
                    addbody(x + 3, y + 1);
                    break;
                case "r":
                    addbody(x - 1, y);
                    addbody(x - 1, y-1);
                    addbody(x - 1, y-2);
                    addbody(x - 1, y+1);
                    addbody(x - 1, y+2);

                    addbody(x - 2, y);

                    addbody(x - 3, y);
                    addbody(x - 3, y-1);
                    addbody(x - 3, y+1);
                    break;
            }

    }

    //这个写了不一定要用，或者说最好不要用到
    public Airplane GetAirplane1() { return Airplane1; }
    public Airplane GetAirplane2() { return Airplane2; }
    public Airplane GetAirplane3() { return Airplane3; }

    public int[] getX() { return X; }
    public int[] getY() { return Y; }
    public String[] getDirection() { return direction; }


    private static AirplanePlace airplanePlace = null;//静态私有成员变量
                                                      //私有构造函数
    private AirplanePlace() { }

    //静态公有工厂方法，返回唯一实例
    public static AirplanePlace getAirplanePlace()
    {
        if (airplanePlace == null)
            airplanePlace = new AirplanePlace();
        return airplanePlace;
    }
}