using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinBack.Models;

namespace FinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        private readonly Context _context;

        public BalancesController(Context context)
        {
            _context = context;
        }

        // GET: api/Balances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Balance>>> GetBalances()
        {
            return await _context.Balances.ToListAsync();
        }

        // GET: api/Balances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Balance>> GetBalance(int id)
        {
            var balance = await _context.Balances.FindAsync(id);

            if (balance == null)
            {
                return NotFound();
            }

            return balance;
        }

        // PUT: api/Balances/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBalance(int id, Balance balance)
        {
            if (id != balance.Id)
            {
                return BadRequest();
            }

            _context.Entry(balance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BalanceExists(id))
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

        // POST: api/Balances
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Balance>> PostBalance(Balance balance)
        {
            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBalance", new { id = balance.Id }, balance);
        }

        // DELETE: api/Balances/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Balance>> DeleteBalance(int id)
        {
            var balance = await _context.Balances.FindAsync(id);
            if (balance == null)
            {
                return NotFound();
            }

            _context.Balances.Remove(balance);
            await _context.SaveChangesAsync();

            return balance;
        }

        private bool BalanceExists(int id)
        {
            return _context.Balances.Any(e => e.Id == id);
        }
    }
}
