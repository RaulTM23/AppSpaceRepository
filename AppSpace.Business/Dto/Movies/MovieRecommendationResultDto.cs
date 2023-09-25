using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.Movies
{
    public class MovieRecommendationResultDto
    {
        public List<int> genre_ids { get; set; }
        public int id { get; set; }
        public string original_language { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public List<string> key_words { get; set; }
        public string web_site { get; set; }
    }
}
