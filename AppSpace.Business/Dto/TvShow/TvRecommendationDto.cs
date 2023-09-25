using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.TvShow
{
    public class TvRecommendationDto
    {
        public string backdrop_path { get; set; }
        public string first_air_date { get; set; }
        public List<int> genre_ids { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public List<string> origin_country { get; set; }
        public string original_language { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }
}
