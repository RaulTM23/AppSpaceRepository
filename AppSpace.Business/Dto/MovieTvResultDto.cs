using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSpace.Business.Dto.Movies;
using AppSpace.Business.Dto.TvShow;

namespace AppSpace.Business.Dto
{
    public class MovieTvResultDto
    {
        public List<MovieRecommendationResultDto> MovieRecommendations { get; set; }
        public List<TvRecommendationResultDto> TvRecommendations { get; set; }
    }
}
