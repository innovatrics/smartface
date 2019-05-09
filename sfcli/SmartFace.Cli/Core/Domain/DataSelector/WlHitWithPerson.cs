using SmartFace.ODataClient.SmartFace.Data.Models.Core;

namespace SmartFace.Cli.Core.Domain.DataSelector
{
    public class WlHitWithPerson : WlHit
    {
        public Person Person { get; }

        public WlHitWithPerson(Person person)
        {
            Person = person;
        }
    }
}
