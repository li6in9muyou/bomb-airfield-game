using System.Text.RegularExpressions;

namespace Online;

public interface ICommunicator
{
    bool IsLostConnection();
    string Read();
    string Expect(Regex expected);
    void Write(string message);
    void Start();
    void Start(string remoteHandle);
    void Stop();
    string RemoteHandle();
}

public class ExpectMismatch : Exception
{
    public ExpectMismatch(Regex expected, string actual)
        : base($"expecting {expected}, got {actual}")
    {
    }
}

public class MockReceiving : ICommunicator
{
    private readonly string[] _received;
    private readonly List<string> _send = new();
    private int _current;
    private string? _remoteHandle;

    public MockReceiving(string[] received)
    {
        _received = received;
    }

    public bool IsLostConnection()
    {
        return false;
    }

    public string Read()
    {
        if (_current < _received.Length) return _received[_current++];

        throw new Exception();
    }

    public string Expect(Regex expected)
    {
        var t = Read();
        if (!expected.IsMatch(t)) throw new ExpectMismatch(expected, t);
        return t;
    }

    public void Write(string message)
    {
        _send.Add(message);
    }

    public void Start()
    {
        Console.Out.WriteLine("MockReceiving.Start()");
    }

    public void Start(string remoteHandle)
    {
        _remoteHandle = remoteHandle;
        Console.Out.WriteLine($"MockReceiving.Start({remoteHandle})");
    }

    public void Stop()
    {
        Console.Out.WriteLine("MockReceiving.Stop()");
    }

    public string RemoteHandle()
    {
        return _remoteHandle!;
    }

    public string[] GetMessagesSentToRemote()
    {
        return _send.ToArray();
    }
}