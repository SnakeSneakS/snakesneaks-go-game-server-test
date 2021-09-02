
//GameMethod.ExitWorld 
public partial class GameMethodHandler 
{
    public delegate void ExitWorldEventHandler<T>(T args);
    public event ExitWorldEventHandler<ExitWorldEventArgs> OnExitWorldEventHandler;

    public struct ExitWorldEventArgs
    {
        public uint user_id;
    }

    /// <summary>
    /// Recieve ExitWorld Message From Server: delete client data in local 
    /// </summary>
    /// <param name="user_id"></param>
    public void ReceiveExitWorld(uint user_id)
    {
        OnExitWorldEventHandler?.Invoke(new ExitWorldEventArgs { user_id = user_id });
    }
}
