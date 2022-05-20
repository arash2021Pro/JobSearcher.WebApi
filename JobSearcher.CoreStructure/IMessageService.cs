namespace JobSearcher.CoreStructure;

public interface IMessageService
{
    Task SendMessage(string sender, string receptor, string message);
    
}