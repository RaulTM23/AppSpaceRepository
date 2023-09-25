using AppSpace.Business.Dto;
using AppSpace.Business.Dto.Billboard;
using AppSpace.Business.Dto.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Interfaces
{
    public interface ITheaterManager
    {
        Task<List<MovieRecommendationResultDto>> GetUpcomingMovieRecommendation(TheaterManagerRecommendationRequest movieRecommendationRequest);
        Task<List<MovieRecommendationResultDto>> GetUpcomingSuccesfulMovieRecommendation(TheaterManagerRecommendationRequest movieRecommendationRequest);
        Task<List<BillboardResultsPerDayDto>> GetBillboardMovieRecommendation(BillBoardRequestDto movieRecommendationRequest);
    }
}
