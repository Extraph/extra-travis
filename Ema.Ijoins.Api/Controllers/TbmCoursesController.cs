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
    public class TbmCoursesController : ControllerBase
    {
        private readonly ema_databaseContext _context;

        public TbmCoursesController(ema_databaseContext context)
        {
            _context = context;
        }

        // GET: api/TbmCourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbmCourse>>> GetTbmCourses()
        {
            return await _context.TbmCourses.ToListAsync();
        }

        // GET: api/TbmCourses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TbmCourse>> GetTbmCourse(string id)
        {
            var tbmCourse = await _context.TbmCourses.FindAsync(id);

            if (tbmCourse == null)
            {
                return NotFound();
            }

            return tbmCourse;
        }

        // PUT: api/TbmCourses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTbmCourse(string id, TbmCourse tbmCourse)
        {
            if (id != tbmCourse.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(tbmCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TbmCourseExists(id))
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

        // POST: api/TbmCourses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TbmCourse>> PostTbmCourse(TbmCourse tbmCourse)
        {
            _context.TbmCourses.Add(tbmCourse);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TbmCourseExists(tbmCourse.CourseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTbmCourse", new { id = tbmCourse.CourseId }, tbmCourse);
        }

        // DELETE: api/TbmCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTbmCourse(string id)
        {
            var tbmCourse = await _context.TbmCourses.FindAsync(id);
            if (tbmCourse == null)
            {
                return NotFound();
            }

            _context.TbmCourses.Remove(tbmCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TbmCourseExists(string id)
        {
            return _context.TbmCourses.Any(e => e.CourseId == id);
        }
    }
}
