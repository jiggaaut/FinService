using System;
using Xunit;
using FinBack.Controllers;
using FinBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace FinBack.Tests
{
    public abstract class ClientsControllerTest
    {
        protected ClientsControllerTest(DbContextOptions<Context> contextOptions)
        {
            ContextOptions = contextOptions;
            Seed();
        }
        protected DbContextOptions<Context> ContextOptions { get; }
        private void Seed()
        {
            using (var context = new Context(ContextOptions))
            {
                context.Database.EnsureDeleted();

                context.Database.EnsureCreated();

                List<Client> L9 = new List<Client>();

                for (int i = 1; i <= 50; i++)
                {
                    Client client = new Client { Name = $"TestName{i}" };
                    L9.Add(client);
                }
                context.Clients.AddRange(L9);

                context.SaveChanges();
                List<Balance> W8 = new List<Balance>();
                for (int i = 1; i <= 50; i++)
                {
                    Balance balance = new Balance { AccountName = $"TestBalance{i}", ClientId = i, Amount = 0 };
                    W8.Add(balance);
                }
                context.Balances.AddRange(W8);
                context.SaveChanges();                
            }
        }

        [Fact]
        public void Can_get_clients()
        {
            var c1 = new Context(ContextOptions);
            var c2 = new Context(ContextOptions);
            var c3 = new Context(ContextOptions);
            var c4 = new Context(ContextOptions);
            var c5 = new Context(ContextOptions);
            var c6 = new Context(ContextOptions);
            var c7 = new Context(ContextOptions);
            var c8 = new Context(ContextOptions);
            var c9 = new Context(ContextOptions);
            var c10 = new Context(ContextOptions);
            var cc1 = new ClientsController(c1);
            var cc2 = new ClientsController(c2);
            var cc3 = new ClientsController(c3);
            var cc4 = new ClientsController(c4);
            var cc5 = new ClientsController(c5);
            var cc6 = new ClientsController(c6);
            var cc7 = new ClientsController(c7);
            var cc8 = new ClientsController(c8);
            var cc9 = new ClientsController(c9);
            var cc10 = new ClientsController(c10);
            ParallelLoopResult res = Parallel.ForEach(new List<ClientsController> { cc1, cc2, cc3, cc4, cc5, cc6, cc7, cc8, cc9, cc10 }, Do);

            if (res.IsCompleted)
            {
                using (Context context = new Context(ContextOptions))
                {
                    ClientsController controller = new ClientsController(context);
                    var result = controller.GetClients().Result.Value;
                    int sum = 0;
                    string str = "";
                    string ids = "";
                    foreach (Client c in result)
                    {
                        str += c.Balances.First().Amount + " ";
                        sum += c.Balances.First().Amount;
                        ids += c.Balances.First().Id + " ";
                    }
                    Assert.Equal("100", Convert.ToString((string)sum.ToString())); //Строковая строка
                }
            }
        }

        static Random random = new Random();
        public void Do(ClientsController controller)
        {
            for (int i = 1; i<=10;i++)
            {
                int a = random.Next(1, 50);
                int val = 1; //random.Next(100, 1000);
                controller.AddBalance(a, val).Wait(); // Не хочет дожидаться catch               
            }            
        }
    }
}
