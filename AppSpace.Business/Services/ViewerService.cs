using AppSpace.Business.Dto;
using AppSpace.Business.Dto.Movies;
using AppSpace.Business.Dto.TvShow;
using AppSpace.Business.Interfaces;
using AppSpace.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AppSpace.Business.Services
{
    public class ViewerService : IViewer
    {
        private readonly AppDbContext _context;
        private HttpClient client;

        public ViewerService(AppDbContext context, IHttpClientFactory factory)
        {
            _context = context;
            client = factory.CreateClient("movieDbClient");
        }
        /// <summary>
        /// Gets all time movie recommendation
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        public async Task<List<MovieRecommendationResultDto>> GetAllTimeMovieRecommendation(ViewerRecommendationRequestDto movieRecommendationRequest)
        {
            List<MovieRecommendationResultDto> recommendations = new List<MovieRecommendationResultDto>();
            try
            {
                recommendations = await GetMovieRecommendation("discover/movie", movieRecommendationRequest);
                return recommendations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the upcoming movie recommendation
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        public async Task<List<MovieRecommendationResultDto>> GetUpcomingMovieRecommendation(ViewerRecommendationRequestDto movieRecommendationRequest)
        {
            List<MovieRecommendationResultDto> recommendations = new List<MovieRecommendationResultDto>();
            try
            {
                recommendations = await GetMovieRecommendation("movie/upcoming", movieRecommendationRequest);
                recommendations = recommendations.Where(rec => movieRecommendationRequest.YearsPeriod != 0 ? DateTime.Parse(rec.release_date).Year - DateTime.Now.Year <= movieRecommendationRequest.YearsPeriod : true).ToList();
                return recommendations;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Gets all time tv show recommendation
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        public async Task<List<TvRecommendationResultDto>> GetAllTimeTvRecommendation(ViewerRecommendationRequestDto movieRecommendationRequest)
        {
            List<TvRecommendationResultDto> recommendations = new List<TvRecommendationResultDto>();

            try
            {
                recommendations = await GetTvRecommendation("discover/tv", movieRecommendationRequest);
                return recommendations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets documentary recommendation. Movie or TvShow
        /// </summary>
        /// <param name="recommendationRequest"></param>
        /// <returns></returns>
        public async Task<MovieTvResultDto> GetDocumentaryRecommendation(ViewerRecommendationRequestDto recommendationRequest)
        {
            MovieTvResultDto recommendations = new MovieTvResultDto();

            try
            {
                var tvGenreIdList = await this.GetTvGenreIdList(recommendationRequest);
                var movieGenreIdList = await this.GetMovieGenreIdList(recommendationRequest);

                recommendations.TvRecommendations = await GetTvRecommendation($"discover/tv?with_genres={string.Join(",",tvGenreIdList)}", recommendationRequest);
                recommendations.MovieRecommendations = await GetMovieRecommendation($"discover/movie?with_genres={string.Join(",", movieGenreIdList)}", recommendationRequest);
                return recommendations;
            }
            catch (Exception ex)
            {   
                throw;
            }
        }

        #region privateMethods

        /// <summary>
        /// Gets movie recommendation
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        private async Task<List<MovieRecommendationResultDto>> GetMovieRecommendation(string endpoint, ViewerRecommendationRequestDto movieRecommendationRequest)
        {
            try
            {
                HttpResponseMessage response;
                List<MovieRecommendationResultDto> recommendationList = new List<MovieRecommendationResultDto>();
                List<int> desiredGenreIdList = new List<int>();
                List<string> desiredKeywordList = movieRecommendationRequest.KeyWords;

                //Request
                response = await client.GetAsync(endpoint);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<MovieRecommendationRootDto>(json);

                if (result != null)
                {
                    //AdditionalRequests
                    if (movieRecommendationRequest.Genres.Count() > 0)
                    {
                        desiredGenreIdList = await GetMovieGenreIdList(movieRecommendationRequest);
                    }

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
                            key_words = await this.GetMovieKeywordList(obj.id),
                            web_site = null
                        };
                        recommendationList.Add(recomendation);
                    }

                    //Filters
                    recommendationList = recommendationList.Where(rec => 
                                    (desiredGenreIdList != null ? rec.genre_ids.Intersect(desiredGenreIdList).Any() : true) &&                                     //Filter 1: Genre
                                    (desiredKeywordList != null ? rec.key_words.Intersect(desiredKeywordList, StringComparer.OrdinalIgnoreCase).Any() : true)      //Filter 2: Keyword
                                    ).ToList();
                }

                return recommendationList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets tv show recommendation
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        private async Task<List<TvRecommendationResultDto>> GetTvRecommendation(string endpoint, ViewerRecommendationRequestDto movieRecommendationRequest)
        {
            try
            {
                HttpResponseMessage response;
                List<TvRecommendationResultDto> recommendationList = new List<TvRecommendationResultDto>();
                List<string> desiredKeywords = movieRecommendationRequest.KeyWords;

                //Request
                response = await client.GetAsync(endpoint);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TvRecommendationRootDto>(json);

                if (result != null)
                {
                    //AdditionalRequests
                    List<int> desiredGenreIdList = await GetTvGenreIdList(movieRecommendationRequest);

                    //Mapping
                    foreach (var obj in result.results)
                    {
                        var tvShowDetails = await GetTvShowDetails(obj.id);
                        TvRecommendationResultDto recomendation = new TvRecommendationResultDto()
                        {
                            id = obj.id,
                            genre_ids = obj.genre_ids,
                            original_language = obj.original_language,
                            overview = obj.overview,
                            title = obj.original_name,
                            key_words = await this.GetTvKeywordList(obj.id),
                            web_site = null,
                            first_air_date = obj.first_air_date,
                            number_of_episodes = tvShowDetails.number_of_episodes,
                            number_of_seasons = tvShowDetails.number_of_seasons,
                            concluded = tvShowDetails.status.Equals("Ended")

                        };
                        recommendationList.Add(recomendation);
                    }

                    //Filter
                    recommendationList = recommendationList.Where(rec =>
                                    ((desiredGenreIdList != null) ? rec.genre_ids.Intersect(desiredGenreIdList).Any() : true) &&                             //Filter 1: Genre
                                    ((desiredKeywords != null)? rec.key_words.Intersect(desiredKeywords, StringComparer.OrdinalIgnoreCase).Any() : true)       //Filter 2: Keyword
                                    ).ToList();
                }

                return recommendationList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets keyword names for found movie.
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        private async Task<List<string>> GetMovieKeywordList(int movieId)
        {
            List<string> keywordList = new List<string>();

            HttpResponseMessage response = await client.GetAsync($"movie/{movieId}/keywords");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MovieKeywordRootDto>(content);

            if (result != null && result.keywords != null)
            {
                keywordList = result.keywords.Select(x => x.name).ToList();
            }

            return keywordList;
        }

        /// <summary>
        /// Gets keywords for found tv show.
        /// </summary>
        /// <param name="serieId"></param>
        /// <returns></returns>
        private async Task<List<string>> GetTvKeywordList(int serieId)
        {
            List<string> keywordList = new List<string>();

            HttpResponseMessage response = await client.GetAsync($"tv/{serieId}/keywords");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TvKeywordRootDto>(content); 

            if (result != null && result.results != null)
            {
                keywordList = result.results.Select(x => x.name).ToList();
            }

            return keywordList;
        }

        /// <summary>
        /// Gets each genre id by the given genre names.
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        private async Task<List<int>> GetMovieGenreIdList(ViewerRecommendationRequestDto movieRecommendationRequest)
        {
            List<int> desiredGenresIds = new List<int>();

            HttpResponseMessage response = await client.GetAsync($"genre/movie/list");
            var content = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<GenresRootDto>(content);

            if (resp != null && resp.Genres != null)
            {
                desiredGenresIds = resp.Genres.Where(genre => movieRecommendationRequest.Genres.Contains(genre.Name, StringComparer.OrdinalIgnoreCase))
                                                .Select(genre => genre.Id).ToList();
            }

            return desiredGenresIds;
        }

        /// <summary>
        /// Gets each genre id by the given genre names.
        /// </summary>
        /// <param name="movieRecommendationRequest"></param>
        /// <returns></returns>
        private async Task<List<int>> GetTvGenreIdList(ViewerRecommendationRequestDto movieRecommendationRequest)
        {
            List<int> desiredGenresIds = new List<int>();

            HttpResponseMessage response = await client.GetAsync($"genre/tv/list");
            var content = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<GenresRootDto>(content);

            if (resp != null && resp.Genres != null)
            {
                desiredGenresIds = resp.Genres.Where(genre => movieRecommendationRequest.Genres.Contains(genre.Name, StringComparer.OrdinalIgnoreCase))
                                                .Select(genre => genre.Id).ToList();
            }

            return desiredGenresIds;
        }

        /// <summary>
        /// Gets tv show details.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        private async Task<TvRecommendationResultDto> GetTvShowDetails(int seriesId)
        {
            HttpResponseMessage response = await client.GetAsync($"tv/{seriesId}");
            var content = await response.Content.ReadAsStringAsync();
            TvRecommendationResultDto tvShowDetails = JsonConvert.DeserializeObject<TvRecommendationResultDto>(content);

            return tvShowDetails;
        }
        #endregion

    }
}
