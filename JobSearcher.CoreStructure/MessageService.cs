using Kavenegar;
using Microsoft.Extensions.Options;

namespace JobSearcher.CoreStructure;

public class MessageService:IMessageService
{
    public MessageOption option;
    public MessageService(IOptionsSnapshot<MessageOption>options)
    {
        option = options.Value;
    }
    public async Task SendMessage(string sender, string receptor, string message)
    {
        var api = new KavenegarApi(option.ApiKey);
        await api.Send(sender, receptor, message);
    }
}