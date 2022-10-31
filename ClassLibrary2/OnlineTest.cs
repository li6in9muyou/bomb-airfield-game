using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Online;

internal class OnlineTest
{
    public static void TestCreateRoom()
    {
        var online = new Online();
        var addr = online.CreateRoom();
        Console.Out.WriteLine("addr = {0}", addr);
        var isFirst = online.WaitOpponentToJoin();
        Console.Out.WriteLine("isFirst = {0}", isFirst);
        online.AbandonRoom();
    }

    public static void AnEchoServer()
    {
        var server = new TcpListener(IPAddress.Loopback, 61234);
        server.Start();
        var bytes = new byte[256];
        Console.Write("Waiting for a connection... ");
        using var client = server.AcceptTcpClient();

        Console.WriteLine("Connected!");
        var stream = client.GetStream();
        int i;
        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            var data = "echo: " + Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("Received: {0}", data);
            var msg = Encoding.ASCII.GetBytes(data);
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("Sent: {0}", data);
        }

        client.Close();
    }
}