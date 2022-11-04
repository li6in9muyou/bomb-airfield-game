using Newtonsoft.Json;
using Fleck;
namespace UserInterface;

public class MsgRoot
{
    public string header { get; set; }
    public string body { get; set; }
}
public class UIServer
{
    private static WebSocketServer? _server;
    private static IWebSocketConnection? _socket;
    public static void Init() 
    {
        _server = new WebSocketServer("ws://127.0.0.1:9898");
        _server.Start(socket => {
            socket.OnOpen = () =>
            {
                _socket = socket;
                Console.WriteLine("Open");
            };
            socket.OnClose = () =>
            {
                _socket = null;
                Console.WriteLine("Close");
            };
            socket.OnMessage = message => {
                Console.WriteLine(message);//接收消息，判断类型，放入相应缓存
                SaveMsg(message);
            };
        });
        while (true) 
        {
            Console.Write("> ");
            String? cmd = Console.ReadLine();
            if (cmd!=null&&cmd.Equals("close"))break;
        }
    }
    public static void SendMsg(String header,String body)
    {
        if (_socket != null)
        {
            //俩参数，一头一内容，在此合并
            var data = new { header = header, body = body };
            string jsonData = JsonConvert.SerializeObject(data);
            _socket.Send(jsonData);
        }
    }
    public static void SaveMsg(String msg)
    {
        if (_socket != null)
        {
            MsgRoot msgRoot = JsonConvert.DeserializeObject<MsgRoot>(msg);
            if (msgRoot.header.Equals("IpAddress"))
            {
                UiCache.SetIpAddress(msgRoot.body);
            }else if (msgRoot.header.Equals("BombLocation"))
            {
                UiCache.SetBombLocation(msgRoot.body);
            }else if (msgRoot.header.Equals("AirplanesPlacement"))
            {
                UiCache.SetAirplanesPlacement(msgRoot.body);
            }
        }
    }
}
