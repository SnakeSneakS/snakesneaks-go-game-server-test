
//Gamemodel.Gameclient 
public partial class Gamemodel 
{
    public enum ClientConnectionState
    {
        None,
        Active,
        Disconnected
    }

    public struct GameClient
    {
        ClientConnectionState connectionState;
        GameClientInfo gameClientInfo;
    }

    public struct GameClientInfo
    {
        string user_id;
        string username;
    }
    
}
