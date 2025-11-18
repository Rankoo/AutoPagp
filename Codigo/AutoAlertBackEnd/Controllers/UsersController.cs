using AutoAlertBackEnd.Dtos;
using AutoAlertBackEnd.Models;
using AutoAlertBackEnd.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AutoAlertBackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Users>> GetUser(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<Users>> CreateUser(CreateUserDto newUser)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(newUser.Email);
        if (existingUser != null)
            return BadRequest("El usuario ya se encuentra registrado");

        var createdUser = await _userRepository.CreateUserAsync(newUser);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, Users user)
    {
        if (id != user.Id)
            return BadRequest();

        var updatedUser = await _userRepository.UpdateUserAsync(user);
        if (updatedUser == null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userRepository.DeleteUserAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<Users>> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}