using Avro;

namespace BCSample.Partials
{

    public partial class LoginActionEvent
    {
        public string UserName { get; set; }
        public DateTime LoginTime { get; set; }

    }

}

