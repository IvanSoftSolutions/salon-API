using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salon_web_api.Models;

namespace salon_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayment()
        {
            return await _context.Payment.ToListAsync();
        }

        // GET: api/Payment/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<Payment>> GetPayment(int userId)
        {
            var Payment = await _context.Payment.Where(e => e.UserId == userId).SingleOrDefaultAsync();

            if (Payment == null)
            {
                return NotFound();
            }

            return Payment;
        }

        // POST: api/Payment
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Payment>> DeletePayment(int eventoId)
        {
            var evento = await _context.Payment.FindAsync(eventoId);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Payment.Remove(evento);
            await _context.SaveChangesAsync();

            return evento;
        }
    }
}
