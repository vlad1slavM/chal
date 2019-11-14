using System;
using System.Collections.Generic;

namespace Challenge.DataContracts
{
    public class Round
    {
        public string Id { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public bool CanChooseType { get; set; }
        public TimeSpan SoftDeadline { get; set; }
        public TimeSpan HardDeadline { get; set; }
        public List<TaskType> TaskTypes { get; set; }
    }
}
