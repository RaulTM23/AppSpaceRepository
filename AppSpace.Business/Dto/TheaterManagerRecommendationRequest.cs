using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto
{
    public class TheaterManagerRecommendationRequest
    {
        public string Genre { get; set; }
        public bool Adult { get; set; }
        public int YearPeriod { get; set; } = 0;
        public bool SuccesfulMovieSearch = false;
    }
}
