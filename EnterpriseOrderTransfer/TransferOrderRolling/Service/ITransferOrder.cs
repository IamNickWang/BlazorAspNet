using TransferOrderRolling.Model;

namespace TransferOrderRolling.Service
{
    public interface ITransferOrder
    {
        Task<List<erpTransferOrder>> GetTransferOrders();
    }
}
