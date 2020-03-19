using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector
{
    public class MatchResultWithPerson : MatchResult
    {
        public Person Person { get; }

        public MatchResultWithPerson(Person person)
        {
            Person = person;
        }
    }
}
