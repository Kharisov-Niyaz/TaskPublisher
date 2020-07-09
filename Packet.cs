using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace TaskPublisher
{
    class Packet
    {
        public int id { get; set; }
        public string message { get; set; }
        public DateTime sendDate { get; set; }
        public string hash { get; set; }

        [JsonIgnore]
        public bool sended { get; set; }
    }
}
