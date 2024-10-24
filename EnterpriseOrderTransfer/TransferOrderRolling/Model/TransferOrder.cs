using System.ComponentModel.DataAnnotations;

namespace TransferOrderRolling.Model
{
    public class erpTransferOrder
    {
        public string recordType { get; set; }
        [Key]
        public Int64? internalId { get; set; }
        public DateTime? tranDate { get; set; }
        public string type { get; set; }
        public string documentNumber { get; set; }
        public string status { get; set; }
        public string location { get; set; }
        public string transferLocation { get; set; }
        public string createdBy { get; set; }
        public string deliveryInstruction { get; set; }
    }
}
