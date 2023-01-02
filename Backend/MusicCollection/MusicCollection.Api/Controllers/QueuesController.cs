using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Queues;
using MusicCollection.BusinessLogic.Services.QueuesService;

namespace MusicCollection.Controllers;

[ApiController]
[Route("queues/{userId:guid}")]
public class QueuesController : Controller
{
    public QueuesController(IQueuesService queuesService)
    {
        this.queuesService = queuesService;
    }

    [HttpPost("create/{contextId:guid}")]
    public async Task<ActionResult> CreateQueueAsync([FromRoute] Guid userId, [FromRoute] Guid contextId)
    {
        await queuesService.CreateQueueAsync(userId, contextId);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> ClearQueueAsync([FromRoute] Guid userId)
    {
        await queuesService.ClearQueueAsync(userId);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<QueueTrack[]>> GetQueue([FromRoute] Guid userId)
    {
        return await queuesService.GetQueue(userId);
    }

    [HttpGet("current")]
    public async Task<ActionResult<QueueTrack?>> GetCurrentAsync([FromRoute] Guid userId)
    {
        return await queuesService.GetCurrentAsync(userId);
    }

    [HttpPost("move/previous")]
    public async Task<ActionResult<QueueTrack>> MovePreviousAsync([FromRoute] Guid userId)
    {
        return await queuesService.MovePreviousAsync(userId);
    }

    [HttpPost("move/next")]
    public async Task<ActionResult<QueueTrack>> MoveNextAsync([FromRoute] Guid userId)
    {
        return await queuesService.MoveNextAsync(userId);
    }

    [HttpPost("move/{nextPosition:int}")]
    public async Task<ActionResult<QueueTrack>> MoveToPositionAsync([FromRoute] Guid userId, [FromRoute] int nextPosition)
    {
        return await queuesService.MoveToPositionAsync(userId, nextPosition);
    }

    private readonly IQueuesService queuesService;
}