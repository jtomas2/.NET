using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Files
{
    public class FileAddRequest
    {
        public string Name { get; set; }

        [Required]
        public int EntityTypeId { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int FileTypeId { get; set; }

        public int CreatedBy { get; set; }
    }
}