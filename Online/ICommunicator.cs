using System.Text.RegularExpressions;
using Common;
using Easy.Logger.Interfaces;

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
    private readonly IEasyLogger _note;
    private readonly string[] _received;
    private readonly List<string> _send = new();
    private int _current;
    private string? _remoteHandle;

    public MockReceiving(string[] received)
    {
        _received = received;
        _note = Logging.GetLogger("Mock--CommunicatorReceiving");
    }

    public bool IsLostConnection()
    {
        return false;
    }

    public string Read()
    {
        if (_current < _received.Length)
        {
            var r = _received[_current++];
            _note.Debug($"reading {r}");
            return r;
        }

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
        _note.Debug($"writing {message}");
        _send.Add(message);
    }

    public void Start()
    {
        _note.Debug("MockReceiving.Start()");
    }

    public void Start(string remoteHandle)
    {
        _remoteHandle = remoteHandle;
        _note.Debug($"MockReceiving.Start({remoteHandle})");
    }

    public void Stop()
    {
        _note.Debug("MockReceiving.Stop()");
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