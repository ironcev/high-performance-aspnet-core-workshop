using System;

namespace GettingThingsDone.Contract.Dto
{
    /// <summary>
    /// The Action DTO object.
    /// </summary>
    /// <remarks>
    /// To learn more about Data Transfer Objects (DTOs) see:
    /// https://en.wikipedia.org/wiki/Data_transfer_object
    /// </remarks>
    public class ActionDto
    {
        public int Id { get; set; }
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