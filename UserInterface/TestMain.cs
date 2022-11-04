using Fleck;

namespace UserInterface;

public class TestMain
{
    public static void RunServer(){
        var server = new WebSocketServer("ws://127.0.0.1:9898");    //创建webscoket服务端实例
            server.Start(socket => {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open");
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close");
                };
                socket.OnMessage = message => {
                    Console.WriteLine(message);
                    socket.Send(message);
                };
            });
            while (true)
            {
                Console.WriteLine("> ");
                String? cmd = Console.ReadLine();
                if (cmd!=null&&cmd.Equals("close"))break;
            }
    }
}
