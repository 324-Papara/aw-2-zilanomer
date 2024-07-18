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

        // Tüm müþterileri ve iliþkili verileri getirir
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> Get()
        {
            var customers = await unitOfWork.CustomerRepository
                .Include(x => x.CustomerAddresses, x => x.CustomerPhones, x => x.CustomerDetail)  //INCLUDE metodu ile iliþkili verileri çekmiþ oluyoruz.
                .ToListAsync();
            return Ok(customers);
        }
        // Belirli bir müþteri ID'sine sahip müþteri ve iliþkili verileri getirir
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

        // isimle arama yaparýz.
        [HttpGet("search")]
        public async Task<ActionResult<List<Customer>>> Search([FromQuery] string name)
        {
            var customers = await unitOfWork.CustomerRepository
                .Where(x => x.FirstName == name || x.LastName == name);  // WHERE metodu ile dinamik eþleþen müþterileri getiririz.
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

        // mevcut kaydý sileriz.
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

        // mevcut olan kaydý sileriz.
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