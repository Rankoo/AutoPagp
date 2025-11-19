using System;
using AutoAlertBackEnd.Models;
using AutoAlertBackEnd.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoAlertBackEnd.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RolesController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet("GetAllRoles")]
    public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
    {
        try {
            var roles = await _roleRepository.GetAllRolesAsync();
            return Ok(roles);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("GetRolById/{id}")]
    public async Task<ActionResult<Roles>> GetRole(Guid id)
    {
        try {
            var role = await _roleRepository.GetRoleByIdAsync(id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("CreateRol")]
    public async Task<ActionResult<Roles>> CreateRole(Roles role)
    {
        try {
            var existingRole = await _roleRepository.GetRoleByNameAsync(role.Name);
            if (existingRole != null)
                return BadRequest("A role with this name already exists");

            var createdRole = await _roleRepository.CreateRoleAsync(role);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("UpdateRol/{id}")]
    public async Task<IActionResult> UpdateRole(Guid id, Roles role)
    {
        try {
            if (id != role.Id)
                return BadRequest();

            var updatedRole = await _roleRepository.UpdateRoleAsync(role);
            if (updatedRole == null)
                return NotFound();

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete("DeleteRol/{id}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        try {
            var result = await _roleRepository.DeleteRoleAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("GetRolByName/{name}")]
    public async Task<ActionResult<Roles>> GetRoleByName(string name)
    {
        try {
            var role = await _roleRepository.GetRoleByNameAsync(name);

            if (role == null)
                return NotFound();

            return Ok(role);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}