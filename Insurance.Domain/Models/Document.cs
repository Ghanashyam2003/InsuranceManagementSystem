using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "Entry Type is Required")]
        public string EntityType { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "File Path is Required")]
        public string FilePath { get; set; }

        public DateTime UploadDate { get; set; }
    }
}