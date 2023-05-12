using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Inventory.Transfer
{
    public class Transfer
    {

        [JsonProperty("Quantity Ordered")]
        public int QuantityOrdered;
        [JsonProperty("Equipment Id")]
        public string EquipmentId;
        [JsonProperty("Order Date")]
        public DateTime OrderDate;

        public Transfer() { }

        public Transfer(int quantity, Equipment.Equipment equipment)
        {
            QuantityOrdered = quantity;
            EquipmentId = equipment.Id;
            OrderDate = DateTime.Now;
        }

        public bool Arrived()
        {
            return (DateTime.Now - OrderDate).TotalDays >= 1;
        }
    }
}
