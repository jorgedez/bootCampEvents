using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bootCamp.Shared.Entities
{
    public class HotelsJson
    {
        [JsonProperty("hotels")]
        public Hotel[] Hotel { get; set; }
    }
}
