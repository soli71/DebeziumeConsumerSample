using BCSample.Events;
using BCSample.Services.Outbox;
using BCSample.Services.SchemaRegistry;
using Microsoft.AspNetCore.Mvc;

namespace BCSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutboxController : ControllerBase
    {
        private readonly IEventOutboxService _eventOutboxService;
        private readonly ISchemaRegistryService _schemaRegistryService;
        public OutboxController(IEventOutboxService eventOutboxService, ISchemaRegistryService schemaRegistryService)
        {
            _eventOutboxService = eventOutboxService;
            _schemaRegistryService = schemaRegistryService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginEventDto loginEventDto )
        {
            await _eventOutboxService.SaveEventToOutboxAsync(new LoginActionEvent { Username=loginEventDto.UserName,LoginTime=DateTime.Now.ToString()});
            return Ok();
        }

        [HttpGet("get-all-subject")]
        public async Task<IActionResult> Get()
        {
            var result=await _schemaRegistryService.GetAllSubjectAsync();
            return Ok(result);
        }

        [HttpGet("subject/{subject}/versions")]
        public async Task<IActionResult> Get([FromRoute] string subject)
        {
            var result = await _schemaRegistryService.GetSubjectVersionsAsync(subject);
            return Ok(result);
        }
    }
}

public class LoginEventDto
{
    public string UserName { get; set; }
}