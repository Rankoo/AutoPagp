using AutoAlertBackEnd.Models;
using AutoAlertBackEnd.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoAlertBackEnd.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupRepository _repo;

    public GroupsController(IGroupRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("GetAllGroups")]
    public async Task<ActionResult<IEnumerable<Groups>>> GetAll()
    {
        try {
            var list = await _repo.GetAllAsync();
            return Ok(list);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("GetGroupById/{id}")]
    public async Task<ActionResult<Groups>> Get(Guid id)
    {
        try {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return
                NotFound();
            return 
                Ok(item);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("CreateGroup")]
    public async Task<ActionResult<Groups>> Create(Groups group)
    {
        try {
            var created = await _repo.CreateAsync(group);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("UpdateGroup/{id}")]
    public async Task<IActionResult> Update(Guid id, Groups group)
    {
        try {
            if (id != group.Id)
                return BadRequest();
            var updated = await _repo.UpdateAsync(group);
            if (updated == null)
                return NotFound();
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete("DeleteGroup/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try {
            var ok = await _repo.DeleteAsync(id);
            if (!ok)
                return NotFound();
            return NoContent();

        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
