//Model.HttpResponse 
public partial class Model
{
    [System.Serializable]
    public enum Status
    {
        failed,
        success
    }

    [System.Serializable]
    public struct StatusRes
    {
        public string status; 
    }

    [System.Serializable]
    public struct SessionRes
    {
        public string status;
        public Session session; 
    }
}
