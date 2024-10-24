using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterprisesOrderTransfer.ServiceLibaray
{
    public class createdby
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class transferlocation
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class statusref
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class internalid
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class type
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class location
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class transferOrderValues
    {
        public IList<internalid> internalid { get; set; }
        public string trandate { get; set; }
        public IList<type> type { get; set; }
        public string tranid { get; set; }
        public IList<statusref> statusref { get; set; }
        public IList<location> location { get; set; }
        public IList<transferlocation> transferlocation { get; set; }
        public IList<createdby> createdby { get; set; }
        public string custbody_cts_delivery_instruction { get; set; }


    }

    public class transferOrderRoot
    {
        public string recordType { get; set; }
        public string id { get; set; }
        public transferOrderValues values { get; set; }
    }


    public class transferFlatList
    {
        public string recordType { get; set; }
        public Int64? id { get; set; }
        public string internalidValue { get; set; }
        public string internalidText { get; set; }
        public string trandate { get; set; }
        public string typeValue { get; set; }
        public string typeText { get; set; }
        public string tranid { get; set; }
        public string statusValue { get; set; }
        public string statusText { get; set; }
        public string locationValue { get; set; }
        public string locationText { get; set; }
        public string transferLocationValue { get; set; }
        public string transferLocationText { get; set; }
        public string createdByValue { get; set; }
        public string createdByText { get; set; }
        public string deliveryInstruction { get; set; }
        public int dataPullPart { get; set; }

    }

}
