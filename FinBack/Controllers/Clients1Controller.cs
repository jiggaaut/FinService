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
    public class Clients1Controller : ControllerBase, IDisposable
    {
        private readonly Context _context;

        public Clients1Controller(Context context)
        {
            _context = context;
        }


        // GET: api/Clients
        // Информация о всех клиентах и их балансах
        [HttpGet]
        public  ActionResult<IEnumerable<Client>> GetClients()
        {
            return _context.Clients.Include(x => x.Balances).ToList();
        }

        // GET: api/Clients/5/AddBalance?=500
        //Информация о одном клиенте и его балансах
        [HttpGet("{id}")]
        public ActionResult<Client> GetClient(int id)
        {
            var client = _context.Clients.Include(x => x.Balances).FirstOrDefault(y => y.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // POST: api/Clients
        // "Регистрация"
        [HttpPost]
        public ActionResult<Client> PostClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            Balance balance = new Balance(client.Id);
            _context.Balances.Add(balance);
            _context.SaveChanges();
            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        [HttpPut("{id}/{action}/{am}")]
        // Put: api/Clients/1/AddBalance/300
        public ActionResult<Client> AddBalance(int id, int am)
        {
            if (ClientBalanceExists(id))
            {
                Balance newbalance = _context.Balances.FirstOrDefault(x => x.ClientId == id);
                newbalance.Amount += am;
                _context.Entry(newbalance).State = EntityState.Modified;
            }
            _context.SaveChangesAsync();
            var client = _context.Clients.Include(x => x.Balances).FirstOrDefault(y => y.Id == id);
            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
        private bool ClientBalanceExists(int id)
        {
            return _context.Balances.Any(x => x.ClientId == id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                //CloseHandle(handle);
                //handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;
            }
        }
    }
}