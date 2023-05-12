using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Notifications
{
    public class Notification
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("PersonJmbg")]
        public string PersonJmbg { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonConstructor]
        public Notification(string id,string personJmbg, string message)
        {
            Id = id;
            PersonJmbg = personJmbg;
            Message = message;
        }

    }
}
