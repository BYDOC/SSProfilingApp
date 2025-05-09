using SSProfilingApp.Application.Interfaces;
using SSProfilingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSProfilingApp.Infrastructure.Services
{
    public class SimilarityScoreService : ISimilarityScoreService
    {
        private readonly ISimilarityCalculatorFactory _calculatorFactory;
        public SimilarityScoreService(ISimilarityCalculatorFactory calculatorFactory)
        {
            _calculatorFactory = calculatorFactory;
        }
        public async Task<double> CalculateScoreAsync(IndividualData a, IndividualData b)
        {
            if (!string.IsNullOrWhiteSpace(a.IdentityNumber) &&
                a.IdentityNumber == b.IdentityNumber)
            {
                return 1.0;
            }

            var calc = _calculatorFactory.Get(Application.Enums.SimilarityAlgorithm.JaroWinkler); //or Levenshtein

            var nameA = $"{a.FirstName}{a.MiddleName}{a.LastName}";
            var nameB = $"{b.FirstName}{b.MiddleName}{b.LastName}";
            var scoreName = await calc.CalculateAsync(nameA, nameB);

            var scorePlace = await calc.CalculateAsync(a.BirthPlace, b.BirthPlace);
            var scoreDate = await calc.CalculateAsync(a.BirthDate, b.BirthDate);
            var scoreNationality = a.Nationality == b.Nationality ? 1.0 : 0.0;

            var weighted =
                (scoreName * 0.50) +
                (scorePlace * 0.20) +
                (scoreDate * 0.15) +
                (scoreNationality * 0.15);

            return weighted;
        }
    }
}
