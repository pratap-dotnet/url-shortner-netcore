using System.ComponentModel.DataAnnotations;

namespace UrlShortnerNetCore.Models
{
    public class UrlViewModel
    {
        [Required]
        public string Url { get; set; }
    }
}
