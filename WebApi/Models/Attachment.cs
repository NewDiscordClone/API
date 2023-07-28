namespace WebApi.Models;

public enum AttachmentType
{
    
}

public class Attachment
{
    public int Id { get; set; }
    public AttachmentType Type { get; set; }
    public string Path { get; set; }
    public bool IsSpoiler { get; set; }

    public virtual Message? Message { get; set; }
}