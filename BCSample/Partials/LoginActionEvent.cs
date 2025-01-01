using Avro;
using Avro.Specific;

namespace BCSample.Events
{
    public interface IEvent: ISpecificRecord
    {
    }
    public partial class LoginActionEvent: IEvent
    {

    }
}

