using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
                services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>()
                        .AddSingleton<IDeserializer<Rootobject>, StudentDeserializer>()
                        .AddHostedService<ConsumerHostedService>());
}

public class Rootobject
{
    public Schema schema { get; set; }
    public Payload payload { get; set; }
}

public class Schema
{
    public string type { get; set; }
    public Field[] fields { get; set; }
    public bool optional { get; set; }
    public string name { get; set; }
    public int version { get; set; }
}

public class Field
{
    public string type { get; set; }
    public Field1[] fields { get; set; }
    public bool optional { get; set; }
    public string name { get; set; }
    public string field { get; set; }
    public int version { get; set; }
}

public class Field1
{
    public string type { get; set; }
    public bool optional { get; set; }
    public string field { get; set; }
    public string name { get; set; }
    public int version { get; set; }
    public Parameters parameters { get; set; }
    public string _default { get; set; }
}

public class Parameters
{
    public string allowed { get; set; }
}

public class Payload
{
    public object before { get; set; }
    public After after { get; set; }
    public Source source { get; set; }
    public object transaction { get; set; }
    public string op { get; set; }
    public long ts_ms { get; set; }
    public long ts_us { get; set; }
    public long ts_ns { get; set; }
}

public class After
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Source
{
    public string version { get; set; }
    public string connector { get; set; }
    public string name { get; set; }
    public long ts_ms { get; set; }
    public string snapshot { get; set; }
    public string db { get; set; }
    public object sequence { get; set; }
    public long ts_us { get; set; }
    public long ts_ns { get; set; }
    public string schema { get; set; }
    public string table { get; set; }
    public string change_lsn { get; set; }
    public string commit_lsn { get; set; }
    public int event_serial_no { get; set; }
}