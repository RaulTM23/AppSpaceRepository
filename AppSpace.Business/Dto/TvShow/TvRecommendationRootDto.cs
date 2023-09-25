using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.TvShow
{
    public class TvRecommendationRootDto
    {
        public int page { get; set; }
        public List<TvRecommendationDto> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
