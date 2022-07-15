using System.ComponentModel;

namespace Messenger.BusinessLogic.Commands.Messages.Send;

public class SendMessageRequest
{
    [DefaultValue("message")]
    public string MessageText { get; set; }
    [DefaultValue(@"D:\Messenger\07.07.20220586505e-55fe-4c6a-94e5-9c92072def1dтоп1.jpeg")]
    public string Attachment { get; set; }
    [DefaultValue("6b66fa72-9621-41fd-8adb-a4306349480b")]
    public Guid ChatId { get; set; }
}