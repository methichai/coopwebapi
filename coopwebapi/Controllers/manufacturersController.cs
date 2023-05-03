using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; //ef
using coopwebapi.Models; // model -> manufacturers
using coopwebapi.Database; // DbContext

namespace coopwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class manufacturersController : ControllerBase
    {

        // Variable
        private readonly DataDbContext _dbContext;
        //Constructure Method
        public manufacturersController(DataDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        // get post put delete

        [HttpGet]
        public async Task<ActionResult<List<manufacturers>>> getManufacturers()
        {
            var manufacturers = await _dbContext.manufacturers.ToListAsync();
            if (manufacturers.Count == 0)
            {
                return NotFound();
            }
            return Ok(manufacturers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<manufacturers>> getManufacturerById(int id)
        {
            var manufacturer = await _dbContext.manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return Ok(manufacturer);
        }

        [HttpPost]
        public async Task<ActionResult<manufacturers>> PostManufacturer(manufacturers manufacturer)
        {
            _dbContext.manufacturers.Add(manufacturer);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetManufacturers", new { id = manufacturer.Id }, manufacturer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<manufacturers>> PutManufacturer(int id, manufacturers manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(manufacturer).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync(); //update database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(ManufacturerExist(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<manufacturers>> DeleteManufacturer(int id)
        {
            var manufacturer = await _dbContext.manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            _dbContext.manufacturers.Remove(manufacturer);
            await _dbContext.SaveChangesAsync();//delete data in database

            return Ok();
        }

        private bool ManufacturerExist(int id)
        {
            return _dbContext.manufacturers.Any(manufacturer => manufacturer.Id == id);
        }


    }
}
