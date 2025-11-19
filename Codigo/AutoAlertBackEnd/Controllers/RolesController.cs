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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
    {
        var roles = await _roleRepository.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Roles>> GetRole(Guid id)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);

        if (role == null)
            return NotFound();

        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<Roles>> CreateRole(Roles role)
    {
        var existingRole = await _roleRepository.GetRoleByNameAsync(role.Name);
        if (existingRole != null)
            return BadRequest("A role with this name already exists");

        var createdRole = await _roleRepository.CreateRoleAsync(role);
        return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(Guid id, Roles role)
    {
        if (id != role.Id)
            return BadRequest();

        var updatedRole = await _roleRepository.UpdateRoleAsync(role);
        if (updatedRole == null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var result = await _roleRepository.DeleteRoleAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<Roles>> GetRoleByName(string name)
    {
        var role = await _roleRepository.GetRoleByNameAsync(name);

        if (role == null)
            return NotFound();

        return Ok(role);
    }
}