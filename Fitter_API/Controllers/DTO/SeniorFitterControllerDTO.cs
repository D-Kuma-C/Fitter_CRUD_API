namespace Fitter_API.Controllers.DTO
{
    public class SeniorFitterControllerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public ICollection<FitterDTO> Fitters { get; set; }
    }
}
