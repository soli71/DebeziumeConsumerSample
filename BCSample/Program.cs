using BCSample.Data;
using BCSample.Services.Consumers;
using BCSample.Services.Outbox;
using BCSample.Services.SchemaRegistry;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IEventOutboxService, EventOutboxService>();
builder.Services.AddSingleton<ISchemaRegistryService, SchemaRegistryService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
//builder.Services.AddHostedService<KafkaConsumerHostedService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
