namespace ClassLibrary1
{
    public class airplane1
    {
       private int[] head = new int[2];
       private  string direction = null;
        //up down left right
        public int[] Head { get => head; set => head = value; }
        public string Direction { get => direction; set => direction = value; }


        private static airplane1 airplane = null;//静态私有成员变量

      
        //私有构造函数
        private airplane1() {}

        //静态公有工厂方法，返回唯一实例
        public static airplane1 getAirplane()
        {
            if(airplane == null)
                airplane = new airplane1();
            return airplane;
        }

    }
}