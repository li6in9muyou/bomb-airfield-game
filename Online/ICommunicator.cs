using System;
using System.Text.RegularExpressions;

namespace Online;

public interface ICommunicator
{
    bool IsLostConnection();
    string Read();
    string Expect(Regex expected);
    void Write(string message);
    void Start();
    void Stop();
}

public class ExpectMismatch : Exception
{
    public ExpectMismatch(Regex expected, string actual)
        : base($"expecting {expected}, got {actual}")
    {
    }
}