using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.TvShow
{
    public class TvRecommendationResultDto
    {
        public List<int> genre_ids { get; set; }
        public int id { get; set; }
        public string original_language { get; set; }
        public string overview { get; set; }
        public string title { get; set; }
        public List<string> key_words { get; set; }
        public string web_site { get; set; }

        //Tv
        public string first_air_date { get; set; }
        public int number_of_seasons { get; set; }
        public int number_of_episodes { get; set; }
        public string status { get; set; }
        public bool concluded { get; set; }
    }
}
