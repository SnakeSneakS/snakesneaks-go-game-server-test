//Model.HttpResponse 
public partial class Model
{
   
    [System.Serializable]
    public struct StatusRes
    {
        public ConnStatus status; 
    }

    [System.Serializable]
    public struct SessionRes
    {
        public ConnStatus status;
        public Session session; 
    }
}
