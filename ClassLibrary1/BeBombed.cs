﻿namespace ClassLibrary1
{
    public class BeBombed
    {
        private HashSet<int[]> beBombed = new HashSet<int[]>();

        //存被炸的坐标，HashSet具有数据唯一性，当然对这里不影响
        public HashSet<int[]> bebombed { get => beBombed; set => beBombed = value; }

        private static BeBombed airplane = null;//静态私有成员变量
        //私有构造函数
        private BeBombed() {}

        //静态公有工厂方法，返回唯一实例
        public static BeBombed getAirplane()
        {
            if(airplane == null)
                airplane = new BeBombed();
            return airplane;
        }

    }
}