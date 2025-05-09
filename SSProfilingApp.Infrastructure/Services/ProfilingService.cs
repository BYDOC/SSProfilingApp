using Microsoft.EntityFrameworkCore;
using SSProfilingApp.Application.Enums;
using SSProfilingApp.Application.Interfaces;
using SSProfilingApp.Application.Requests;
using SSProfilingApp.Domain.Entities;
using SSProfilingApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> AddIndividualAsync(CreateIndividualRequest request)
        {
            var newIndividual = new IndividualData
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                BirthPlace = request.BirthPlace,
                BirthDate = request.BirthDate,
                Nationality = request.Nationality,
                IdentityNumber = request.IdentityNumber
            };

            _db.Individuals.Add(newIndividual);
            await _db.SaveChangesAsync();

            return newIndividual.Id;
        }

        public async Task GroupIndividualsAsync()
        {
            var ungrouped = await _db.Individuals
                .Where(i => !_db.DataProfiles.Any(p => p.IndividualDataId == i.Id))
                .ToListAsync();

            var grouped = await _db.Individuals
                .Where(i => _db.DataProfiles.Any(p => p.IndividualDataId == i.Id))
                .ToListAsync();

            foreach (var newIndividual in ungrouped)
            {
                int? matchedProfileId = null;

                foreach (var existing in grouped)
                {
                    double score = await _scoreService.CalculateScoreAsync(newIndividual, existing);
                    if (score >= 0.85)
                    {
                        matchedProfileId = await _db.DataProfiles
                            .Where(p => p.IndividualDataId == existing.Id)
                            .Select(p => (int?)p.ProfileId)
                            .FirstOrDefaultAsync();
                        break;
                    }
                }

                if (matchedProfileId == null)
                {
                    matchedProfileId = await GetNextProfileIdAsync();
                }

                _db.DataProfiles.Add(new DataProfile
                {
                    ProfileId = matchedProfileId.Value,
                    IndividualDataId = newIndividual.Id
                });
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

            await _db.SaveChangesAsync();
        }
    }
}
