using Hangfire;
using JobSearcher.CoreStructure;
using MediatR;

namespace JobSearcher.CqrsOperations.Users;

public class SignupEventNotification:INotification
{
    public string Phonenumber { get; set; }
    public string Message { get; set; }
    
}

public class SignupSendMessageNotificationHandler : INotificationHandler<SignupEventNotification>
{
    private IMessageService _messageService;

    public SignupSendMessageNotificationHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Handle(SignupEventNotification notification, CancellationToken cancellationToken)
    {
       BackgroundJob.Enqueue(() => _messageService.SendMessage("JobSearcher", notification.Phonenumber, notification.Message));
    }
}
