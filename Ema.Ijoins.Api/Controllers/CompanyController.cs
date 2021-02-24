using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ema.Ijoins.Api.EfModels;

namespace Ema.Ijoins.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CompanyController : BaseController
  {
    private readonly ema_databaseContext _context;

    public CompanyController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/Company
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbmCompany>>> GetTbmCompanies()
    {
      return await _context.TbmCompanies.OrderBy(o => o.CompanyId).ToListAsync();
    }

    // GET: api/Company/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TbmCompany>> GetTbmCompany(int id)
    {
      var tbmCompany = await _context.TbmCompanies.FindAsync(id);

      if (tbmCompany == null)
      {
        return NotFound();
      }

      return tbmCompany;
    }

    // PUT: api/Company/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbmCompany(int id, TbmCompany tbmCompany)
    {
      if (id != tbmCompany.CompanyId)
      {
        return BadRequest();
      }

      if (TbmCompanyExists(tbmCompany.CompanyCode))
      {
        return Conflict();
      }

      _context.Entry(tbmCompany).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbmCompanyExists(tbmCompany.CompanyCode))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Company
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbmCompany>> PostTbmCompany(TbmCompany tbmCompany)
    {

      if (TbmCompanyExists(tbmCompany.CompanyCode))
      {
        return Conflict();
      }


      _context.TbmCompanies.Add(tbmCompany);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetTbmCompany", new { id = tbmCompany.CompanyId }, tbmCompany);
    }

    // DELETE: api/Company/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbmCompany(int id)
    {
      var tbmCompany = await _context.TbmCompanies.FindAsync(id);
      if (tbmCompany == null)
      {
        return NotFound();
      }

      _context.TbmCompanies.Remove(tbmCompany);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbmCompanyExists(string CompanyCode)
    {
      return _context.TbmCompanies.Any(e => e.CompanyCode == CompanyCode);
    }
  }
}
