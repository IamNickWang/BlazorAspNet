using BlazorServerApp_AutoRefeshPage.Model;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerApp_AutoRefeshPage.Service
{
    public class TransferOrderService : ITransferOrder
    {
        private readonly DatabaseContext _dbt;

        public TransferOrderService(DatabaseContext ctx)
        {
            _dbt = ctx;
        }
        public async Task<List<erpTransferOrder>> GetTransferOrders()
        {

            return await _dbt.erpTransferOrder.ToListAsync();

        }
    }
}
