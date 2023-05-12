using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Inventory
{
    public class InventoryItem
    {
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
        [JsonProperty("Equipmnt Id")]
        public string EquipmentId { get; set; }
        [JsonProperty("Room Id")]
        public string RoomId { get; set; }

        public InventoryItem() { }

        public InventoryItem(Room.Room room, Equipment.Equipment equipment)
        {
            Quantity = 0;
            if (room != null)
            {
                RoomId = room.Id;
            }
            

            if (equipment != null)
            {
                EquipmentId = equipment.Id;
            }
            
        }
    }
}
