namespace Online;

public class Utility
{
    public static string FetchPublicIPv6Address()
    {
        var apis = new Dictionary<string, Func<string, string>>
        {
            { "https://ip6only.me/api/", s => s.Split(",")[1] },
            { "https://api-ipv6.ip.sb/ip", s => s }
        };
        using var c = new HttpClient();
        foreach (var (api, trans) in apis)
            try
            {
                var t = c.GetStringAsync(api);
                t.Wait();
                var ipv6Address = trans(t.Result);
                Console.Out.WriteLine("ipv6Address = {0}", ipv6Address);
                return ipv6Address;
            }
            catch (HttpRequestException)
            {
            }

        throw new Exception("can not get fetch ip address of this machine");
    }
}