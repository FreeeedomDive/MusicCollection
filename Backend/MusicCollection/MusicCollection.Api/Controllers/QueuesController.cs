using Microsoft.AspNetCore.Mvc;
using MusicCollection.Api.Dto.Exceptions;
using MusicCollection.Api.Dto.FileSystem;
using MusicCollection.Api.Dto.Queues;
using MusicCollection.BusinessLogic.Services.QueuesService;

namespace MusicCollection.Controllers;

[ApiController]
[Route("api/[controller]/{userId:guid}")]
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

    [HttpGet("context")]
    public async Task<ActionResult<FileSystemNode>> GetCurrentQueueContext([FromRoute] Guid userId)
    {
        try
        {
            return await queuesService.GetCurrentContextAsync(userId);
        }
        catch (QueueNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete]
    public async Task<ActionResult> ClearQueueAsync([FromRoute] Guid userId)
    {
        await queuesService.ClearQueueAsync(userId);
        return Ok();
    }

    [HttpGet("list")]
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
        try
        {
            return await queuesService.MovePreviousAsync(userId);
        }
        catch (QueueNotFoundException)
        {
            return NotFound();
        }
        catch (QueueIndexOutOfRangeException)
        {
            return Conflict();
        }
    }

    [HttpPost("move/next")]
    public async Task<ActionResult<QueueTrack>> MoveNextAsync([FromRoute] Guid userId)
    {
        try
        {
            return await queuesService.MoveNextAsync(userId);
        }
        catch (QueueNotFoundException)
        {
            return NotFound();
        }
        catch (QueueIndexOutOfRangeException)
        {
            return Conflict();
        }
    }

    [HttpPost("move/{nextPosition:int}")]
    public async Task<ActionResult<QueueTrack>> MoveToPositionAsync([FromRoute] Guid userId, [FromRoute] int nextPosition)
    {
        try
        {
            return await queuesService.MoveToPositionAsync(userId, nextPosition);
        }
        catch (QueueNotFoundException)
        {
            return NotFound();
        }
        catch (QueueIndexOutOfRangeException)
        {
            return Conflict();
        }
    }

    [HttpPost("shuffle/{shuffle:bool}")]
    public async Task<ActionResult> UpdateShuffle([FromRoute] Guid userId, [FromRoute] bool shuffle)
    {
        await queuesService.UpdateWithShuffleAsync(userId, shuffle);
        return Ok();
    }

    private readonly IQueuesService queuesService;
}