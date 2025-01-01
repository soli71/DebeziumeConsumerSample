namespace BCSample.Data
{
    public class Outbox
    {
       public long Id { get; set; }
        public string Key { get; set; } 
        public string EventType { get; set; }   
        public string Payload { get; set; }   
        public int SchemaId { get; set; }

        public string CreatedAt { get; set; }

        public Outbox(string key,string eventType,string payload,int schemaId)
        {
            EventType = eventType;
            Key=key;
            Payload = payload;
            SchemaId = schemaId;
            CreatedAt = DateTime.Now.ToString();
        }
        public Outbox() { }
    }
}
