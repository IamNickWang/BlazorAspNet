using BlazorServerApp_AutoRefeshPage.Model;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerApp_AutoRefeshPage.Service
{
    public class TransferOrderService : ITransferOrder
    {
        private readonly DatabaseContext _dbt;
        private readonly IConfiguration _config;


        public TransferOrderService(DatabaseContext ctx, IConfiguration config)
        {
            _dbt = ctx;
            _config = config;
        }
        public async Task<List<erpTransferOrder>> GetTransferOrders()
        {
            string sql = "SELECT * FROM erp.netsuiteTransferOrder";
            string connectionString = _config.GetConnectionString("conn");

            return await _dbt.LoadData<erpTransferOrder>(sql, connectionString);

        }
    }
}
