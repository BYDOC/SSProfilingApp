using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SSProfilingApp.Application.Interfaces;
using SSProfilingApp.Application.Requests;
using SSProfilingApp.Domain.Entities;
using SSProfilingApp.Infrastructure.Data;

namespace SSProfilingApp.Infrastructure.Services
{
    public class ProfilingService : IProfilingService
    {

        private readonly AppDbContext _db;
        private readonly ISimilarityCalculatorFactory _similarityFactory;
        private readonly ISimilarityScoreService _scoreService;
        public ProfilingService(AppDbContext db, ISimilarityCalculatorFactory similarityFactory, ISimilarityScoreService scoreService)
        {
            _db = db;
            _similarityFactory = similarityFactory;
            _scoreService = scoreService;
        }

        public async Task<List<int>> AddIndividualAsync(List<CreateIndividualRequest> request)
        {
            var individuals = request.Select(request => new IndividualData
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                BirthPlace = request.BirthPlace,
                BirthDate = request.BirthDate,
                Nationality = request.Nationality,
                IdentityNumber = request.IdentityNumber
            }).ToList();

            _db.Individuals.AddRange(individuals);
            await _db.SaveChangesAsync();

            return individuals.Select(i => i.Id).ToList();
        }

        public async Task GroupIndividualsAsync()
        {
            _db.DataProfiles.RemoveRange(_db.DataProfiles);
            await _db.SaveChangesAsync();

            var individuals = await _db.Individuals.ToListAsync();
            var grouped = new List<(IndividualData individual, int profileId)>();
            int nextProfileId = 1;

            foreach (var current in individuals)
            {
                int? matchedProfileId = null;

                foreach (var (existing, profileId) in grouped)
                {
                    double score = await _scoreService.CalculateScoreAsync(current, existing);
                    if (score >= 0.85)
                    {
                        matchedProfileId = profileId;
                        break;
                    }
                }

                if (matchedProfileId == null)
                {
                    matchedProfileId = nextProfileId++;
                }

                _db.DataProfiles.Add(new DataProfile
                {
                    ProfileId = matchedProfileId.Value,
                    IndividualDataId = current.Id
                });

                grouped.Add((current, matchedProfileId.Value));
            }

            await _db.SaveChangesAsync();
        }

        private async Task<int> GetNextProfileIdAsync()
        {
            return (await _db.DataProfiles.MaxAsync(p => (int?)p.ProfileId) ?? 0) + 1;
        }

        public async Task DeleteAllAsync()
        {
            var dataProfiles = await _db.DataProfiles.ToListAsync();
            _db.DataProfiles.RemoveRange(dataProfiles);

            var individuals = await _db.Individuals.ToListAsync();
            _db.Individuals.RemoveRange(individuals);
#if DEBUG
            await _db.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.Individuals', RESEED, 0)");
            await _db.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('dbo.DataProfiles', RESEED, 0)");
#endif
            await _db.SaveChangesAsync();
        }
    }
}
