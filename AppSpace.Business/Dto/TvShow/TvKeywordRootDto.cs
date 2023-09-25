using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto.TvShow
{
    public class TvKeywordRootDto
    {
        public int id { get; set; }
        public List<KeywordDto> results { get; set; }
    }
}
