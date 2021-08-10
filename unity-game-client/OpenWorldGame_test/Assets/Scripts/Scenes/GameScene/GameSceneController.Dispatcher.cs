using UnityEngine;

public partial class GameSceneController : MonoBehaviour
{
    public Dispatcher dispatcher;

    public void StartDispatcher()
    {
        dispatcher = new Dispatcher();
    }

    private void UpdateDispatcher()
    {
        dispatcher.Execute();
    }
}
