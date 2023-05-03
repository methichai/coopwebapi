using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ef
using coopwebapi.Database; // dbcontext
using coopwebapi.Models; // model -> devices

namespace coopwebapi.Controllers
{
    [Route("api/[controller]")] //url : api/devices
    [ApiController]
    public class devicesController : ControllerBase
    {
        private readonly DataDbContext _DbContext;
        public devicesController(DataDbContext dataDbContext)
        {
            _DbContext = dataDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<devices>>> GetDevices()
        {
            var devices = await _DbContext.devices.ToListAsync();
            if (devices.Count == 0)
            {
                return NotFound();
            }
            return Ok(devices);
        }

        [HttpGet("{id}")] // api/devices/5
        public async Task<ActionResult<devices>> GetDevice(int id)
        {
            var device = await _DbContext.devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }

        [HttpGet("manufacturer/{id}")] // api/devices/manufacturer/5
        public async Task<ActionResult<IEnumerable<devices>>> GetDeviceByManufacturerId(int id)
        {
            var devices = await _DbContext.devices.Where(e => e.Manufacturer_id == id).ToListAsync();
            if (devices.Count == 0)
            {
                return NotFound();
            }
            return Ok(devices);
        }

        [HttpPost]
        public async Task<ActionResult<devices>> PostDevice(devices device)
        {
            _DbContext.devices.Add(device);
            try
            {
                await _DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeviceExist(device.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetDevices", new { id = device.id }, device);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<devices>> PutDevice(int id, devices device)
        {
            if (id != device.id)
            {
                return BadRequest();
            }

            _DbContext.Entry(device).State = EntityState.Modified;
            try
            {
                await _DbContext.SaveChangesAsync(); //update database
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(DeviceExist(id)))
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
        public async Task<ActionResult<devices>> Deleteevice(int id)
        {
            var device = await _DbContext.devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            _DbContext.devices.Remove(device);
            await _DbContext.SaveChangesAsync();//delete data in database

            return Ok();
        }

        private bool DeviceExist(int id)
        {
            return _DbContext.devices.Any(devices => devices.id == id);
        }
    }
}
