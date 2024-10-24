using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;


namespace BlazorServerApp_AutoRefeshPage.Model
{
        public class DatabaseContext : DbContext
        {
            public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }

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
