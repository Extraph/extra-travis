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
  public class UserCompanyController : BaseController
  {
    private readonly ema_databaseContext _context;

    public UserCompanyController(ema_databaseContext context)
    {
      _context = context;
    }

    // GET: api/UserCompany
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TbUserCompany>>> GetTbUserCompanies()
    {
      return await _context.TbUserCompanies.OrderBy(o => o.Id).ToListAsync();
    }

    // GET: api/UserCompany/5
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<TbUserCompany>>> GetTbUserCompany(string id)
    {
      var tbUserCompany = await _context.TbUserCompanies.Where(w => w.UserId == id).OrderBy(o => o.Id).ToListAsync();

      if (tbUserCompany == null)
      {
        return NotFound();
      }

      return tbUserCompany;
    }

    // PUT: api/UserCompany/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTbUserCompany(int id, TbUserCompany tbUserCompany)
    {
      if (id != tbUserCompany.Id)
      {
        return BadRequest();
      }

      //if (TbUserCompanyExists(tbUserCompany.UserId, tbUserCompany.CompanyId))
      //{
      //  return Conflict();
      //}

      _context.Entry(tbUserCompany).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TbUserCompanyExists(tbUserCompany.UserId, tbUserCompany.CompanyId))
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

    // POST: api/UserCompany
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TbUserCompany>> PostTbUserCompany(TbUserCompany tbUserCompany)
    {
      if (TbUserCompanyExists(tbUserCompany.UserId, tbUserCompany.CompanyId))
      {
        return Conflict();
      }

      _context.TbUserCompanies.Add(tbUserCompany);
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
        if (TbUserCompanyExists(tbUserCompany.UserId, tbUserCompany.CompanyId))
        {
          return Conflict();
        }
        else
        {
          throw;
        }
      }

      return CreatedAtAction("GetTbUserCompany", new { id = tbUserCompany.UserId }, tbUserCompany);
    }

    // DELETE: api/UserCompany/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTbUserCompany(int id)
    {
      var tbUserCompany = await _context.TbUserCompanies.FindAsync(id);
      if (tbUserCompany == null)
      {
        return NotFound();
      }

      _context.TbUserCompanies.Remove(tbUserCompany);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TbUserCompanyExists(string UserId, int CompanyId)
    {
      return _context.TbUserCompanies.Any(e => e.UserId == UserId && e.CompanyId == CompanyId);
    }
  }
}
