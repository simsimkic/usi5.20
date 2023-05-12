using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Equipment;
using ZdravoCorp.Core.Room;

namespace ZdravoCorp.Core.Inventory
{
    public class InventoryItemData
    {
        public int Quantity { get; set; }
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public bool EquipmentDinamic { get; set; }
        public string RoomId { get; set; }
        public RoomType RoomType { get; set; }

        public InventoryItemData(int quantity, Equipment.Equipment equipment, Room.Room room) {
            Quantity = quantity;
            EquipmentId = equipment.Id;
            EquipmentName = equipment.Name;
            EquipmentType = equipment.Type;
            EquipmentDinamic = equipment.Dynamic;
            RoomId = room.Id;
            RoomType = room.Type;
        }
    }
}