using Fitter_API.Controllers.DTO;
using Fitter_API.Controllers.RequestModels;
using Fitter_API.Data;
using Fitter_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitter_API.Controllers.Repository
{
    public interface ISeniorFitterRepository
    {
        public Task<IEnumerable<SeniorFitterControllerDTO>> GetAllSeniorFitters();
        public Task<SeniorFitterControllerDTO?> GetSeniorById(int id);
        public Task<SeniorFitter?> FindSeniorFitterById(int id);
        public void RemoveSeniorFitter(SeniorFitter seniorFitter);
        public Task SeniorFitterSaveChanges();
        public Task AddSeniorFitterToTable(SeniorFitter seniorFitter);
        public Task<SeniorFitter?> FindSeniorFitterByPhone(SeniorFitter senior);
        public Task<IEnumerable<Fitter>> GetFitterFromList(PostSeniorController postFitterController);
        public Task<IEnumerable<Fitter>> GetFitterFromListUpdate(UpdateSeniorController UpdateFitterController);
        public void UpdateSeniorTable(SeniorFitter existingfitter);
        public Task<SeniorFitter?> FindSeniorFitterByIdIncludedFitter(int id);
    }

    public class SeniorFitterRepository : ISeniorFitterRepository
    {
        private readonly FitterDbContext context;

        public SeniorFitterRepository(FitterDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SeniorFitterControllerDTO>> GetAllSeniorFitters()
        {
            var seniorFitters = await context.SeniorFitters.Include(e => e.Fitters)
                .Select(senior => new SeniorFitterControllerDTO() { Id = senior.Id, Name = senior.Name, Phone = senior.Phone, Fitters = senior.Fitters.Select(fitter => new FitterDTO() { Id = fitter.Id, Name = fitter.Name, Phone = fitter.Phone }).ToList() })
                .ToListAsync();

            return seniorFitters;
        }

        public async Task<SeniorFitterControllerDTO?> GetSeniorById(int id)
        {
            var seniorFitter = await context.SeniorFitters.Include(e => e.Fitters)
                .Select(senior => new SeniorFitterControllerDTO() { Id = senior.Id, Name = senior.Name, Phone = senior.Phone, Fitters = senior.Fitters.Select(fitter => new FitterDTO() { Id = fitter.Id, Name = fitter.Name, Phone = fitter.Phone }).ToList() })
                .FirstOrDefaultAsync(u => u.Id == id);

            return seniorFitter;
        }

        public async Task<SeniorFitter?> FindSeniorFitterById(int id)
        {
            return await context.SeniorFitters.FindAsync(id);
        }

        public async Task<SeniorFitter?> FindSeniorFitterByIdIncludedFitter(int id)
        {
            return await context.SeniorFitters.Include(p => p.Fitters).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<SeniorFitter?> FindSeniorFitterByPhone(SeniorFitter senior)
        {
            return await context.SeniorFitters.FirstOrDefaultAsync(p => p.Phone == senior.Phone);
        }

        public async Task<IEnumerable<Fitter>> GetFitterFromList(PostSeniorController postFitterController)
        {
            return await context.Fitters.Where(p => postFitterController.FitterId.Contains(p.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Fitter>> GetFitterFromListUpdate(UpdateSeniorController UpdateFitterController)
        {
            return await context.Fitters.Where(p => UpdateFitterController.FitterId.Contains(p.Id)).ToListAsync();
        }

        public async Task AddSeniorFitterToTable(SeniorFitter seniorFitter)
        {
            await context.SeniorFitters.AddAsync(seniorFitter);
        }

        public void UpdateSeniorTable(SeniorFitter existingfitter)
        {
            context.SeniorFitters.Update(existingfitter);
        }

        public void RemoveSeniorFitter(SeniorFitter seniorFitter)
        {
            context.SeniorFitters.Remove(seniorFitter);
        }

        public async Task SeniorFitterSaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}
