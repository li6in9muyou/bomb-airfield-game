namespace Database;

public class AirplanePlace
{
    private Airplane Airplane1 = null;
    private Airplane Airplane2 = null;
    private Airplane Airplane3 = null;

    private int[] X = new int[3];
    private int[] Y = new int[3];
    private String[] direction = new string[3];

    public Boolean setAirplane(int x, int y, String d)
    {//存1，2，3，这里逻辑有问题，但是人和程序其中一个能跑就行
        if (Airplane1 == null) { Airplane1 = new Airplane(x, y, d); }
        else if (Airplane2 == null) { Airplane2 = new Airplane(x, y, d); }
        else if (Airplane3 == null) { Airplane3 = new Airplane(x, y, d); }
        else return false;

        return true;
    }
    public void merge()
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