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

public class MockCommunicator : ICommunicator
{
    public bool IsLostConnection()
    {
        throw new NotImplementedException();
    }

    public string Read()
    {
        throw new NotImplementedException();
    }

    public string Expect(Regex expected)
    {
        throw new NotImplementedException();
    }

    public void Write(string message)
    {
        throw new NotImplementedException();
    }

    public void Start()
    {
        throw new NotImplementedException();
    }

    public void Start(string remoteHandle)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public string RemoteHandle()
    {
        throw new NotImplementedException();
    }
}