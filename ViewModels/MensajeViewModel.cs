using System;
using Newtonsoft.Json;

namespace salon_web_api.ViewModels
{
    public class MensajeViewModel
    {
        [JsonProperty("mensaje")]
        public string mensaje { get; set; }
        [JsonProperty("tipo")]
        public int tipo { get; set; }
    }
}
