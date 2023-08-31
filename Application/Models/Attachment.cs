using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;


public class Attachment
{
    [DefaultValue(false)]
    public bool IsInText { get; set; }
    [Required]
    [DefaultValue("/")]
    [DataType(DataType.ImageUrl)]
    public string Path { get; set; }
    [Required]
    [DefaultValue(false)]
    public bool IsSpoiler { get; set; }
}