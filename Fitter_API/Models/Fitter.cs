using System.ComponentModel.DataAnnotations;

namespace Fitter_API.Models
{
    public class Fitter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public ICollection<SeniorFitter> SeniorFitters { get; set; }

        public Fitter()
        {
            SeniorFitters = new List<SeniorFitter>();
        }
    }
}
