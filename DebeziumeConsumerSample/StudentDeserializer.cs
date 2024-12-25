using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

public class StudentDeserializer : IDeserializer<Rootobject>
{
    public Rootobject Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return null;
        }
        var json = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<Rootobject>(json);
    }
}
