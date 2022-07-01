namespace Messenger.Domain.Entities;

public class FileEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public string ContentType { get; set; }
    public string FilePath { get; set; }

    public UserEntity User { get; set; }
}