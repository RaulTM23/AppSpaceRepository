using AppSpace.Business.Dto;
using AppSpace.Business.Dto.Billboard;
using AppSpace.Business.Dto.Movies;
using AppSpace.Business.Interfaces;
using AppSpace.Business.Services;
using AppSpace.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppSpace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterManagerController : ControllerBase
    {
        private readonly ITheaterManager _theaterManagerService;

        public TheaterManagerController(ITheaterManager theaterManagerService)
        {
            _theaterManagerService = theaterManagerService;
        }

        // GET: api/GetUpcomingMovieRecommendation
        [HttpGet("GetUpcomingMovieRecommendation")]
        public async Task<ActionResult<List<MovieRecommendationResultDto>>> GetUpcomingMovieRecommendation()
        {
            List<MovieRecommendationResultDto> recommendationList = new List<MovieRecommendationResultDto>();
            TheaterManagerRecommendationRequest recommendationRequest = new TheaterManagerRecommendationRequest()
            {
                Adult = false,
                Genre = "Action",
                YearPeriod = 2,
                SuccesfulMovieSearch = false
            };

            if (recommendationRequest.SuccesfulMovieSearch)
            {
                recommendationList = await _theaterManagerService.GetUpcomingSuccesfulMovieRecommendation(recommendationRequest);
            }
            else
            {
                recommendationList = await _theaterManagerService.GetUpcomingMovieRecommendation(recommendationRequest);
            }


            if (recommendationList is null)
                return BadRequest("There is no data for recommendations.");

            return Ok(recommendationList);
        }



        // GET: api/GetBillboardMovieRecommendation
        [HttpGet("GetBillboardMovieRecommendation")]
        public async Task<ActionResult<List<BillboardResultsPerDayDto>>> GetBillboardMovieRecommendation(int screensBigRoom, int screensSmallRoom, int weeksPeriod)
        {
            try
            {
                BillBoardRequestDto recommendationRequest = new BillBoardRequestDto()
                {
                    ScreensBigRoom = screensBigRoom,
                    ScreensSmallRoom = screensSmallRoom,
                    WeeksPeriod = weeksPeriod
                };

                List<BillboardResultsPerDayDto> recommendationList = await _theaterManagerService.GetBillboardMovieRecommendation(recommendationRequest);


                if (recommendationList is null)
                    return BadRequest("There is no data for recommendations.");

                return Ok(recommendationList);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred.");
            }

        }
    }
}
