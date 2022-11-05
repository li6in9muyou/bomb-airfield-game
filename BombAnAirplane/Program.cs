using Common;
using UserInterface;

namespace BombAnAirplane;

internal static class Program
{
    private static void Main()
    {
        UIServer.Init();
        Console.WriteLine("Main running...");
        UserInterfaceAdapter userInterfaceAdapter = new UserInterfaceAdapter();
        Coordinate c = userInterfaceAdapter.WaitLocalUserChooseBombLocation(null);
    }
}