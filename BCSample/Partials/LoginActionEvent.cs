using Avro;

namespace BCSample.Events
{

    public partial class LoginActionEvent
    {
        public string UserName { get; set; }
        public string LoginTime { get; set; }

    }

}

