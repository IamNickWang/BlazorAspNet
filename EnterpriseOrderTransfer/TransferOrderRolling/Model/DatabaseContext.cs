using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace TransferOrderRolling.Model
{
    public class DatabaseContext : DbContext
    {
        private readonly DatabaseContext _dbt;
        private readonly IConfiguration _config;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        //public DbSet<erpTransferOrder> erpTransferOrder { get; set; }

        public async Task<List<erpTransferOrder>> LoadData<erptransferOrder>(string sql, string conn)
        {

            using (IDbConnection connection = new SqlConnection(conn))
            {
                var data = await connection.QueryAsync<erpTransferOrder>(sql);

                return data.ToList();
            }

        }

    }
   
}
