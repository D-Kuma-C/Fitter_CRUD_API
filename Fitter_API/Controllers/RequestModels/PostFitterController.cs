using System.ComponentModel.DataAnnotations;

namespace Fitter_API.Controllers.RequestModels
{
    public class PostFitterController
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        public IEnumerable<int> SeniorId { get; set; } = new List<int>();
    }
}
