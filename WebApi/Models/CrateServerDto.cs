using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{

    public record CrateServerDto
    {
        [Required, MaxLength(255)]
        public string Title { get; init; }

        [DataType(DataType.ImageUrl)]
        public string? Image { get; init; }
    }
}
