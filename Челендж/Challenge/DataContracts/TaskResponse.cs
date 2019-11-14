using System;

namespace Challenge.DataContracts
{
    public class TaskResponse
    {
        public string Id { get; set; }
        public string ChallengeId { get; set; }
        public string TeamId { get; set; }
        public string TypeId { get; set; }
        public string RoundId { get; set; }

        public string Question { get; set; }
        public string UserHint { get; set; }

        public DateTime SoftDeadline { get; set; }
        public DateTime HardDeadline { get; set; }

        public DateTime TakeTimestamp { get; set; }
        public DateTime? AnswerTimestamp { get; set; }

        public DateTime ResponseTime = DateTime.UtcNow;

        public string TeamAnswer { get; set; }
        public TaskStatus Status { get; set; }
        public int Points { get; set; }
        public int Cost { get; set; }
    }
}
