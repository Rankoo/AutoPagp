using AutoAlertBackEnd.Models;
using AutoAlertBackEnd.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoAlertBackEnd.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyRepository _repo;

    public CompaniesController(ICompanyRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("GetAllCompanies")]
    public async Task<ActionResult<IEnumerable<Companies>>> GetAll()
    {
        try
        {
            var list = await _repo.GetAllAsync();
            return Ok(list);
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("GetCompanyById/{id}")]
    public async Task<ActionResult<Companies>> Get(Guid id)
    {
        try
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("CreateCompany")]
    public async Task<ActionResult<Companies>> Create(Companies company)
    {
        try
        {
            var created = await _repo.CreateAsync(company);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPut("UpdateCompany/{id}")]
    public async Task<IActionResult> Update(Guid id, Companies company)
    {
        try {
            if (id != company.Id)
                return BadRequest();
            var updated = await _repo.UpdateAsync(company);
            if (updated == null)
                return NotFound();
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete("DeleteCompany/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var ok = await _repo.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
