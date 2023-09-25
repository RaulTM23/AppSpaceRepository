using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto
{
    public class ViewerRecommendationRequestDto
    {
        public List<string> KeyWords { get; set; }
        public List<string> Genres { get; set; }
        public int YearsPeriod { get; set; } = 0;
    }
}
