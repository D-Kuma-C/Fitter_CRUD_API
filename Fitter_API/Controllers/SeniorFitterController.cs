using Fitter_API.Controllers.DTO;
using Fitter_API.Controllers.Exceptions;
using Fitter_API.Controllers.Repository;
using Fitter_API.Controllers.RequestModels;
using Fitter_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fitter_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeniorFitterController : ControllerBase
    {
        private readonly ISeniorFitterRepository seniorFitterRepository;

        public SeniorFitterController(ISeniorFitterRepository seniorFitterRepository)
        {
            this.seniorFitterRepository = seniorFitterRepository;
        }

        [HttpGet(Name = "AllSeniorFitters")]
        [ProducesResponseType(typeof(IEnumerable<SeniorFitterControllerDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get()
        {
            var seniorFitters = await seniorFitterRepository.GetAllSeniorFitters();


            return Ok(seniorFitters);
        }

        [HttpGet("{id:int}", Name = "GetSeniorFitter")]
        [ProducesResponseType(typeof(SeniorFitterControllerDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetSenior(int id)
        {
            var seniorFitter = await seniorFitterRepository.GetSeniorById(id) ?? throw new NotFoundException($"Could not find Senior Fitter with Id {id}");

            return Ok(seniorFitter);
        }

        [HttpPost(Name = "PostSeniorFitter")]
        [ProducesResponseType(typeof(SeniorFitter), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Post([FromBody] PostSeniorController senior)
        {
            SeniorFitter seniorFitter = new();
            seniorFitter.Name = senior.Name;
            seniorFitter.Phone = senior.Phone;

            var existing = await seniorFitterRepository.FindSeniorFitterByPhone(seniorFitter);

            if (existing != null)
                return Forbid();

            var existingFitter = await seniorFitterRepository.GetFitterFromList(senior);
            var fitterNotInDB = senior.FitterId.Except(existingFitter.Select(p => p.Id));

            if (fitterNotInDB.Any())
                return NotFound(fitterNotInDB);

            seniorFitter.Fitters = existingFitter.ToList();
            await seniorFitterRepository.AddSeniorFitterToTable(seniorFitter);

            await seniorFitterRepository.SeniorFitterSaveChanges();

            return Ok(seniorFitter);
        }



        [HttpPut(Name = "UpdateSeniorFitter")]
        [ProducesResponseType(typeof(SeniorFitter), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateSenior([FromBody] UpdateSeniorController seniorFitter)
        {
            var existing = await seniorFitterRepository.FindSeniorFitterByIdIncludedFitter(seniorFitter.Id) ?? throw new NotFoundException($"Could not find Senior Fitter with Id {seniorFitter.Id}");
            if (existing == null)
                return NotFound();


            if (existing.Phone != seniorFitter.Phone)
            {
                SeniorFitter seniorPhoneCheck = new();
                seniorPhoneCheck.Phone = seniorFitter.Phone;
                var samePhone = await seniorFitterRepository.FindSeniorFitterByPhone(seniorPhoneCheck);
                if (samePhone != null)
                    return Forbid();
            }



            var existingFitter = await seniorFitterRepository.GetFitterFromListUpdate(seniorFitter);
            var fitterNotInDB = seniorFitter.FitterId.Except(existingFitter.Select(p => p.Id));
            if (fitterNotInDB.Any())
                return NotFound(fitterNotInDB);

            existing.Name = seniorFitter.Name;
            existing.Phone = seniorFitter.Phone;
            existing.Fitters = existingFitter.ToList();


            seniorFitterRepository.UpdateSeniorTable(existing);
            await seniorFitterRepository.SeniorFitterSaveChanges();

            return Ok(existing);
        }

        [HttpDelete("{id:int}", Name = "DeleteSeniorFitter")]
        [ProducesResponseType(typeof(SeniorFitter), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSenior(int id)
        {
            var seniorFitter = await seniorFitterRepository.FindSeniorFitterById(id) ?? throw new NotFoundException($"Could not find Senior Fitter with Id {id}");

            seniorFitterRepository.RemoveSeniorFitter(seniorFitter);
            await seniorFitterRepository.SeniorFitterSaveChanges();

            return Ok(seniorFitter);
        }
    }
}
