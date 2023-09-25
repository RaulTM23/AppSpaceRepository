using AppSpace.Business.Dto;
using AppSpace.Business.Dto.Movies;
using AppSpace.Business.Interfaces;
using AppSpace.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppSpace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewerController : ControllerBase
    {
        private readonly IViewer _viewerService;

        public ViewerController(IViewer viewerService)
        {
            _viewerService = viewerService;
        }

        // GET: api/GetAllTimeMovieRecommendation
        [HttpGet("GetAllTimeMovieRecommendation")]
        public async Task<ActionResult<List<MovieRecommendationResultDto>>> GetAllTimeMovieRecommendation()
        {
            ViewerRecommendationRequestDto recommendationRequest = new ViewerRecommendationRequestDto()
            {
                Genres = new List<string>() { "Thriller", "War", "Documentary", "Tv movie" },
                KeyWords = new List<string> { "trauma" }
            };

            var viewers = await _viewerService.GetAllTimeMovieRecommendation(recommendationRequest);

            if (viewers is null)
                return BadRequest("There is no data for recommendations.");

            return Ok(viewers);
        }

        // GET: api/GetAllTimeTvRecommendation
        [HttpGet("GetAllTimeTvRecommendation")]
        public async Task<ActionResult<List<MovieRecommendationResultDto>>> GetAllTimeTvRecommendation()
        {
            ViewerRecommendationRequestDto recommendationRequest = new ViewerRecommendationRequestDto()
            {
                Genres = new List<string>() { "Crime", "Drama", "Documentary" },
                KeyWords = new List<string> { "trauma", "detective", "horse", "current affairs" }
            };

            var viewers = await _viewerService.GetAllTimeTvRecommendation(recommendationRequest);

            if (viewers is null)
                return BadRequest("There is no data for recommendations.");

            return Ok(viewers);
        }

        // GET: api/GetUpcomingMovieRecommendation
        [HttpGet("GetUpcomingMovieRecommendation")]
        public async Task<ActionResult<List<MovieRecommendationResultDto>>> GetUpcomingMovieRecommendation()
        {
            ViewerRecommendationRequestDto recommendationRequest = new ViewerRecommendationRequestDto()
            {
                Genres = new List<string>() { "Thriller", "War", "Documentary", "Tv movie" },
                KeyWords = new List<string> { "trauma", "shotgun" },
                YearsPeriod = 10

            };

            var viewers = await _viewerService.GetUpcomingMovieRecommendation(recommendationRequest);

            if (viewers is null)
                return BadRequest("There is no data for recommendations.");

            return Ok(viewers);
        }

        // GET: api/GetDocumentaryRecommendation
        [HttpGet("GetDocumentaryRecommendation")]
        public async Task<ActionResult<List<MovieRecommendationResultDto>>> GetDocumentaryRecommendation()
        {
            ViewerRecommendationRequestDto recommendationRequest = new ViewerRecommendationRequestDto()
            {
                Genres = new List<string>() { "Documentary", "War"}
            };

            var viewers = await _viewerService.GetDocumentaryRecommendation(recommendationRequest);

            if (viewers is null)
                return BadRequest("There is no data for recommendations.");

            return Ok(viewers);
        }

    }
}
