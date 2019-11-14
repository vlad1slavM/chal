using System.Collections.Generic;

namespace Challenge.DataContracts
{
    public class ChallengeResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Round> Rounds { get; set; }
    }
}
