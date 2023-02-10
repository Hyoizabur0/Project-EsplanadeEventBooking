using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EsplanadeEventBooking.Server.Data;
using EsplanadeEventBooking.Shared.Domain;
using EsplanadeEventBooking.Server.IRepository;

namespace EsplanadeEventBooking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookeventController : ControllerBase
    {
        //Refactored
        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        //public BookeventsController(ApplicationDbContext context)
        public BookeventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Bookevents
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Bookevent>>> GetBookevents()
        public async Task<IActionResult> GetBookevents()
        {
            //return await _context.Bookevents.ToListAsync();
            var bookevents = await _unitOfWork.Bookevents.GetAll(includes: q => q.Include(x => x.Creator).Include(x => x.Location));
            return Ok(bookevents);
        }

        // GET: api/Bookevents/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<Bookevent>> GetBookevent(int id)
        public async Task<IActionResult> GetBookevent(int id)
        {
            //var bookevent = await _context.Bookevents.FindAsync(id);
            var bookevent = await _unitOfWork.Bookevents.Get(q => q.Id == id);

            if (bookevent == null)
            {
                return NotFound();
            }

            return Ok(bookevent);
        }

        // PUT: api/Bookevents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookevent(int id, Bookevent bookevent)
        {
            if (id != bookevent.Id)
            {
                return BadRequest();
            }

            //_context.Entry(bookevent).State = EntityState.Modified;
            _unitOfWork.Bookevents.Update(bookevent);

            try
            {
                //await _context.SaveChangesAsync();
                await _unitOfWork.Save(HttpContext);    
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!BookeventExists(id))
                if (!await BookeventExists(id))
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

        // POST: api/Bookevents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bookevent>> PostBookevent(Bookevent bookevent)
        {
            //_context.Bookevents.Add(bookevent);
            //await _context.SaveChangesAsync();
            await _unitOfWork.Bookevents.Insert(bookevent);
            await _unitOfWork.Save(HttpContext);

            return CreatedAtAction("GetBookevent", new { id = bookevent.Id }, bookevent);
        }

        // DELETE: api/Bookevents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookevent(int id)
        {
            //var bookevent = await _context.Bookevents.FindAsync(id);
            var bookevent = await _unitOfWork.Bookevents.Get(q => q.Id == id);
            if (bookevent == null)
            {
                return NotFound();
            }

            //_context.Bookevents.Remove(bookevent);
            //await _context.SaveChangesAsync();
            await _unitOfWork.Bookevents.Delete(id);
            await _unitOfWork.Save(HttpContext);

            return NoContent();
        }

        private async Task<bool> BookeventExists(int id)
        {
            //return _context.Bookevents.Any(e => e.Id == id);
            var bookevent = await _unitOfWork.Bookevents.Get(q => q.Id == id);
            return bookevent != null;
        }
    }
}
