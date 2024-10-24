using Microsoft.EntityFrameworkCore;
using TransferOrderRolling.Model;

namespace TransferOrderRolling.Service
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
