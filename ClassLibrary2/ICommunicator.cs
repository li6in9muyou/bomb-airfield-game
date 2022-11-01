namespace Online;

public interface ICommunicator
{
    bool IsLostConnection();
    string Read();
    string Write();
}