using Easy.Logger;
using Easy.Logger.Interfaces;

namespace Common;

public class Logging
{
    private static readonly ILogService LogService = Log4NetService.Instance;

    public static IEasyLogger GetLogger(string name)
    {
        LogService.Configure(new FileInfo("./log4net.config"));
        return LogService.GetLogger(name);
    }
}