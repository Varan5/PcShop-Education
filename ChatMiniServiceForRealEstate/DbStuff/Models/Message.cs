namespace ChatMiniServiceForRealEstate.DbStuff.Models;

public class Message
{
    public string UserName { get; set; }
    
    public string MessageText { get; set; }
    
    public DateTime DataCreate { get; set; }
}