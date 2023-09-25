using AppSpace.Business.Dto;
using AppSpace.Business.Dto.Movies;
using AppSpace.Business.Dto.TvShow;
using AppSpace.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Interfaces
{
    public interface IViewer
    {
        Task<List<MovieRecommendationResultDto>> GetAllTimeMovieRecommendation(ViewerRecommendationRequestDto movieRecommendationRequest);
        Task<List<MovieRecommendationResultDto>> GetUpcomingMovieRecommendation(ViewerRecommendationRequestDto movieRecommendationRequest); 
        Task<List<TvRecommendationResultDto>> GetAllTimeTvRecommendation(ViewerRecommendationRequestDto movieRecommendationRequest);
        Task <MovieTvResultDto> GetDocumentaryRecommendation(ViewerRecommendationRequestDto movieRecommendationRequest);

    }
}
