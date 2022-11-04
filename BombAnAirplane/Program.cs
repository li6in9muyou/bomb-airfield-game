namespace BombAnAirplane;

internal static class Program
{
    private static void Main()
    {
        /*      HashSet<int> beb = new HashSet<int>();
              int a = 1;
              int b = 1;
              beb.Add(a);
              bool c=beb.Contains(b);
              Console.WriteLine("{0}",c);*/
        Dictionary<int, int[]> beb = new Dictionary<int, int[]>();
        int[] a = { 1, 2 };
        int aa = 10 * a[0] + a[1];
       


        bool flag = beb.ContainsKey(12);
        Console.WriteLine(flag);
        if (!flag)
        {
            beb.Add(12,a);
        }



        int[] c=new int[2];
        bool b = beb.TryGetValue(13,out c);

        if(b)foreach(var i in c)
            Console.WriteLine(i);
    }
}