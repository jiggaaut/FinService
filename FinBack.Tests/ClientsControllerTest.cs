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
            //using (Context context = new Context(ContextOptions))
            //{
                //ClientsController controller = new ClientsController(context);
                //Clients1Controller controller1 = new Clients1Controller(context);

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
                ParallelLoopResult res = Parallel.ForEach(new List<Context> { c1, c2, c3, c4/*, c5, c6, c7, c8, c9, c10*/ }, Do);

                /*async void XC(Context context)
                {
                    await controller.AddBalance(1, 1);
                }*/
            //}

            using (Context context = new Context(ContextOptions))
            {
                ClientsController controller = new ClientsController(context);
                var result = controller.GetClients().Result.Value;
                int sum = 0;
                string str = "";
                foreach (Client c in result)
                {
                    str += c.Balances.First().Amount + " ";
                    sum += c.Balances.First().Amount;
                }
                Assert.Equal("100", sum.ToString());
            }
        }

        public void Do(Context context)
        {
            Random random = new Random();
            for (int j = 1; j <= 10; j++)
            {
                int a = 1;//random.Next(1, 10);
                int val = 1; //random.Next(100, 1000);
                var newbalance = context.Balances.FirstOrDefault(x => x.ClientId == a);
                newbalance.Amount += val;
                context.SaveChanges();
            }
        }

    }
}
