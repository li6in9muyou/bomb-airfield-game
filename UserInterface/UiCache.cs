namespace UserInterface;

public class UiCache
{
    private static String? IpAddress = null;
    private static String? BombLocation = null;
    private static Boolean isBombLocationNew = false;
    private static String? AirplanesPlacement = null;

    public static String WaitIpAddress()
    {
        while (IpAddress==null)
        {
            Thread.Sleep(3000);
        }
        return IpAddress;
    }

    public static String WaitBombLocation()
    {
        while (!isBombLocationNew)
        {
            Thread.Sleep(3000);
        }
        isBombLocationNew = false;
        return BombLocation;
    }
    
    public static String WaitAirplanesPlacement()
    {
        while (AirplanesPlacement==null)
        {
            Thread.Sleep(3000);
        }
        return AirplanesPlacement;
    }

    public static void SetIpAddress(String i)
    {
        IpAddress = i;
    }
    public static void SetBombLocation(String b)
    {
        BombLocation = b;
        isBombLocationNew = true;
    }
    public static void SetAirplanesPlacement(String a)
    {
        AirplanesPlacement = a;
    }
}