namespace Online;

public class Utility
{
    public static string FetchPublicIPv6Address()
    {
        using var c = new HttpClient();
        var t = c.GetStringAsync("https://ip6only.me/api/");
        // todo: 如果这个地址访问不到怎么办，搞多几个这样的api作为后备。
        t.Wait();
        var ipv6Address = t.Result.Split(",")[1];
        Console.Out.WriteLine("ipv6Address = {0}", ipv6Address);
        return ipv6Address;
    }
}