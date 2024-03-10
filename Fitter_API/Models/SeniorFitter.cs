using System.ComponentModel.DataAnnotations;

namespace Fitter_API.Models
{
    public class SeniorFitter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public ICollection<Fitter> Fitters { get; set; }
        public SeniorFitter() 
        {
            Fitters = new List<Fitter>();
        }

    }
}