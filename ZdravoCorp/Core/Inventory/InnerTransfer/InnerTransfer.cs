using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Inventory.InnerTransfer
{
    public class InnerTransfer
    {
        [JsonProperty("Quantity Transfered")]
        public int QuantityTransfered;
        [JsonProperty("Equipment Id")]
        public string EquipmentId;
        [JsonProperty("Delivery Date")]
        public DateTime DeliveryDate;
        [JsonProperty("Room Id")]
        public string RoomId;
        [JsonProperty("Delivery Room Id")]
        public string DeliveryRoomId;

        public InnerTransfer() { }

        public InnerTransfer(int quantity, InventoryItemData item, string deliveryRoomId, DateTime deliveryDate)
        {
            QuantityTransfered = quantity;
            EquipmentId = item.EquipmentId;
            DeliveryRoomId = deliveryRoomId;
            DeliveryDate = deliveryDate;
            RoomId = item.RoomId;
        }

        public InnerTransfer(int quantity, InventoryItemData item, string deliveryRoomId)
        {
            QuantityTransfered = quantity;
            EquipmentId = item.EquipmentId;
            DeliveryRoomId = deliveryRoomId;
            DeliveryDate = DateTime.Now;
            RoomId = item.RoomId;
        }

        public bool Arrived()
        {
            return DeliveryDate<=DateTime.Now;
        }


        public bool IsItems(InventoryItemData item)
        {
            return EquipmentId == item.EquipmentId && RoomId == item.RoomId;
        }
    }
}
