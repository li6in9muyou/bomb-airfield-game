namespace ClassLibrary1
{
    public class Airplane2
    {
        private int[] head = new int[2];
        private string direction = null;

        public int[] Head { get => head; set => head = value; }
        public string Direction { get => direction; set => direction = value; }



        private static Airplane2 airplane = null;//静态私有成员变量
        //私有构造函数
        private Airplane2() {}

        //静态公有工厂方法，返回唯一实例
        public static Airplane2 getAirplane()
        {
            if(airplane == null)
                airplane = new Airplane2();
            return airplane;
        }

    }
}