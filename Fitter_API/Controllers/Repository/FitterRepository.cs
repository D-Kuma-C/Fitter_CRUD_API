using Fitter_API.Controllers.DTO;
using Fitter_API.Controllers.RequestModels;
using Fitter_API.Data;
using Fitter_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitter_API.Controllers.Repository
{
    public interface IFitterRepository
    {
        public Task<IEnumerable<FitterControllerDTO>> GetAllFittersAsFitterControllerDTO();
        public Task<FitterControllerDTO?> GetFittersById(int id);
        public Task<Fitter?> GetFitterByPhone(Fitter fitter);
        public Task<IEnumerable<SeniorFitter>> GetSeniorFitterFromList(PostFitterController postSeniorController);
        public Task<IEnumerable<SeniorFitter>> GetSeniorFitterFromListUpdate(UpdateFitterController UpdateFitterController);
        public Task AddToFittersTable(Fitter fitter);
        public Task SaveDbChanges();
        public Task<Fitter?> FindFitterById(int id);
        public void RemoveFitter(Fitter existing);
        public void UpdateFitterTable(Fitter existingfitter);
        public Task<Fitter?> FindFitterByIdIncludedSenior(int id);
    }
    public class FitterRepository : IFitterRepository
    {
        private readonly FitterDbContext context;

        public FitterRepository(FitterDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<FitterControllerDTO>> GetAllFittersAsFitterControllerDTO()
        {
            var fitters = await context.Fitters.Include(f => f.SeniorFitters)
                .Select(f => new FitterControllerDTO() { Id = f.Id, Name = f.Name, Phone = f.Phone, SeniorFitters = f.SeniorFitters.Select(e => new SeniorFitterDTO() { Id = e.Id, Name = e.Name, Phone = e.Phone }).ToList() })
                .ToListAsync();

            return fitters;
        }

        public async Task<FitterControllerDTO?> GetFittersById(int id)
        {
            var fitters = await context.Fitters.Include(f => f.SeniorFitters)
                .Select(f => new FitterControllerDTO() { Id = f.Id, Name = f.Name, Phone = f.Phone, SeniorFitters = f.SeniorFitters.Select(e => new SeniorFitterDTO() { Id = e.Id, Name = e.Name, Phone = e.Phone }).ToList() })
                .FirstOrDefaultAsync(i => i.Id == id);

            return fitters;
        }

        public async Task<Fitter?> GetFitterByPhone(Fitter fitter)
        {
            return await context.Fitters.FirstOrDefaultAsync(p => p.Phone == fitter.Phone);
        }

        public async Task<IEnumerable<SeniorFitter>> GetSeniorFitterFromList(PostFitterController postFitterController)
        {
            return await context.SeniorFitters.Where(p => postFitterController.SeniorId.Contains(p.Id)).ToListAsync();
        }

        public async Task<IEnumerable<SeniorFitter>> GetSeniorFitterFromListUpdate(UpdateFitterController UpdateFitterController)
        {
            return await context.SeniorFitters.Where(p => UpdateFitterController.SeniorId.Contains(p.Id)).ToListAsync();
        }

        public async Task AddToFittersTable(Fitter fitter)
        {
            await context.Fitters.AddAsync(fitter);
        }

        public async Task SaveDbChanges()
        {
            await context.SaveChangesAsync();
        }

        public async Task<Fitter?> FindFitterById(int id)
        {
            return await context.Fitters.FindAsync(id);
        }

        public async Task<Fitter?> FindFitterByIdIncludedSenior(int id)
        {
            return await context.Fitters.Include(p => p.SeniorFitters).FirstOrDefaultAsync(p => p.Id == id);
        }

        public void UpdateFitterTable(Fitter existingfitter)
        {
            context.Fitters.Update(existingfitter);
        }

        public void RemoveFitter(Fitter existing)
        {
            context.Fitters.Remove(existing);
        }
    }
}
