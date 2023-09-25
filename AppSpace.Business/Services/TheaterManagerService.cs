using AppSpace.Business.Dto;
using AppSpace.Business.Dto.Billboard;
using AppSpace.Business.Dto.Movies;
using AppSpace.Business.Interfaces;
using AppSpace.Domain.Context;
using AppSpace.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Services
{
    public class TheaterManagerService : ITheaterManager
    {
        private readonly AppDbContext _context;
        private HttpClient client;

        /// <summary>
        /// TheaterManagerService constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="factory"></param>
        public TheaterManagerService(AppDbContext context, IHttpClientFactory factory)
        {
            _context = context;
            client = factory.CreateClient("movieDbClient");
        }

        /// <summary>
        /// Gets upcoming api movies, then match the coincidences by name because ids dont match
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        public async Task<List<MovieRecommendationResultDto>> GetUpcomingMovieRecommendation(TheaterManagerRecommendationRequest movieRecommendationRequest)
        {
            List<MovieRecommendationResultDto> recommendations = new List<MovieRecommendationResultDto>();
            try
            {
                recommendations = await GetMovieRecommendation("movie/upcoming", movieRecommendationRequest);
                //Period filter
                recommendations = recommendations.Where(rec => movieRecommendationRequest.YearPeriod != 0 ? DateTime.Parse(rec.release_date).Year - DateTime.Now.Year <= movieRecommendationRequest.YearPeriod : true).ToList();
                return recommendations;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets succesful db movies and upcoming api movies, then match the coincidences by name because ids dont match
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        public async Task<List<MovieRecommendationResultDto>> GetUpcomingSuccesfulMovieRecommendation(TheaterManagerRecommendationRequest movieRecommendationRequest)
        {
            List<MovieRecommendationResultDto> recommendations = new List<MovieRecommendationResultDto>();
            try
            {
                var succesfulMovieList = await GetSuccesfulMovieList();
                List<string> succesfulMovieNameList = succesfulMovieList.Select(movie=> movie.OriginalTitle).ToList();

                //var recommendations2 = await GetMovieRecommendation("movie/upcoming", movieRecommendationRequest).Result;

                recommendations = await GetMovieRecommendation("movie/upcoming", movieRecommendationRequest);
                //Period and succesful movie filter
                recommendations = recommendations.Where(rec => (movieRecommendationRequest.YearPeriod != 0 ? DateTime.Parse(rec.release_date).Year - DateTime.Now.Year <= movieRecommendationRequest.YearPeriod : true)
                                                        && (succesfulMovieNameList.Contains(rec.title))).ToList();
                return recommendations;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets succesful db movies and upcoming api movies, then match the coincidences by name because ids dont match
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        public async Task<List<BillboardResultsPerDayDto>> GetBillboardMovieRecommendation(BillBoardRequestDto movieRecommendationRequest)
        {
            List<BillboardResultsPerDayDto> recommendations = new List<BillboardResultsPerDayDto>();
            BillboardResultsPerDayDto recommendation = new BillboardResultsPerDayDto();
            int weekDays = 7;
            int j = 0;
            try
            {
                Dictionary<int, int> genrePopularity = new Dictionary<int, int>();
                List<MovieGenre> movieGenreList = _context.MovieGenres.ToList();
                List<Movie> movieList = _context.Movies.ToList();
                List<Genre> genreList = _context.Genres.ToList();

                List<Room> bigRoomList = _context.Rooms.Where(room => room.Size.Equals("Big")).ToList();
                List<Room> smallRoomList = _context.Rooms.Where(room => room.Size.Equals("Small")).ToList();
                Random random = new Random();

                List<int> movieGenreIdGenreList = movieGenreList.Select(mvgnr => mvgnr.GenreId).Distinct().ToList();

                foreach ( var genreId in movieGenreIdGenreList)
                {
                     var popularity = movieGenreList.Where(movieGenre => movieGenre.GenreId == genreId).Count();
                    genrePopularity.Add(genreId, popularity);
                }

                var blockbusterGenreIdList = genrePopularity.OrderByDescending(genrepop=> genrepop.Value).Take(genrePopularity.Count() / 2).Select(genrepop=> genrepop.Key).ToList();
                var minorityGenreIdList = genrePopularity.OrderByDescending(genrepop => genrepop.Value).TakeLast(genrePopularity.Count() / 2).Select(genrepop => genrepop.Key).ToList();
                var movieIdBlockbusterList = movieGenreList.Where(movie => blockbusterGenreIdList.Contains(movie.GenreId)).Select(movie => movie.MovieId).Distinct().ToList();
                var movieIdMinorityGenreList = movieGenreList.Where(movie => minorityGenreIdList.Contains(movie.GenreId)).Select(movie => movie.MovieId).Distinct().ToList();

                for (int week = 1; week<=movieRecommendationRequest.WeeksPeriod; week++)
                {
                    for (int day = 1; day <= weekDays; day++)
                    {
                        recommendation = new BillboardResultsPerDayDto();
                        recommendation.WeekDay = day;
                        recommendation.Week = week;
                        //BigRooms
                        foreach (var room in bigRoomList)
                        {
                            for (int i = 1; i <= movieRecommendationRequest.ScreensBigRoom; i++)
                            {
                                //In case there are more screens than films cases
                                if (j >= movieIdBlockbusterList.Count())
                                    j = random.Next(0, movieIdBlockbusterList.Count());

                                var movieId = movieIdBlockbusterList[j];
                                var movieName = movieList.Where(movie => movie.Id == movieId).FirstOrDefault()?.OriginalTitle;

                                var genreIdList = movieGenreList.Where(mvgnr => mvgnr.MovieId == movieId).Select(mvgnr => mvgnr.GenreId).ToList();
                                var genreNameList = genreList.Where(genre => genreIdList.Contains(genre.Id)).Select(genre=> genre.Name).ToList();
                                recommendation.BigRoomList.Add(new BillboardResultDto()
                                {
                                    Movie = movieName,
                                    Genres = genreNameList,
                                    RoomId = room.Id,
                                    Screen = i
                                });
                                j++;
                            }
                        }
                        //SmallRooms
                        foreach (var room in smallRoomList)
                        {
                            for (int i = 1; i <= movieRecommendationRequest.ScreensSmallRoom; i++)
                            {
                                //In case there are more screens than films cases
                                if (j >= movieIdMinorityGenreList.Count())
                                    j = random.Next(0, movieIdMinorityGenreList.Count());

                                var movieId = movieIdMinorityGenreList[j];
                                var movieName = movieList.Where(movie => movie.Id == movieId).FirstOrDefault()?.OriginalTitle;

                                var genreIdList = movieGenreList.Where(mvgnr => mvgnr.MovieId == movieId).Select(mvgnr => mvgnr.GenreId).ToList();
                                var genreNameList = genreList.Where(genre => genreIdList.Contains(genre.Id)).Select(genre => genre.Name).ToList();
                                recommendation.SmallRoomList.Add(new BillboardResultDto()
                                {
                                    Movie = movieName,
                                    Genres = genreNameList,
                                    RoomId = room.Id,
                                    Screen = i
                                });

                                j++;
                            }
                        }
                        recommendations.Add(recommendation);
                    }
                    
                }
                return recommendations;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets movie recommendation
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        private async Task<List<MovieRecommendationResultDto>> GetMovieRecommendation(string endpoint, TheaterManagerRecommendationRequest movieRecommendationRequest)
        {
            try
            {
                HttpResponseMessage response;
                List<MovieRecommendationResultDto> recommendations = new List<MovieRecommendationResultDto>();
                List<int> desiredGenresIdList = new List<int>();

                //Request
                response = await client.GetAsync(endpoint);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<MovieRecommendationRootDto>(json);

                if (result != null)
                {
                    //AdditionalRequests
                    if (movieRecommendationRequest.Genre != null)
                    {
                        desiredGenresIdList = await GetMovieGenreId(movieRecommendationRequest);
                    }
                    
                    //Filters
                    result.results = result.results.Where(rec =>
                                    (desiredGenresIdList.Count > 0 ? rec.genre_ids.Intersect(desiredGenresIdList).Any() : true) &&           //Filter 1: Genre
                                    rec.adult == movieRecommendationRequest.Adult).ToList();                                                 //Filter 2: Age rate

                    //Mapping
                    foreach (var obj in result.results)
                    {
                        MovieRecommendationResultDto recomendation = new MovieRecommendationResultDto()
                        {
                            id = obj.id,
                            genre_ids = obj.genre_ids,
                            original_language = obj.original_language,
                            overview = obj.overview,
                            release_date = obj.release_date,
                            title = obj.title,
                            key_words = obj.key_words,
                            web_site = null
                        };
                        recommendations.Add(recomendation);
                    }
                }

                return recommendations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets each genre id by the given genre names.
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        private async Task<List<int>> GetMovieGenreId(TheaterManagerRecommendationRequest movieRecommendationRequest)
        {
            List<int> desiredGenresIds = new List<int>();

            HttpResponseMessage response = await client.GetAsync($"genre/movie/list");
            var content = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<GenresRootDto>(content);

            if (resp != null && resp.Genres != null)
            {

                desiredGenresIds = resp.Genres.Where(genre => movieRecommendationRequest.Genre.ToLower().Equals(genre.Name.ToLower()))
                                                .Select(genre => genre.Id).ToList();
            }

            return desiredGenresIds;
        }

        /// <summary>
        /// Gets each genre id by the given genre names.
        /// </summary>
        /// <returns></returns>
        private async Task<List<Movie>> GetSuccesfulMovieList()
        {
            Dictionary<int, int> soldSeatsByMovie = new Dictionary<int, int>();
            List<int> popularMovieIdList = new List<int>();

            List<Movie> movieList = _context.Movies.ToList();


            foreach (var movie in movieList)
            {
                int movieId = movie.Id;
                soldSeatsByMovie.Add(movieId, await GetSoldSeatsByMovieId(movieId));
            }

            popularMovieIdList = soldSeatsByMovie.OrderByDescending(soldseats => soldseats.Value).Take(10).Select(soldseats => soldseats.Key).ToList();
            movieList = movieList.Where(movie => popularMovieIdList.Contains(movie.Id)).ToList();

            return movieList;
        }

        /// <summary>
        /// Gets each genre id by the given genre names.
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        private async Task<int> GetSoldSeatsByMovieId(int movieId)
        {
            int? seatsSold = 0;
            var movieSessions = _context.Sessions.Where(session => session.MovieId == movieId).ToList();

            foreach ( var session in movieSessions)
            {
                seatsSold += session.SeatsSold;
            }

            return (int) seatsSold;
        }

    }
}
