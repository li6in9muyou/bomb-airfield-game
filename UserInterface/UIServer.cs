using System.Diagnostics;
using Newtonsoft.Json;
using Fleck;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

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
        Thread serverThread = new Thread(new ThreadStart(ServerRun));
        serverThread.Start();
        Task.Run(() =>
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://127.0.0.1:8080")
                .UseStartup<StartUp>()
                .Build().Run();
            Process.Start("explorer", "http:/127.0.0.1:8080/index.html");
        });
    }
    private static void ServerRun() 
    {
        Console.WriteLine("Server Running...");
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
                Console.WriteLine("接到消息："+message);//接收消息，判断类型，放入相应缓存
                SaveMsg(message);
            };
        });
        while (true) 
        {
            Console.Write("> ");
            String? cmd = Console.ReadLine();
            if (cmd != null)
            {
                if (cmd.Equals("close"))break;
                _socket.Send(cmd);
            }
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
            MsgRoot? msgRoot;
            try
            {
                msgRoot = JsonConvert.DeserializeObject<MsgRoot>(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _socket.Send("不能解析");
                return;
            };
            if(msgRoot!=null)
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
    
    private class StartUp
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"./WebHtmlView")
                )
            });
        }
    }
}
