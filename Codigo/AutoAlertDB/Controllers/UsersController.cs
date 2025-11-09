using AutoAlertDB.Models;
using AutoAlertDB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AutoAlertDB.Controllers;

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
    public async Task<ActionResult<Users>> CreateUser(Users user)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingUser != null)
            return BadRequest("A user with this email already exists");

        var createdUser = await _userRepository.CreateUserAsync(user);
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