using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Para.Data.Context;
using Para.Data.Domain;
using Para.Data.UnitOfWork;

namespace Para.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // T�m m��terileri ve ili�kili verileri getirir
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            var customers = await unitOfWork.CustomerRepository
                .Include(x => x.CustomerAddresses, x => x.CustomerPhones, x => x.CustomerDetail)  //INCLUDE metodu ile ili�kili verileri �ekmi� oluyoruz.
                .ToListAsync();
            return Ok(customers);
        }
        // Belirli bir m��teri ID'sine sahip m��teri ve ili�kili verileri getirir
        [HttpGet("{customerId}")]
        public async Task<ActionResult<Customer>> Get(long customerId)
        {
            var customer = await unitOfWork.CustomerRepository
                .Include(x => x.CustomerAddresses, x => x.CustomerPhones, x => x.CustomerDetail)
                .FirstOrDefaultAsync(x => x.Id == customerId);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        // isimle arama yapar�z.
        [HttpGet("search")]
        public async Task<ActionResult<List<Customer>>> Search([FromQuery] string name)
        {
            var customers = await unitOfWork.CustomerRepository
                .Where(x => x.FirstName == name || x.LastName == name);  // WHERE metodu ile dinamik e�le�en m��terileri getiririz.
            return Ok(customers);
        }

        // yeni kayit ekleme
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer value)
        {
            if (value == null)
                return BadRequest("Customer cannot be null");

            await unitOfWork.CustomerRepository.Insert(value);
            await unitOfWork.Complete();
            return CreatedAtAction(nameof(Get), new { customerId = value.Id }, value);
        }

        // mevcut kayd� sileriz.
        [HttpPut("{customerId}")]
        public async Task<IActionResult> Put(long customerId, [FromBody] Customer value)
        {
            if (value == null)
                return BadRequest("Customer cannot be null");

            var existingCustomer = await unitOfWork.CustomerRepository.GetById(customerId);
            if (existingCustomer == null)
                return NotFound();

            value.Id = existingCustomer.Id; // ID'yi korur
            await unitOfWork.CustomerRepository.Update(value);
            await unitOfWork.Complete();
            return NoContent();
        }

        // mevcut olan kayd� sileriz.
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> Delete(long customerId)
        {
            var existingCustomer = await unitOfWork.CustomerRepository.GetById(customerId);
            if (existingCustomer == null)
                return NotFound();

            await unitOfWork.CustomerRepository.Delete(existingCustomer);
            await unitOfWork.Complete();
            return NoContent();
        }
    }
}