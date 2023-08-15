namespace Application.Models;

public enum AttachmentType
{
    Url,
    UrlImage,
    UrlGif,
    Image,
    Gif,
    Video,
    Audio
}

public class Attachment
{
    public int Id { get; set; }
    public AttachmentType Type { get; set; }
    public string Path { get; set; }
    public bool IsSpoiler { get; set; }
    public virtual Message? Message { get; set; }
}