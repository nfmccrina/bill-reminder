using Newtonsoft.Json;

namespace BillReminderService.Service.Models
{
    public class Bill
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dayOfMonth")]
        public int DayOfMonth { get; set; }

        [JsonProperty("reminder")]
        public string ReminderMessage { get; set; }
    }
}