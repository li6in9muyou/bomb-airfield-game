namespace Database;

public class AirplanePlace
{
    private static AirplanePlace airplanePlace; //静态私有成员变量
    private Airplane Airplane1;
    private Airplane Airplane2;

    private Airplane Airplane3;

    //私有构造函数
    private AirplanePlace()
    {
    }

    //静态公有工厂方法，返回唯一实例
    public static AirplanePlace getAirplane()
    {
        if (airplanePlace == null)
            airplanePlace = new AirplanePlace();
        return airplanePlace;
    }
}