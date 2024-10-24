using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerApp_AutoRefeshPage.Model
{
        public class DatabaseContext : DbContext
        {
            public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }

            public DbSet<erpTransferOrder> erpTransferOrder { get; set; }

        }

}
