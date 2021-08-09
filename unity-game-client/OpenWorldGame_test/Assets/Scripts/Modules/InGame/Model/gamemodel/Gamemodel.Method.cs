
//Gamemodel.GameMethod 
public partial class Gamemodel
{
    public enum GameMethodType
    {
        Idle,
        EnterWorld,
        ExitWorld,
        Chat,
        Move,
    }

    //Idle
    public struct IdleMethod
    {

    }

    //EnterWorld
    public struct EnterWorldMethod
    {

    }

    //ExitWorld
    public struct ExitWorldMethod
    {

    }

    //Chat
    public struct ChatMethod
    {
        public string text;
    }

    //Move
    public struct MoveMethod
    {
        public Gamemodel.GameReqTransform from;
        public Gamemodel.GameReqTransform to;
    }
}
