using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace lab_4.Models
{
    public class FileUploadModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}