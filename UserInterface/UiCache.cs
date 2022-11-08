namespace UserInterface;

public class UiCache
{
    private static string? _ipAddress;
    private static string? _bombLocation;
    private static bool _isBombLocationNew;
    private static string? _airplanesPlacement;

    public static string WaitIpAddress()
    {
        while (_ipAddress == null)
        {
            Console.WriteLine("IP:" + _ipAddress);
            Thread.Sleep(3000);
        }

        return _ipAddress;
    }

    public static string WaitBombLocation()
    {
        while (!_isBombLocationNew)
        {
            Console.WriteLine("BL:" + _bombLocation);
            Thread.Sleep(3000);
        }

        _isBombLocationNew = false;
        return _bombLocation!;
    }

    public static string WaitAirplanesPlacement()
    {
        while (_airplanesPlacement == null)
        {
            Console.WriteLine("AP:" + _airplanesPlacement);
            Thread.Sleep(3000);
        }

        return _airplanesPlacement;
    }

    public static void SetIpAddress(string i)
    {
        _ipAddress = i;
    }

    public static void SetBombLocation(string b)
    {
        _bombLocation = b;
        _isBombLocationNew = true;
    }

    public static void SetAirplanesPlacement(string a)
    {
        _airplanesPlacement = a;
    }
}