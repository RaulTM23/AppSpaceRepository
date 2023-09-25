using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.Movies
{ 
    public class MovieRecommendationRootDto
    {
        public int page { get; set; }
        public List<MovieRecommendationDto> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
