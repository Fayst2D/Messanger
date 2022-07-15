namespace Messenger.Domain.Entities;

public class FileEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public string ContentType { get; init; } = ""; 
    public string FilePath { get; init; } = "";

    public UserEntity? User { get; set; }
}