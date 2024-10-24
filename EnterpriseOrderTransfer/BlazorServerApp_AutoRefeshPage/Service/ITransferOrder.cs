using BlazorServerApp_AutoRefeshPage.Model;

namespace BlazorServerApp_AutoRefeshPage.Service

{
    public interface ITransferOrder
    {
        Task<List<erpTransferOrder>> GetTransferOrders();
    }
}
