using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.Movies
{
    public class MovieKeywordRootDto
    {
        public int id { get; set; }
        public List<KeywordDto> keywords { get; set; }
    }
}
