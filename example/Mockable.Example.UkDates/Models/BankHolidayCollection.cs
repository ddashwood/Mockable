using System.Text.Json.Serialization;

namespace Mockable.Example.UkDates.Models
{
    public class BankHolidayCollection
    {
        [JsonPropertyName("england-and-wales")]
        public Division englandandwales { get; set; } = new Division();

        public Division scotland { get; set; } = new Division();

        [JsonPropertyName("northern-ireland")]
        public Division northernireland { get; set; } = new Division();
    }

    public class Division
    {
        public string division { get; set; } = "";
        public Event[] events { get; set; } = new Event[0];

        internal object firstordefault()
        {
            throw new NotImplementedException();
        }
    }

    public class Event
    {
        public string title { get; set; } = "";
        public DateOnly date { get; set; }
        public string notes { get; set; } = "";
        public bool bunting { get; set; }
    }
}
