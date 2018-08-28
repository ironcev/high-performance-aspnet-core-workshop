using System;

namespace GettingThingsDone.Contracts.Dto
{
    public class ActionDto : EntityDto
    {
        public string Title { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public DateTimeOffset? DoOn { get; set; }
        public DateTimeOffset? DoneAt { get; set; }
        public bool IsDone => DoneAt != null;
        public int ListId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}