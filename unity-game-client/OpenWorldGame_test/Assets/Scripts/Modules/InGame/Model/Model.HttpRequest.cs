//Model.HttpRequest 
public partial class Model 
{
    [System.Serializable]
    public struct SessionReq
    {
        [UnityEngine.SerializeField] Session session; 
    }

    [System.Serializable]
    public struct UserReq
    {
        [UnityEngine.SerializeField] User user; 
    }
}
