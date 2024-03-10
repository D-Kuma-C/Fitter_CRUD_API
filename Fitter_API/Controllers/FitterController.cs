using Fitter_API.Controllers.DTO;
using Fitter_API.Controllers.Exceptions;
using Fitter_API.Controllers.Repository;
using Fitter_API.Controllers.RequestModels;
using Fitter_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitter_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FitterController : ControllerBase
    {
        private readonly IFitterRepository fitterRepository;

        public FitterController(IFitterRepository fitterRepository)
        {
            this.fitterRepository = fitterRepository;
        }

        [HttpGet(Name = "AllFitters")]
        [ProducesResponseType(typeof(IEnumerable<FitterControllerDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Get()
        {
            var fitters = await fitterRepository.GetAllFittersAsFitterControllerDTO();

            return Ok(fitters);
        }

        [HttpGet("{id:int}", Name = "GetFitter")]
        [ProducesResponseType(typeof(FitterControllerDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetFitter(int id)
        {
            var fitter = await fitterRepository.GetFittersById(id) ?? throw new NotFoundException($"Could not find Fitter with Id {id}");

            return Ok(fitter);
        }

        [HttpPost(Name = "PostFitter")]
        [ProducesResponseType(typeof(Fitter), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Post([FromBody] PostFitterController postSeniorController)
        {
            Fitter fitter = new();
            fitter.Name = postSeniorController.Name;
            fitter.Phone = postSeniorController.Phone;
            var existing = await fitterRepository.GetFitterByPhone(fitter);
            if (existing != null)
                return Forbid();

            bool isSeniorIdThere = postSeniorController.SeniorId.Count() > 0;
            if (!isSeniorIdThere)
                return Forbid();

            var existingSenior = await fitterRepository.GetSeniorFitterFromList(postSeniorController);
            var kagefitter = postSeniorController.SeniorId.Except(existingSenior.Select(p => p.Id));
            if (kagefitter.Any())
                return NotFound(kagefitter);

            fitter.SeniorFitters = existingSenior.ToList();
            await fitterRepository.AddToFittersTable(fitter);
            await fitterRepository.SaveDbChanges();

            return Ok(fitter);
        }


        [HttpPut(Name = "UpdateFitter")]
        [ProducesResponseType(typeof(Fitter), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateFitter([FromBody] UpdateFitterController updateFitterController)
        {
            var existing = await fitterRepository.FindFitterByIdIncludedSenior(updateFitterController.Id) ?? throw new NotFoundException($"Could not find Fitter with Id {updateFitterController.Id}");


            bool haveNotChangedPhone = existing.Phone == updateFitterController.Phone;
            if (!haveNotChangedPhone)
            {
                Fitter phoneCheck = new();
                phoneCheck.Phone = updateFitterController.Phone;
                var existingPhone = await fitterRepository.GetFitterByPhone(phoneCheck);
                if (existingPhone != null)
                    return Forbid();
            }

            var seniorId = updateFitterController.SeniorId.Count() > 0;
            if (!seniorId)
                return Forbid();

            var existingSenior = await fitterRepository.GetSeniorFitterFromListUpdate(updateFitterController);
            var kagefitter = updateFitterController.SeniorId.Except(existingSenior.Select(p => p.Id));
            if (kagefitter.Any())
                return NotFound(kagefitter);


            existing.Name = updateFitterController.Name;
            existing.Phone = updateFitterController.Phone;
            existing.SeniorFitters = existingSenior.ToList();
            fitterRepository.UpdateFitterTable(existing);
            await fitterRepository.SaveDbChanges();

            return Ok(existing);
        }



        [HttpDelete("{id:int}", Name = "DeleteFitter")]
        [ProducesResponseType(typeof(Fitter), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteFitter(int id)
        {
            Fitter? existing = await fitterRepository.FindFitterById(id) ?? throw new NotFoundException($"Could not find Fitter with Id {id}");

            fitterRepository.RemoveFitter(existing);
            await fitterRepository.SaveDbChanges();

            return Ok(existing);
        }
    }
}
