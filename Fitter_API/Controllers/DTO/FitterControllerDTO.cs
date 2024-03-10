namespace Fitter_API.Controllers.DTO
{
    public class FitterControllerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public ICollection<SeniorFitterDTO> SeniorFitters { get; set; }
    }
}
