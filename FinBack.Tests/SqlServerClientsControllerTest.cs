using FinBack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinBack.Tests
{
    public class SqlServerClientsControllerTest : ClientsControllerTest
    {
        public SqlServerClientsControllerTest() : base (
            new DbContextOptionsBuilder<Context>()
                    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestDB;ConnectRetryCount=0;MultipleActiveResultSets=true;")
                    .Options)
        {
        }        
    }
}
