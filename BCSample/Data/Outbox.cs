namespace BCSample.Data
{
    public class Outbox
    {
       public long Id { get; set; }
        public string Key { get; set; } 
        public string EventType { get; set; }   
        public string Value { get; set; }   
        public int SchemaId { get; set; }
        public string SchemaVersion { get; set; }
        public string CreatedAt { get; set; }

        public Outbox(string eventType,string payload,int schemaId)
        {
            this.EventType = eventType;
            Value = payload;
            SchemaId = schemaId;
        }
    }
}
