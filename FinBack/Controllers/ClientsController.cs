using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinBack.Models;
using System.Linq.Expressions;
using System.Threading;

namespace FinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        public readonly Context _context;
        object locker = new object();
        public ClientsController(Context context)
        {
            _context = context;
        }


        // GET: api/Clients
        // Информация о всех клиентах и их балансах
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.Include(x => x.Balances).ToListAsync();
        }

        // GET: api/Clients/5/
        //Информация о одном клиенте и его балансах
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.Include(x=>x.Balances).FirstOrDefaultAsync(y=>y.Id==id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // POST: api/Clients
        // "Регистрация"
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            Balance balance = new Balance(client.Id);
            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        [HttpPut("{id}/{action}/{am}")]
        // Put: api/Clients/1/AddBalance/300
        public async Task<ActionResult<Client>> AddBalance(int id, int am)
        {
            if (ClientBalanceExists(id))
            {
                var newbalance = await _context.Balances.FirstOrDefaultAsync(x => x.ClientId == id);
                newbalance.Amount += am;
                _context.Entry(newbalance).State = EntityState.Modified;
            }
            var saved = false;
            while (!saved)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Balance)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];
                                // TODO: decide which value should be written to database
                                if (property.Name is "Amount")
                                {
                                    int buffer = (int)databaseValues[property] + am;
                                    proposedValues[property] = buffer;
                                }
                            }
                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                    // TODO: обратботка которая не работает
                }
            }
            return NoContent();
            //var client = await _context.Clients.Include(x => x.Balances).FirstOrDefaultAsync(y => y.Id == id);
            //return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
        private bool ClientBalanceExists(int id)
        {
            return _context.Balances.Any(x => x.ClientId == id);
        }
    }
}
