using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Integration.Meeting.Zoom.Dto.Response
{
    public class SubAccountResponseDto
    {
        [JsonProperty("id")]
        public int AccountId { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("owner_email")]
        public int OwnerEmail { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreationDate { get; set; }
    }
}
