using Microsoft.EntityFrameworkCore;

namespace TransferOrderRolling.Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<erpTransferOrder> erpTransferOrder { get; set; }

    }
   
}
