using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSpace.Business.Dto
{
    public class GenresRootDto
    {
        [JsonProperty("genres")]
        public List<GenreDto> Genres { get; set; } = null!;
    }
}
