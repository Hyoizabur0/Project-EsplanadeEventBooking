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
    public class LocationController : ControllerBase
    {
        //Refactored
        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        //public LocationsController(ApplicationDbContext context)
        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Locations
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        public async Task<IActionResult> GetLocations()
        {
            //return await _context.Locations.ToListAsync();
            var locations = await _unitOfWork.Locations.GetAll();
            return Ok(locations);
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<Location>> GetLocation(int id)
        public async Task<IActionResult> GetLocation(int id)
        {
            //var location = await _context.Locations.FindAsync(id);
            var location = await _unitOfWork.Locations.Get(q => q.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            //_context.Entry(location).State = EntityState.Modified;
            _unitOfWork.Locations.Update(location);

            try
            {
                //await _context.SaveChangesAsync();
                await _unitOfWork.Save(HttpContext);    
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!LocationExists(id))
                if (!await LocationExists(id))
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

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            //_context.Locations.Add(location);
            //await _context.SaveChangesAsync();
            await _unitOfWork.Locations.Insert(location);
            await _unitOfWork.Save(HttpContext);

            return CreatedAtAction("GetLocation", new { id = location.Id }, location);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            //var location = await _context.Locations.FindAsync(id);
            var location = await _unitOfWork.Locations.Get(q => q.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            //_context.Locations.Remove(location);
            //await _context.SaveChangesAsync();
            await _unitOfWork.Locations.Delete(id);
            await _unitOfWork.Save(HttpContext);

            return NoContent();
        }

        private async Task<bool> LocationExists(int id)
        {
            //return _context.Locations.Any(e => e.Id == id);
            var location = await _unitOfWork.Locations.Get(q => q.Id == id);
            return location != null;
        }
    }
}
