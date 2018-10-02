using Newtonsoft.Json;
using System;

namespace GettingThingsDone.Contracts.Dto
{
    public class ActionDto : EntityDto
    {
        public string Title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DueDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DoOn { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DoneAt { get; set; }
        public bool IsDone => DoneAt != null;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ListId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ProjectId { get; set; }
        public int UserId { get; set; }
    }
}